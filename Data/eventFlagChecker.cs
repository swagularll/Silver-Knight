﻿using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using System.Collections.Generic;


public class eventFlagChecker : MonoBehaviour
{
    public string flag_name = "";
    public string fsm_name = "FSM";//linked FSM

    //to_activate
    public List<string> true_flag_collection;
    public List<string> false_flag_collection;

    //to_destory
    public List<string> true_destory_flag_colleciton;
    public List<string> false_destory_flag_collection;

    //default setting
    public bool indipendent_event = false;
    public bool precondition_occupy_scene = false;
    public bool event_retain_occupy_scene = false;


    private eventCenter event_center;
    private PlayMakerFSM fsm;

    private void Start()
    {
        if (indipendent_event)
        {
            initialization();
        }
    }

    public bool initialization()//returning the event is going to occupy the scene or not
    {
        setFSM();

        bool precondition_done = true;
        for (int i = 0; i < true_flag_collection.Count; i++)
        {
            if (!event_center.getFlagBool(true_flag_collection[i].Trim()))
                precondition_done = false;
        }

        for (int i = 0; i < false_flag_collection.Count; i++)
        {
            if (event_center.getFlagBool(false_flag_collection[i].Trim()))
                precondition_done = false;
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

        if (precondition_done)
        {
            bool result = event_center.getFlagBool(flag_name);
            if (result)
            {
                //Event has already happened
                if (to_destory)
                {
                    //Happened and event self destroy
                    fsm.SendEvent(eventName.true_result);
                    return false;
                }
                else
                {
                    //Event has happened but still waiting for next coming event
                    fsm.SendEvent(eventName.event_retain);
                    return event_retain_occupy_scene;
                }
            }
            else
            {
                //Activating event
                fsm.SendEvent(eventName.false_result);
                return true;
            }
        }
        else
        {
            fsm.SendEvent(eventName.precondition_not_done);
            return precondition_occupy_scene;
        }
    }


    public void setFSM()
    {
        flag_name = flag_name.Trim();
        fsm_name = fsm_name.Trim();
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();

        if (!String.IsNullOrEmpty(fsm_name))
        {
            fsm = fsmHelper.getFsm(transform.gameObject, fsm_name);
            fsm.FsmVariables.GetFsmString(ODMVariable.local.flag_name).Value = flag_name;
            fsm.FsmVariables.GetFsmString(ODMVariable.local.fsm_name).Value = fsm_name;
            fsm.FsmVariables.GetFsmGameObject(ODMVariable.local.self).Value = transform.gameObject;
        }
    }



    ////renew code
    //public void flagCheck()
    //{
    //    ODM.errorLog("Calling wrong evnet!", "");
    //}

    #region For Call
    public void registerEvent(GameObject _level_loader)
    {
        _level_loader.GetComponent<afterLoad>().addToEventCollection(transform.gameObject);
    }
    #endregion

}
