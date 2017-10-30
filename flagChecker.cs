using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using System.Collections.Generic;

public class flagChecker : MonoBehaviour
{
    public string flag_name = "";
    public string fsm_name = "";//linked FSM

    //to_activate
    public List<string> true_flag_collection;
    public List<string> false_flag_collection;

    //to_destory
    public List<string> true_destory_flag_colleciton;
    public List<string> false_destory_flag_collection;

    private eventCenter event_center;
    private PlayMakerFSM fsm;

    public void flagCheck()
    {
        setFSM();

        bool to_activate = true;
        for (int i = 0; i < true_flag_collection.Count; i++)
        {
            if (!event_center.getFlagBool(true_flag_collection[i].Trim()))
                to_activate = false;
        }

        for (int i = 0; i < false_flag_collection.Count; i++)
        {
            if (event_center.getFlagBool(false_flag_collection[i].Trim()))
                to_activate = false;
        }

        bool to_destory = true;
        for (int i = 0; i < true_destory_flag_colleciton.Count; i++)
        {
            if (!event_center.getFlagBool(true_destory_flag_colleciton[i].Trim()))
                to_destory = false;
        }

        for (int i = 0; i < false_destory_flag_collection.Count; i++)
        {
            if (event_center.getFlagBool(false_destory_flag_collection[i].Trim()))
                to_destory = false;
        }

        if (to_activate)
        {
            bool result = event_center.getFlagBool(flag_name);
            if (result)
            {
                if (to_destory)
                    fsm.SendEvent("true result");
                else
                    fsm.SendEvent("event retain");
            }
            else
                fsm.SendEvent("false result");
        }
        else
            fsm.SendEvent("precondition not done");

    }
    public void setFSM()
    {
        flag_name = flag_name.Trim();
        fsm_name = fsm_name.Trim();
        event_center = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value.GetComponent<eventCenter>();

        if (!String.IsNullOrEmpty(fsm_name))
        {
            fsm = fsmHelper.getFsm(transform.gameObject, fsm_name);
            fsm.FsmVariables.GetFsmString("flagName").Value = flag_name;
            fsm.FsmVariables.GetFsmString("fsmName").Value = fsm_name;
            fsm.FsmVariables.GetFsmGameObject("self").Value = transform.gameObject;
        }
    }
}
