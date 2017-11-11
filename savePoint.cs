using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using Assets.Script.ODM_Widget;

public class savePoint : MonoBehaviour
{
    public GameObject loadSound;
    void Start()
    {

    }
    public void createSaveRecored(bool isAuto)
    {
        string save_id = isAuto ? Guid.NewGuid().ToString() : "Auto";

        ODM.ODMDictionary dict_save_data = getSaveData(save_id);
        ODM.ODMDictionary dict_flag_collection = GetComponent<eventCenter>().flag_collection;
        diaryLog diary_log = ODMObject.event_manager.GetComponent<diarySystem>().getDiary();

        ODM.ODMDictionary lair_info_collection = ODMObject.event_manager.GetComponent<warmbugLairManager>().getWarmbugDistribution();
        List<itemManager.itemInfo> item_collection = ODMObject.event_manager.GetComponent<itemManager>().getItemDistribution();

        //Generates saveRecord by collecting data from different scripts
        saveRecord instance_save_record = new saveRecord(save_id, dict_save_data, dict_flag_collection, diary_log, lair_info_collection, item_collection);
        instance_save_record.saveProgress(instance_save_record);

        //display message
        if (isAuto)
            FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value.
                GetComponent<messagePanel>().showMessage("auto save");
        else
            FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value.
                GetComponent<messagePanel>().showMessage("save successed");
    }

    private ODM.ODMDictionary getSaveData(string save_id)
    {
        ODM.ODMDictionary dict_save_data = new ODM.ODMDictionary();

        string map_title = GetComponent<MapDatabase>().getMap(Application.loadedLevelName).title;
        string save_created_time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

        string saved_scene = Application.loadedLevelName;
        string game_difficulty = PlayerPrefs.GetInt("game_difficulty").ToString();
        string move_speed = FsmVariables.GlobalVariables.GetFsmFloat("move_speed").Value.ToString();

        string status_armor = FsmVariables.GlobalVariables.GetFsmBool("status_armor").Value.ToString();
        string status_cum = FsmVariables.GlobalVariables.GetFsmBool("status_cum").Value.ToString();
        string status_protected = FsmVariables.GlobalVariables.GetFsmBool("status_protected").Value.ToString();
        string status_serum = FsmVariables.GlobalVariables.GetFsmBool("status_serum").Value.ToString();

        string ava_current_health = FsmVariables.GlobalVariables.GetFsmFloat("ava_current_health").Value.ToString();
        string ava_current_sp = FsmVariables.GlobalVariables.GetFsmFloat("ava_current_sp").Value.ToString();
        string ava_current_poison = ODMObject.character_ava.transform.position.x.ToString();
        //FsmVariables.GlobalVariables.GetFsmFloat("ava_current_poison").Value.ToString();

        string ava_current_position = ODMObject.character_ava.transform.position.x.ToString();
        string current_bullets = FsmVariables.GlobalVariables.GetFsmInt("current_bullets").Value.ToString();
        string inventory_collection_string = ODMObject.event_manager.GetComponent<inventoryDash>().getItemArray();

        dict_save_data.add("save_id", save_id);
        dict_save_data.add("save_created_time", save_created_time);

        dict_save_data.add("saved_scene", saved_scene);
        dict_save_data.add("game_difficulty", game_difficulty);
        dict_save_data.add("move_speed", move_speed);

        dict_save_data.add("status_armor", status_armor);
        dict_save_data.add("status_cum", status_cum);
        dict_save_data.add("status_protected", status_protected);
        dict_save_data.add("status_serum", status_serum);

        dict_save_data.add("ava_current_health", ava_current_health);
        dict_save_data.add("ava_current_sp", ava_current_sp);
        dict_save_data.add("ava_current_poison", ava_current_poison);

        dict_save_data.add("ava_current_position", ava_current_poison);
        dict_save_data.add("current_bullets", current_bullets);
        dict_save_data.add("inventory_collection", inventory_collection_string);//Will also generate bullets in bag
        return dict_save_data;
    }

    public void deleteSave(string saveID)
    {

    }

    public void setSupplyFull()
    {
        GameObject _ava = ODMObject.character_ava;

        fsmHelper.getFsm(_ava, "Player Control").enabled = true;
        fsmHelper.getFsm(_ava, "Player Control Disabled").enabled = false;
        fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled").
                FsmVariables.GetFsmBool("is_hurt_start").Value = true;

        FsmVariables.GlobalVariables.GetFsmBool("status_armor").Value = true;
        FsmVariables.GlobalVariables.GetFsmBool("status_serum").Value = true;
        FsmVariables.GlobalVariables.GetFsmBool("status_cum").Value = false;
        FsmVariables.GlobalVariables.GetFsmBool("status_protected").Value = false;

        FsmVariables.GlobalVariables.GetFsmFloat("moveSpeed").Value = FsmVariables.GlobalVariables.GetFsmFloat("moveSpeed_max").Value;

        FsmVariables.GlobalVariables.GetFsmFloat("ava_current_health").Value = 100f;
        FsmVariables.GlobalVariables.GetFsmFloat("ava_current_sp").Value = 100f;
        FsmVariables.GlobalVariables.GetFsmFloat("ava_current_poison").Value = 0f;//this is POISON, not POSITION

        ODMObject.event_manager.GetComponent<inventoryDash>().addSupplyFull();

        createSaveRecored(true);
        Debug.Log("Save record created");
        GameObject sound_loaded = Instantiate(loadSound);

    }
}
