﻿using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using Assets.Script.ODM_Widget;

public class decision : MonoBehaviour {

    public GameObject decisionController;
    public string fsmName = "FSM";

    private void makeDecision()
    {
        fsmHelper.getFsm(ODMObject.event_manager,ODMVariable.fsm_scene_controller).SendEvent("start hold");
        FsmVariables.GlobalVariables.GetFsmBool("isOnEvent").Value = true;
        string msg = dataWidget.getTranslaton("confirm save zarker");

        FsmVariables.GlobalVariables.GetFsmGameObject("obj_ConfirmPanel").Value.GetComponent<confirmPanel>().
            showConfirmation(transform.gameObject, "afterDecision", "stay away", "save zarker" , msg);
    }
    public void afterDecision(bool _result)
    {
        if (_result)
        {
            fsmHelper.getFsm(decisionController, fsmName).SendEvent("selection true");
        }
        else
        {
            fsmHelper.getFsm(decisionController, fsmName).SendEvent("selection false");
        }
    }
}
