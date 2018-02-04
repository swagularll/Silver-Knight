using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

using System.Collections.Generic;
using System;

public class afterLoad : MonoBehaviour
{
    public bool development_load = false;

    private List<GameObject> event_entity_collection;
    private List<GameObject> item_entity_collection;
    private List<GameObject> scene_object_collection;

    private warmbugLair level_warmbug_lair;
    private eventCenter event_center;
    private itemManager item_manager;
    private sceneObjectManager scene_info_manager;

    private string map_display_text;
    private string stage_flag_name;

    void Start()
    {
        InitialzeScript();

        event_center.renewLocation(map_display_text);

        if (!event_center.getFlagBool(stage_flag_name) || development_load)//When player first time entering the level.
        {
            //When first enter a level, set area flag true
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
        if (!checkEventOccupation())
        {
            level_warmbug_lair.releaseWarmbugs();
        }
        ODMVariable.fsm.fade.SendEvent(eventName.fade_in);

        //fsmHelper.getFsm(transform.gameObject, "FSM").SendEvent("broadcast ready");//do some modification on this shit
    }
    private void InitialzeScript()
    {
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();
        item_manager = ODMObject.event_manager.GetComponent<itemManager>();
        level_warmbug_lair = ODMObject.current_level_lair.GetComponent<warmbugLair>();
        scene_info_manager = ODMObject.event_manager.GetComponent<sceneObjectManager>();

        CMap currentMap = ODMObject.event_manager.GetComponent<MapDatabase>().getMap(Application.loadedLevelName);
        map_display_text = currentMap.name + " " + currentMap.title;
        stage_flag_name = ODMVariable.convert.getAreaFlag(Application.loadedLevelName);

        event_entity_collection = new List<GameObject>();
        item_entity_collection = new List<GameObject>();
        scene_object_collection = new List<GameObject>();

        //Collect array
        BroadcastMessage(eventName.sys.register_event, transform.gameObject, SendMessageOptions.DontRequireReceiver);
        BroadcastMessage(eventName.sys.register_item, transform.gameObject, SendMessageOptions.DontRequireReceiver);
        BroadcastMessage(eventName.sys.register_scene_object, transform.gameObject, SendMessageOptions.DontRequireReceiver);
    }

    public void addToItemCollection(GameObject _item_entity)
    {
        item_entity_collection.Add(_item_entity);
    }
    public void addToEventCollection(GameObject _event_entity)
    {
        event_entity_collection.Add(_event_entity);
    }
    public void addToSceneObjectCollection(GameObject _scene_object)
    {
         scene_object_collection.Add(_scene_object);
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
