using UnityEngine;
using System.Collections;
using System;

public class warmbugAction : MonoBehaviour
{
    public string warmbug_name;
    public string warmbug_guid = "Default GUID";
    public float warmbug_hp = 0;
    public float sp_bonus = 0;
    public float armor_creak = 0;
    public float normal_damage = 0;
    public float normal_sp_damage = 0;
    public float melee_damage = 0;
    public float mate_critial_hit = 0;
    public float feed_critial_hit = 0;

    public float base_warmbug_mate_force = 0;
    public float base_warmbug_feed_force = 0;

    public bool soloBug = false;

    private warmbugLair ref_lair;
    private lairInfo lair_info;
    private PlayMakerFSM health_fsm;
    private PlayMakerFSM damage_fsm;
    private PlayMakerFSM warmbug_ai_fsm;
    private string health_fsm_name = ODMVariable.common.health_fsm;
    private string damage_fsm_name = "Warmbug Damage";
    private string warmbug_ai_fsm_name = "Warmbug AI";


    void Start()
    {
        if (soloBug)
        {
            lair_info = new lairInfo();
            getAbilitiesHealth();
            getAbilitiesDamage();
        }
    }

    public void initilization(warmbugLair _lair, lairInfo _lair_info)
    {
        ref_lair = _lair;
        lair_info = _lair_info;
        warmbug_guid = _lair_info.warmbug_guid;
        getAbilitiesHealth();
        getAbilitiesDamage();
    }
    public void initilization(warmbugLair _lair)
    {
        ref_lair = _lair;
        lair_info = new lairInfo();
        warmbug_guid = lair_info.warmbug_guid;
        getAbilitiesHealth();
        getAbilitiesDamage();
    }

    public string getName()
    {
        return warmbug_name;
    }

    public void sendDeadMessage()
    {
        if (ref_lair != null)
        {
            ref_lair.setDead(warmbug_guid);
        }
    }

    public void getAbilitiesHealth()
    {
        if (lair_info != null || soloBug)
        {
            health_fsm = fsmHelper.getFsm(transform.gameObject, health_fsm_name);

            float difficuly_magnification = 0f;

            switch (ODMVariable.game_difficulty)
            {
                case 1:
                    difficuly_magnification = (float)lair_info.easyMagnification;
                    break;
                case 2:
                    difficuly_magnification = (float)lair_info.normalMagnification;
                    break;
                case 3:
                    difficuly_magnification = (float)lair_info.hardMagnification;
                    break;
                case 4:
                    difficuly_magnification = (float)lair_info.hellMagnification;
                    break;
                default:
                    ODM.errorLog(transform.name, "Missing game difficulty: getAbilitiesHealth()");
                    break;
            }
            warmbug_hp = health_fsm.FsmVariables.GetFsmFloat(ODMVariable.warmbug.warmbug_hp).Value = (float)(warmbug_hp * difficuly_magnification);
            sp_bonus = health_fsm.FsmVariables.GetFsmFloat(ODMVariable.warmbug.sp_bonus).Value = (float)(sp_bonus * difficuly_magnification);

            health_fsm.SendEvent(eventName.warmbug_ready);
        }
        else
        {
            if (!soloBug)
                ODM.errorLog(transform.name, "getAbilitiesDamage missing LairInfo.");
        }
    }
    public void getAbilitiesDamage()
    {
        if (lair_info != null || soloBug)
        {
            damage_fsm = fsmHelper.getFsm(transform.gameObject, damage_fsm_name);
            warmbug_ai_fsm = fsmHelper.getFsm(transform.gameObject, warmbug_ai_fsm_name);

            float difficulyMagnification = 0f;
            switch (ODMVariable.game_difficulty)
            {
                case 1:
                    difficulyMagnification = 0.5f;
                    break;
                case 2:
                    difficulyMagnification = 1f;
                    break;
                case 3:
                    difficulyMagnification = 1.5f;
                    break;
                case 4:
                    difficulyMagnification = 2.7f;
                    break;
                default:
                    ODM.errorLog(transform.name, "Missing game difficulty: getAbilitiesDamage()");
                    break;
            }
            armor_creak = damage_fsm.FsmVariables.GetFsmFloat(ODMVariable.warmbug.armor_creak).Value = (float)(armor_creak * difficulyMagnification);
            normal_damage = damage_fsm.FsmVariables.GetFsmFloat(ODMVariable.warmbug.normal_damage).Value = (float)(normal_damage * difficulyMagnification);
            normal_sp_damage = damage_fsm.FsmVariables.GetFsmFloat(ODMVariable.warmbug.normal_sp_damage).Value = (float)(normal_sp_damage * difficulyMagnification);
            melee_damage = damage_fsm.FsmVariables.GetFsmFloat(ODMVariable.warmbug.melee_damage).Value = (float)(melee_damage * difficulyMagnification);
            mate_critial_hit = damage_fsm.FsmVariables.GetFsmFloat(ODMVariable.warmbug.mate_critial_hit).Value = (float)(mate_critial_hit * difficulyMagnification);
            feed_critial_hit = damage_fsm.FsmVariables.GetFsmFloat(ODMVariable.warmbug.feed_critial_hit).Value = (float)(feed_critial_hit * difficulyMagnification);
            damage_fsm.SendEvent(eventName.warmbug_ready);

            //Constant Value
            warmbug_ai_fsm.FsmVariables.GetFsmFloat(ODMVariable.warmbug.base_warmbug_mate_force).Value = base_warmbug_mate_force;
            warmbug_ai_fsm.FsmVariables.GetFsmFloat(ODMVariable.warmbug.base_warmbug_feed_force).Value = base_warmbug_feed_force;
            warmbug_ai_fsm.SendEvent(eventName.warmbug_ready);
        }
        else
        {
            if (!soloBug)
                ODM.errorLog(transform.name, "getAbilitiesDamage missing LairInfo.");
        }
    }
}
