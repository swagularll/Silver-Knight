using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;


public class savePoint : MonoBehaviour
{
    public GameObject loadSound;
    void Start()
    {

    }
    public void createSaveRecored(bool isAuto)
    {
        ODMVariable.create_new_save = false;

        ODMVariable.system.save_code = isAuto ? ODMVariable.common.auto_save_file_name : Guid.NewGuid().ToString();

        ODM.ODMDictionary dict_save_data = getSaveData(ODMVariable.system.save_code);
        ODM.ODMDictionary dict_flag_collection = GetComponent<eventCenter>().flag_collection;
        diaryLog diary_log = ODMObject.event_manager.GetComponent<diarySystem>().getDiary();

        ODM.ODMDictionary lair_info_collection = ODMObject.event_manager.GetComponent<warmbugLairManager>().getWarmbugDistribution();
        List<itemSetting.sceneItemInfo> item_collection = ODMObject.event_manager.GetComponent<itemManager>().getItemDistribution();

        //Generates saveRecord by collecting data from different scripts
        saveRecord instance_save_record = new saveRecord(ODMVariable.system.save_code, dict_save_data, dict_flag_collection, diary_log, lair_info_collection, item_collection);
        instance_save_record.saveProgress(instance_save_record);

        //display message
        if (isAuto)
            ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.auto_save);
        else
            ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.save_successed);
    }

    private ODM.ODMDictionary getSaveData(string save_id)
    {
        ODM.ODMDictionary dict_save_data = new ODM.ODMDictionary();

        string map_title = GetComponent<MapDatabase>().getMap(Application.loadedLevelName).title;
        string save_created_time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

        string saved_scene = Application.loadedLevelName;
        string game_difficulty = ODMVariable.game_difficulty.ToString();
        string move_speed = ODMVariable.ava_move_speed.ToString();

        string status_armor = ODMVariable.status_armor.ToString();
        string status_cum = ODMVariable.status_cum.ToString();
        string status_protected = ODMVariable.status_protected.ToString();
        string status_serum = ODMVariable.status_serum.ToString();

        string ava_current_health = ODMVariable.ava_current_health.ToString();
        string ava_current_sp = ODMVariable.ava_current_sp.ToString();
        string ava_current_poison = ODMObject.character_ava.transform.position.x.ToString();

        string ava_current_position = ODMObject.character_ava.transform.position.x.ToString();
        string current_bullets = ODMVariable.current_bullet.ToString();
        string inventory_collection_string = ODMObject.event_manager.GetComponent<inventoryDash>().getItemArray();

        dict_save_data.add(ODMVariable.save.save_id, save_id);
        dict_save_data.add(ODMVariable.save.save_created_time, save_created_time);

        dict_save_data.add(ODMVariable.save.saved_scene, saved_scene);
        dict_save_data.add(ODMVariable.save.game_difficulty, game_difficulty);
        dict_save_data.add(ODMVariable.save.move_speed, move_speed);

        dict_save_data.add(ODMVariable.save.status_armor, status_armor);
        dict_save_data.add(ODMVariable.save.status_cum, status_cum);
        dict_save_data.add(ODMVariable.save.status_protected, status_protected);
        dict_save_data.add(ODMVariable.save.status_serum, status_serum);

        dict_save_data.add(ODMVariable.save.ava_current_health, ava_current_health);
        dict_save_data.add(ODMVariable.save.ava_current_sp, ava_current_sp);
        dict_save_data.add(ODMVariable.save.ava_current_poison, ava_current_poison);

        dict_save_data.add(ODMVariable.save.ava_current_position, ava_current_poison);
        dict_save_data.add(ODMVariable.save.current_bullets, current_bullets);
        dict_save_data.add(ODMVariable.save.inventory_collection, inventory_collection_string);//Will also generate bullets in bag
        return dict_save_data;
    }

    public void deleteSave(string saveID)
    {

    }

    public void setSupplyFull()
    {
        GameObject _ava = ODMObject.character_ava;

        ODMVariable.fsm.player_control.enabled = true;
        ODMVariable.fsm.player_control_disabled.enabled = false;
        ODMVariable.fsm.player_control_disabled.FsmVariables.GetFsmBool(ODMVariable.local.is_hurt_start).Value = true;



        ODMVariable.status_armor = true;
        ODMVariable.status_serum = true;
        ODMVariable.status_cum = false;
        ODMVariable.status_protected = false;

        ODMVariable.ava_move_speed = ODMVariable.world.move_speed_max;

        ODMVariable.ava_current_health = 100f;
        ODMVariable.ava_current_sp = 100f;
        ODMVariable.ava_current_poison = 0f;//this is Posion, not Position

        ODMObject.event_manager.GetComponent<inventoryDash>().addSupplyFull();

        createSaveRecored(true);
        GameObject sound_loaded = Instantiate(loadSound);
    }
}
