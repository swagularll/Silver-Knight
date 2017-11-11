using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using Assets.Script.ODM_Widget;

public class itemSetting : MonoBehaviour
{
    //Add count down for item to dispear.XXX

    //public bool is_warmbug_item = false; //item does not managed by flags
    public string flag_name;
    public inventoryDash.inventoryItem catalog_code;
    public int item_amount = 1;
    public bool is_document = false;
    public string linked_fsm = "FSM";
    public string special_collect_msg = "";//show special message when picking up an item

    private string default_collect_msg = "showGetItem";//picked up [item]
    private eventCenter event_center;
    private dialogPanel dialogue_panel;
    private CDocument c_document;
    private ItemDatabase item_db;
    private inventoryDash inventory_dash;


    private void Awake()
    {
        catalog_code = inventoryDash.inventoryItem.none;
    }

    private void tryPickUpItem()//Add item, set event callback reference
    {
        if (is_document)
        {
            c_document = ODMObject.event_manager.GetComponent<documentDash>().getDocument(flag_name.Trim());
            dialogue_panel.currentSlotItem = c_document.Name.Trim();//For replacing [INPUT] => ???
        }
        else
        {
            int itemID = (int)catalog_code;
            if (itemID != -1)
            {
                dialogue_panel.currentSlotItem = item_db.getItem(itemID).title.Trim();
                if (item_amount == 0)
                    inventory_dash.addItem(itemID);
                else
                {
                    if (itemID == (int)inventoryDash.inventoryItem.device_battery)
                        itemID = (int)inventoryDash.inventoryItem.little_bastard;
                    inventory_dash.addItem(itemID, item_amount);
                }
            }
            else
            { 
                ODM.errorLog(transform.name, "itemSetting missing itemID.", "");
            }
        }
    }

    //Will only be called when object generates
    public void initilaization()
    {
        if (!String.IsNullOrEmpty(special_collect_msg))
            default_collect_msg = special_collect_msg.Trim();

        event_center = ODMObject.event_manager.GetComponent<eventCenter>();
        dialogue_panel = ODMObject.event_manager.GetComponent<dialogPanel>();
        item_db = ODMObject.event_manager.GetComponent<ItemDatabase>();
        inventory_dash = ODMObject.event_manager.GetComponent<inventoryDash>();

        PlayMakerFSM fsm = fsmHelper.getFsm(transform.gameObject, linked_fsm.Trim());
        fsm.FsmVariables.GetFsmString("msg_flag_name").Value = default_collect_msg.Trim();
        fsm.FsmVariables.GetFsmString("fsm_fame").Value = linked_fsm.Trim();
        fsm.FsmVariables.GetFsmGameObject("self").Value = transform.gameObject;

        if (!String.IsNullOrEmpty(flag_name))
            fsm.FsmVariables.GetFsmString("flag_name").Value = flag_name.Trim();

        fsm.SendEvent("script ready");
    }
}
