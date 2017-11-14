using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using Assets.Script.ODM_Widget;
using System.Collections.Generic;

public class afterLoad : MonoBehaviour
{
    public List<GameObject> item_entity_collection;
    public List<GameObject> event_entity_collection;

    //For scene drag
    public GameObject obj_warmbug_lair;
    

    private warmbugLair level_warmbug_lair;
    private eventCenter event_center;
    private itemManager item_manager;

    private string map_display_text;
    private string stage_flag_name;

    private void InitialzeScript()
    {
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();
        item_manager = ODMObject.event_manager.GetComponent<itemManager>();
        level_warmbug_lair = obj_warmbug_lair.GetComponent<warmbugLair>();

        CMap currentMap = ODMObject.event_manager.GetComponent<MapDatabase>().getMap(Application.loadedLevelName);
        map_display_text = currentMap.name + " " + currentMap.title;
        stage_flag_name = "Area " + Application.loadedLevelName;
    }

    void Start()
    {
        InitialzeScript();

        event_center.renewLocation(map_display_text);

        if (!event_center.getFlagBool(stage_flag_name))//When player first time entering the level.
        {
            //Set area flag true
            event_center.setFlagTrue(stage_flag_name);

            //Register local level warmbugs & items
            level_warmbug_lair.registerLevelLair();
            this.registerAllLevelItems();
        }

        //check if there is any event running
        if (!checkEventOccupation()) //Release Warmbugs
        {
            level_warmbug_lair.releaseWarmbugs();
        }
        setItemReady();

        fsmHelper.getFsm(ODMObject.event_manager, "Fade").SendEvent("fade in");
        //fsmHelper.getFsm(transform.gameObject, "FSM").SendEvent("broadcast ready");//do some modification on this shit
    }

    private bool checkEventOccupation()
    {
        for (int i = 0; i < event_entity_collection.Count; i++)
        {
            //initializing event
            bool on_event = event_entity_collection[i].GetComponent<eventFlagChecker>().initialization();
            if (on_event)
                return true;
        }
        return false;
    }

    private void registerAllLevelItems()
    {
        for (int i = 0; i < item_entity_collection.Count; i++)
        {
            item_manager.registerItem(item_entity_collection[i]);
        }
    }

    private void setItemReady()
    {
        for (int i = 0; i < item_entity_collection.Count; i++)
        {
            item_entity_collection[i].GetComponent<itemSetting>().initilaization();
        }
    }
}
