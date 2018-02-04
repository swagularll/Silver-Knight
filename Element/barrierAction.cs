using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrierAction : MonoBehaviour {

    private PlayMakerFSM fsm;
    private PlayMakerFSM health_fsm;
    private Animator ani;
    private sceneObjectManager scene_object_manager;
    private sceneObjectInfo info;
    private string identifier;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        info = GetComponent<sceneObjectInfo>();
    }
    void Start () {
        scene_object_manager = ODMObject.event_manager.GetComponent<sceneObjectManager>();
        fsm = fsmHelper.getFsm(transform.gameObject, ODMVariable.common.default_fsm);
        health_fsm = fsmHelper.getFsm(transform.gameObject, ODMVariable.common.health_fsm);
        identifier = info.getIdentifier();

        int status = scene_object_manager.getSceneObjectInfo(identifier);
        switch (status)
        {
            case 0:
                fsm.enabled = true;//FSM will get disabled from FSM side.
                break;
            case 1://Open
                ani.SetBool(ODMVariable.animation.is_crack_final, true);
                break;
        }
    }

    public void crackBarrier()
    {
        ani.SetBool(ODMVariable.animation.is_crack, true);
        scene_object_manager.setSceneObjectInfo(identifier, 1);
    }
}
