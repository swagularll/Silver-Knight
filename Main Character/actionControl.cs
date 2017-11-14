using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using Assets.Script.ODM_Widget;

public class actionControl : MonoBehaviour
{

    private fsmHelper fsmHelper;
    private bool ava_status;

    void Start()
    {
        fsmHelper = new fsmHelper();
    }

    public void setIdle()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("jump_Idle");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled");
            fControlDisabled.SendEvent("jump_Idle");
        }
    }

    public void avaPickUpItem()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava pick item");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled");
            fControlDisabled.SendEvent("ava pick item");
        }
    }

    public void avaDownCheck()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava down check");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled");
            fControlDisabled.SendEvent("ava down check");
        }
    }

    public void avaGetHurt()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.FsmVariables.GetFsmFloat("ava_hurt_force").Value = 0;
            fControl.SendEvent("ava event hurt");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled");
            fControlDisabled.FsmVariables.GetFsmFloat("ava_hurt_force").Value = 0;
            fControl.SendEvent("ava event hurt");
        }
    }

    public void avaGetHit(float force)
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            FsmVariables.GlobalVariables.GetFsmFloat("ava_hurt_force").Value = (-1) * force * FsmVariables.GlobalVariables.GetFsmFloat("direction_Ava").Value;
            fControl.SendEvent("ava get hit");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled");
            FsmVariables.GlobalVariables.GetFsmFloat("ava_hurt_force").Value = (-1) * force * FsmVariables.GlobalVariables.GetFsmFloat("direction_Ava").Value;
            fControl.SendEvent("ava get hit");
        }
    }

    public void avaFainting()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava event faint");
        }
    }

    public void avaAwake()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava event awake");
        }
    }

    public void avaGetKilled()
    {
        FsmVariables.GlobalVariables.GetFsmGameObject("obj_Matebar_Canvas").Value.GetComponent<CanvasGroup>().alpha = 0f;
        GameObject currentMate = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value;
        if (currentMate != null)
        {
            string mateName = currentMate.GetComponent<warmbugAction>().warmbug_name;
            switch (mateName)
            {
                case "Scorpion":
                    fsmHelper.getFsm(ODMObject.character_ava, "Event").SendEvent("killed by Scorpion");
                    break;
                case "MECB":
                    fsmHelper.getFsm(ODMObject.character_ava, "Event").SendEvent("killed by MECB");
                    break;
                case "Larvae":
                    fsmHelper.getFsm(ODMObject.character_ava, "Event").SendEvent("killed by Larvae");
                    break;
                case "Inuji":
                    fsmHelper.getFsm(ODMObject.character_ava, "Event").SendEvent("killed by Inuji");
                    break;
                case "Ambusher":
                    fsmHelper.getFsm(ODMObject.character_ava, "Event").SendEvent("killed by Ambusher");
                    break;
                case "Silencer":
                    fsmHelper.getFsm(ODMObject.character_ava, "Event").SendEvent("killed by Ambusher");
                    break;
                default:
                    ODM.errorLog(transform.name, "avaGetKilled missing mate target.", "");
                    break;
            }
        }
        else
        {
            ODM.log(transform.name, "Ava get killed. No current mate.");
            PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Event");
            if (fControl.isActiveAndEnabled)
            {
                fControl.SendEvent("ava event dead");
            }
            else
            {
                PlayMakerFSM fControlDisabled = fsmHelper.getFsm(ODMObject.character_ava, "Event");
                fControlDisabled.SendEvent("ava event dead");
            }
        }
    }


    public void setPlayerWait()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava wait");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled");
            fControlDisabled.SendEvent("ava wait");
        }
    }

    public void avaEventAim()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava event aim");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled");
            fControlDisabled.SendEvent("ava event aim");
        }
    }

    public void avaEventWalk()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava event walk");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled");
            fControlDisabled.SendEvent("ava event walk");
        }
    }
    public void setPlayerContinue()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(ODMObject.character_ava, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("jump_Idle");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(ODMObject.character_ava, "Player Control Disabled");
            fControlDisabled.SendEvent("jump_Idle");
        }
    }
    public void avaLocation(GameObject _location_obj)
    {
        var avaTransform = ODMObject.character_ava.transform.GetComponent<Transform>();
        avaTransform.position = new Vector3(_location_obj.transform.position.x, 0, 0);
    }

    public void enableControl()
    {
        PlayMakerFSM fsm =
            FsmVariables.GlobalVariables.GetFsmBool("status_armor").Value ?
            fsmHelper.getFsm(transform.gameObject, "Player Control") :
            fsmHelper.getFsm(transform.gameObject, "Player Control Disabled");
        fsm.SendEvent("resume FSM");
    }
    public void disableControl()
    {
        PlayMakerFSM fsm =
            FsmVariables.GlobalVariables.GetFsmBool("status_armor").Value ?
            fsmHelper.getFsm(transform.gameObject, "Player Control") :
            fsmHelper.getFsm(transform.gameObject, "Player Control Disabled");
        fsm.FsmVariables.GetFsmString("previous_state").Value = fsm.ActiveStateName;
        fsm.SendEvent("hold FSM");
    }
}
