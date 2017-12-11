using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class decisionPanel : MonoBehaviour
{
    //public GameObject decisionController;

    //For scene drag
    public GameObject obj_selection_true_event;
    public GameObject obj_selection_false_event;

    public string msg_context_key;
    public string left_button_key;
    public string right_button_key;
    private string after_selection_event = "afterDecision";

    private PlayMakerFSM fsm;
    //private void Awake()
    //{
    //    //fsm = fsmHelper.getFsm(transform.gameObject, ODMVariable.common.default_fsm);
    //}

    //decision content
    private void makeDecision()
    {
        //renew code - bool setting and scene controller
        ODMVariable.fsm.scene_controller.SendEvent(eventName.start_hold);
        ODMVariable.is_on_event = true;

        string msg_context = dataWidget.getTranslaton(msg_context_key);
        ODMObject.confirmation_panel.GetComponent<confirmPanel>().
            showConfirmation(transform.gameObject, after_selection_event, left_button_key, right_button_key, msg_context);
    }
    public void afterDecision(bool _result)
    {
        if (_result)
            fsmHelper.getFsm(obj_selection_true_event, eventName.proceed_event);
        else
            fsmHelper.getFsm(obj_selection_false_event, eventName.proceed_event);
    }
}
