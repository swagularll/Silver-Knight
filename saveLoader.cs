using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using System.IO;
using System.Text;
using System.Collections.Generic;
using LitJson;

public class saveLoader : MonoBehaviour
{
    private TextAsset flag_collection_asset;
    public bool development_load = false;
    public List<int[]> inventory_collection = new List<int[]>();

    void Start()
    {
        try
        {
            if (!FsmVariables.GlobalVariables.GetFsmBool("create_new_save").Value)
            {
                //Load game from save record
                ODM.saveRecord save_record = ODM.getCurrentRecord();

                PlayerPrefs.SetString("level_to_load", save_record.save_data.getValue("saved_scene"));
                PlayerPrefs.SetString("game_difficulty", save_record.save_data.getValue("game_difficulty"));
                FsmVariables.GlobalVariables.GetFsmFloat("move_speed").Value = float.Parse(save_record.save_data.getValue("move_speed"));

                FsmVariables.GlobalVariables.GetFsmBool("status_armor").Value = Convert.ToBoolean(save_record.save_data.getValue("status_armor"));
                FsmVariables.GlobalVariables.GetFsmBool("status_cum").Value = Convert.ToBoolean(save_record.save_data.getValue("status_cum"));
                FsmVariables.GlobalVariables.GetFsmBool("status_protected").Value = Convert.ToBoolean(save_record.save_data.getValue("status_protected"));
                FsmVariables.GlobalVariables.GetFsmBool("status_serum").Value = Convert.ToBoolean(save_record.save_data.getValue("status_serum"));

                FsmVariables.GlobalVariables.GetFsmFloat("ava_current_health").Value = float.Parse(save_record.save_data.getValue("ava_current_health"));
                FsmVariables.GlobalVariables.GetFsmFloat("ava_current_sp").Value = float.Parse(save_record.save_data.getValue("ava_current_sp"));
                FsmVariables.GlobalVariables.GetFsmFloat("ava_current_poison").Value = float.Parse(save_record.save_data.getValue("ava_current_poison"));

                float ava_current_position = float.Parse(save_record.save_data.getValue("ava_current_position"));
                GameObject Ava = FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value;
                if (!development_load)
                {
                    Vector3 ava_location = new Vector3(ava_current_position, Ava.transform.position.y, Ava.transform.position.z);
                    Ava.transform.position = ava_location;
                }
                FsmVariables.GlobalVariables.GetFsmInt("current_bullets").Value = Int32.Parse(save_record.save_data.getValue("current_bullets"));
                inventory_collection = JsonMapper.ToObject<List<int[]>>(save_record.save_data.getValue("inventory_collection"));//inventory Dash will access this list
            }

            //set this to true when the armor recovered
            fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled").
                FsmVariables.GetFsmBool("is_hurt_start").Value = false;

            if (FsmVariables.GlobalVariables.GetFsmBool("status_armor").Value)
            {
                fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control").enabled = true;
                fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled").enabled = false;
            }
            else
            {
                fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control").enabled = false;
                fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled").enabled = true;
            }
            if (!development_load)
                Application.LoadLevel(PlayerPrefs.GetString("level_to_load"));
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name, "Error occurred when initilizing the save record.", ex.ToString());
        }
    }


}
