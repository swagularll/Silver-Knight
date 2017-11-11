using UnityEngine;
using System.Collections;
using System;
using Assets.Script.ODM_Widget;

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
    private PlayMakerFSM healthFSM;
    private PlayMakerFSM damageFSM;
    private PlayMakerFSM warmbugAI_FSM;

    void Start()
    {
        if (soloBug)
        {
            lair_info = new lairInfo();
            getAbilitiesHealth();
            getAbilitiesDamage();
        }

    }

    public void initilization(warmbugLair _lair, lairInfo _lair_info, bool isLiving)
    {
        ref_lair = _lair;
        lair_info = _lair_info;
        warmbug_guid = _lair_info.warmbug_guid;
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
            healthFSM = fsmHelper.getFsm(transform.gameObject, "Health System");

            int difficuly = PlayerPrefs.GetInt("game_difficulty");
            float difficulyMagnification = 0f;

            switch (difficuly)
            {
                case 1:
                    difficulyMagnification = (float)lair_info.easyMagnification;
                    break;
                case 2:
                    difficulyMagnification = (float)lair_info.normalMagnification;
                    break;
                case 3:
                    difficulyMagnification = (float)lair_info.hardMagnification;
                    break;
                case 4:
                    difficulyMagnification = (float)lair_info.hellMagnification;
                    break;
            }
            warmbug_hp = healthFSM.FsmVariables.GetFsmFloat("warmbug_hp").Value = (float)(warmbug_hp * difficulyMagnification);
            sp_bonus = healthFSM.FsmVariables.GetFsmFloat("sp_bonus").Value = (float)(sp_bonus * difficulyMagnification);

            healthFSM.SendEvent("WB Ready");
        }
        else
        {
            if (!soloBug)
                ODM.errorLog(transform.name,
                    "getAbilitiesHealth Missing LairInfo", "");
        }
    }
    public void getAbilitiesDamage()
    {
        if (lair_info != null || soloBug)
        {
            damageFSM = fsmHelper.getFsm(transform.gameObject, "Warmbug Damage");
            warmbugAI_FSM = fsmHelper.getFsm(transform.gameObject, "Warmbug AI");

            int difficuly = PlayerPrefs.GetInt("game_difficulty");
            float difficulyMagnification = 0f;
            switch (difficuly)
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
            }
            armor_creak = damageFSM.FsmVariables.GetFsmFloat("armor_creak").Value = (float)(armor_creak * difficulyMagnification);
            normal_damage = damageFSM.FsmVariables.GetFsmFloat("normal_damage").Value = (float)(normal_damage * difficulyMagnification);
            normal_sp_damage = damageFSM.FsmVariables.GetFsmFloat("normal_sp_damage").Value = (float)(normal_sp_damage * difficulyMagnification);
            melee_damage = damageFSM.FsmVariables.GetFsmFloat("melee_damage").Value = (float)(melee_damage * difficulyMagnification);
            mate_critial_hit = damageFSM.FsmVariables.GetFsmFloat("mate_critial_hit").Value = (float)(mate_critial_hit * difficulyMagnification);
            feed_critial_hit = damageFSM.FsmVariables.GetFsmFloat("feed_critial_hit").Value = (float)(feed_critial_hit * difficulyMagnification);
            damageFSM.SendEvent("WB Ready");

            //Constant Value
            warmbugAI_FSM.FsmVariables.GetFsmFloat("base_warmbug_mate_force").Value = base_warmbug_mate_force;
            warmbugAI_FSM.FsmVariables.GetFsmFloat("base_warmbug_feed_force").Value = base_warmbug_feed_force;
            warmbugAI_FSM.SendEvent("WB Ready");
        }
        else
        {
            if (!soloBug)
                ODM.errorLog(transform.name,"getAbilitiesDamage Missing LairInfo", "");
        }
    }
}
