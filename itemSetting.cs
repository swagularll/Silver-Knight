using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker;

public class itemSetting : MonoBehaviour
{
    public bool isWarmbugItem = false; //item does not managed by flags
    public string flagName;//normal item will require a flag to avoid multiple pick up
    public inventoryDash.inventoryItem itemEntity = inventoryDash.inventoryItem.none;
    public int itemAmount = 1;
    public bool isDocument = false;
    public string linkedFSM = "FSM";

    private string defaultMsg = "showGetItem";//picked up [item]
    public string specialPickupMsg = "";//show special message when picking up an item

    private eventCenter eventCenter;
    private dialogPanel dialogue;
    private CDocument document;
    private ItemDatabase itemDB;
    private inventoryDash inventoryDash;

    private void tryPickUpItem()//Add item, set event callback reference
    {
        if (!String.IsNullOrEmpty(flagName) || isWarmbugItem)
        {
            if (isDocument)
            {
                document = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value.GetComponent<documentDash>().getDocument(flagName.Trim());
                dialogue.currentSlotItem = document.Name.Trim();//For replacing [INPUT]
            }
            else
            {
                int itemID = (int)itemEntity;
                if (itemID != -1)
                {
                    dialogue.currentSlotItem = itemDB.getItem(itemID).title.Trim();
                    if (itemAmount == 0)
                        inventoryDash.addItem(itemID);
                    else
                    {
                        if (itemID == (int)inventoryDash.inventoryItem.device_battery)
                            itemID = (int)inventoryDash.inventoryItem.little_bastard;
                        inventoryDash.addItem(itemID, itemAmount);
                    }
                }
                else
                    ODM.log(transform.name,  "itemSetting missing itemID.");
            }
        }
        else
            ODM.log(transform.name,  "itemSetting missing flagName.");
    }
    public void scriptSetting()
    {
        if (!String.IsNullOrEmpty(specialPickupMsg))
            defaultMsg = specialPickupMsg.Trim();

        eventCenter = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value.GetComponent<eventCenter>();
        dialogue = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value.GetComponent<dialogPanel>();
        itemDB = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value.GetComponent<ItemDatabase>();
        inventoryDash = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value.GetComponent<inventoryDash>();

        PlayMakerFSM fsm = fsmHelper.getFsm(transform.gameObject, linkedFSM.Trim());
        fsm.FsmVariables.GetFsmString("msgFlagName").Value = defaultMsg.Trim();
        fsm.FsmVariables.GetFsmString("flagName").Value = flagName.Trim();
        fsm.FsmVariables.GetFsmString("fsmName").Value = linkedFSM.Trim();
        fsm.FsmVariables.GetFsmGameObject("self").Value = transform.gameObject;


        if (!isWarmbugItem)
        {
            bool isDone = eventCenter.getFlagBool(flagName);//check if item already been picked up
            if (isDone)
                DestroyObject(transform.gameObject);
        }
        fsm.SendEvent("document ready");


    }
}
