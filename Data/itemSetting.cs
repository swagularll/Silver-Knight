using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker;

using LitJson;
using System.Collections.Generic;

public class itemSetting : MonoBehaviour
{
    //Add count down for item to dispear.XXX
    //item does not managed by flags
    public string flag_name;
    public ODMVariable.itemCatalogue catalog_code;
    public int item_amount = 1;
    public bool is_document = false;
    public string linked_fsm = "FSM";
    public string special_collect_msg = "";//show special message when picking up an item

    public sceneItemInfo item_info;


    private string default_collect_msg = "showGetItem";//Default message: picked up [item]
    private eventCenter event_center;
    private dialogPanel dialogue_panel;
    private CDocument c_document;
    private ItemDatabase item_db;
    private inventoryDash inventory_dash;
    private void Awake()
    {
        if (!is_document)
        {
            item_info = new sceneItemInfo();
            item_info.item_guid = Guid.NewGuid().ToString();
            item_info.catalog_code = catalog_code;
            item_info.located_level = Application.loadedLevelName;
            item_info.location_x = this.transform.position.x;
            item_info.location_y = this.transform.position.y;
            item_info.location_z = this.transform.position.z;
        }
    }

    private void Start()
    {
        initializeComponent();

        if (is_document)
        {
            if (event_center.getFlagBool(flag_name))
                GameObject.Destroy(transform.gameObject);
            initilaization();
        }
    }
    public void initializeComponent()
    {
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();
        dialogue_panel = ODMObject.event_manager.GetComponent<dialogPanel>();
        item_db = ODMObject.event_manager.GetComponent<ItemDatabase>();
        inventory_dash = ODMObject.event_manager.GetComponent<inventoryDash>();
    }

    public void initilaization()
    {
        if (!String.IsNullOrEmpty(special_collect_msg))
            default_collect_msg = special_collect_msg.Trim();

        PlayMakerFSM fsm = fsmHelper.getFsm(transform.gameObject, linked_fsm.Trim());
        fsm.FsmVariables.GetFsmString(ODMVariable.local.msg_flag_name).Value = default_collect_msg.Trim();
        fsm.FsmVariables.GetFsmString(ODMVariable.local.fsm_name).Value = linked_fsm.Trim();
        fsm.FsmVariables.GetFsmGameObject(ODMVariable.local.self).Value = transform.gameObject;

        if (!String.IsNullOrEmpty(flag_name))
            fsm.FsmVariables.GetFsmString(ODMVariable.local.flag_name).Value = flag_name.Trim();

        fsm.SendEvent(eventName.script_ready);
    }

    public void resetPositionInfo(Vector3 vector3)
    {
        item_info.location_x = vector3.x;
        item_info.location_y = vector3.y;
        item_info.location_z = vector3.z;
    }

    #region For Call
    public void registerItem(GameObject _level_loader)
    {
        _level_loader.GetComponent<afterLoad>().addToItemCollection(transform.gameObject);
    }
    #endregion

    #region Fsm Call
    private void collectItem()
    {
        if (is_document)
        {
            c_document = ODMObject.event_manager.GetComponent<documentDash>().getDocument(flag_name.Trim());
            dialogue_panel.currentSlotItem = c_document.Name.Trim();//For displaying collection message
        }
        else
        {
            int itemID = (int)catalog_code;//Important
            if (itemID != -1)
            {
                dialogue_panel.currentSlotItem = item_db.getItem(itemID).title.Trim();
                if (item_amount == 0)
                    inventory_dash.addItem(itemID);
                else
                {
                    if (itemID == (int)ODMVariable.itemCatalogue.device_battery)
                        itemID = (int)ODMVariable.itemCatalogue.little_bastard;
                    inventory_dash.addItem(itemID, item_amount);
                }
            }
            else
            {
                ODM.errorLog(transform.name, "itemSetting missing itemID.");
            }
        }
    }

    public void setItemCollected()
    {
        if (!is_document)
        {
            ODMObject.event_manager.GetComponent<itemManager>().removeItemRegistration(this.item_info.item_guid);
        }
        else
        {
            ODMObject.event_manager.GetComponent<eventCenter>().setFlagTrue(flag_name);
        }
    }
    #endregion

    #region Local Class
    public class sceneItemInfo
    {
        public string item_guid { get; set; }//For item with same id
        public ODMVariable.itemCatalogue catalog_code { get; set; }//Equals to id in in item
        public int amount { get; set; }
        public string located_level { get; set; }
        public double location_x { get; set; }
        public double location_y { get; set; }
        public double location_z { get; set; }

        public sceneItemInfo()
        {
        }

        public sceneItemInfo(string _json)
        {
            sceneItemInfo item_info = JsonMapper.ToObject<sceneItemInfo>(_json);
            this.catalog_code = item_info.catalog_code;
            this.amount = item_info.amount;
            this.location_x = item_info.location_x;
            this.location_y = item_info.location_y;
            this.location_z = item_info.location_z;
        }
    }
    #endregion

}
