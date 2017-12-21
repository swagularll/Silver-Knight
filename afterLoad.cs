using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

using System.Collections.Generic;
using System;

public class afterLoad : MonoBehaviour
{
    public List<GameObject> event_entity_collection;
    public List<GameObject> item_entity_collection;
    public List<GameObject> scene_object_collection;

    //For scene drag
    public GameObject obj_warmbug_lair;


    private warmbugLair level_warmbug_lair;
    private eventCenter event_center;
    private itemManager item_manager;
    private sceneObjectManager scene_info_manager;

    private string map_display_text;
    private string stage_flag_name;

    private string fsm_fade = "Fade";

    private void InitialzeScript()
    {
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();
        item_manager = ODMObject.event_manager.GetComponent<itemManager>();
        level_warmbug_lair = obj_warmbug_lair.GetComponent<warmbugLair>();
        scene_info_manager = ODMObject.event_manager.GetComponent<sceneObjectManager>();

        CMap currentMap = ODMObject.event_manager.GetComponent<MapDatabase>().getMap(Application.loadedLevelName);
        map_display_text = currentMap.name + " " + currentMap.title;
        stage_flag_name = ODMVariable.convert.getAreaFlag(Application.loadedLevelName);
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
            this.registerAllSceneObjects();
            this.registerAllLevelItems();
        }
        else
        {
            this.removeAllLevelItems();
            item_manager.deployLevelItems(Application.loadedLevelName);
        }

        //check if there is any event running
        if (!checkEventOccupation()) //Release Warmbugs
        {
            level_warmbug_lair.releaseWarmbugs();
        }

        fsmHelper.getFsm(ODMObject.event_manager, fsm_fade).SendEvent(eventName.fade_in);
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
        //Register item with new GUID
        for (int i = 0; i < item_entity_collection.Count; i++)
        {
            item_manager.registerItem(item_entity_collection[i]);
            item_entity_collection[i].GetComponent<itemSetting>().initilaization();
        }
    }
    private void registerAllSceneObjects()
    {
        for (int i = 0; i < scene_object_collection.Count; i++)
        {
            scene_info_manager.registerSceneInfo(scene_object_collection[i].GetComponent<sceneObjectInfo>().getIdentifier());
        }
    }
    private void removeAllLevelItems()
    {
        for (int i = 0; i < item_entity_collection.Count; i++)
        {
            GameObject.Destroy(item_entity_collection[i]);
        }
    }
}
