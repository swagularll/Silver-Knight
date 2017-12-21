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
    public bool development_load = false;
    public List<int[]> inventory_collection = new List<int[]>();

    void Start()
    {
        try
        {
            if (!ODMVariable.create_new_save)
            {
                //Load game from save record
                saveRecord save_record = saveRecord.getCurrentRecord();

                ODMVariable.level_to_load = save_record.save_data.getValue(ODMVariable.save.saved_scene);
                ODMVariable.game_difficulty = Int32.Parse(save_record.save_data.getValue(ODMVariable.save.game_difficulty));
                ODMVariable.ava_move_speed = float.Parse(save_record.save_data.getValue(ODMVariable.save.move_speed));

                ODMVariable.status_armor = Convert.ToBoolean(save_record.save_data.getValue(ODMVariable.save.status_armor));
                ODMVariable.status_cum = Convert.ToBoolean(save_record.save_data.getValue(ODMVariable.save.status_cum));
                ODMVariable.status_protected = Convert.ToBoolean(save_record.save_data.getValue(ODMVariable.save.status_protected));
                ODMVariable.status_serum = Convert.ToBoolean(save_record.save_data.getValue(ODMVariable.save.status_serum));

                ODMVariable.ava_current_health = float.Parse(save_record.save_data.getValue(ODMVariable.save.ava_current_health));
                ODMVariable.ava_current_sp = float.Parse(save_record.save_data.getValue(ODMVariable.save.ava_current_sp));
                ODMVariable.ava_current_poison = float.Parse(save_record.save_data.getValue(ODMVariable.save.ava_current_poison));

                float ava_current_position = float.Parse(save_record.save_data.getValue(ODMVariable.save.ava_current_position));
                if (!development_load)
                {
                    Vector3 ava_location = new Vector3(ava_current_position, ODMObject.character_ava.transform.position.y, ODMObject.character_ava.transform.position.z);
                    ODMObject.character_ava.transform.position = ava_location;
                }
                ODMVariable.current_bullet = Int32.Parse(save_record.save_data.getValue(ODMVariable.save.current_bullets));
                inventory_collection = JsonMapper.ToObject<List<int[]>>(save_record.save_data.getValue(ODMVariable.save.inventory_collection));//inventory Dash will access this list
            }

            //set this to true when the armor recovered
            ODMVariable.fsm.player_control_disabled.
                FsmVariables.GetFsmBool(ODMVariable.local.is_hurt_start).Value = false;

            ODMVariable.ava_direction = 1f;

            if (ODMVariable.status_armor)
            {
                ODMVariable.fsm.player_control.enabled = true;
                ODMVariable.fsm.player_control_disabled.enabled = false;
            }
            else
            {
                ODMVariable.fsm.player_control.enabled = false;
                ODMVariable.fsm.player_control_disabled.enabled = true;
            }
            if (!development_load)
            {
                Application.LoadLevel(ODMVariable.level_to_load);
            }
            ODMObject.event_manager.GetComponent<diarySystem>().startCounting();
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name, ex.ToString());
        }
    }
}
