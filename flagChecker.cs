using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagChecker : MonoBehaviour {
    public string flag_name;
    private PlayMakerFSM fsm;
    private eventCenter event_center;
    private void Awake()
    {
        fsm = fsmHelper.getFsm(transform.gameObject, ODMVariable.common.default_fsm);
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();
    }

    private void Start()
    {
        if (event_center.getFlagBool(flag_name))
        {
            fsm.SendEvent(eventName.true_result);
        }
        else
        {
            fsm.SendEvent(eventName.false_result);
        }
    }

    public void setFlagTrue()
    {
        event_center.setFlagTrue(flag_name);
    }
}
