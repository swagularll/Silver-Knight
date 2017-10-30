using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class actionControl : MonoBehaviour
{

    private fsmHelper fsmHelper;

    void Start()
    {
        fsmHelper = new fsmHelper();
    }

    public void setIdle()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("jump_Idle");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled");
            fControlDisabled.SendEvent("jump_Idle");
        }
    }

    public void avaPickUpItem()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava pick item");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled");
            fControlDisabled.SendEvent("ava pick item");
        }
    }

    public void avaDownCheck()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava down check");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled");
            fControlDisabled.SendEvent("ava down check");
        }
    }

    public void avaGetHurt()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.FsmVariables.GetFsmFloat("ava_hurt_force").Value = 0;
            fControl.SendEvent("ava event hurt");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled");
            fControlDisabled.FsmVariables.GetFsmFloat("ava_hurt_force").Value = 0;
            fControl.SendEvent("ava event hurt");
        }
    }

    public void avaGetHit(float force)
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            FsmVariables.GlobalVariables.GetFsmFloat("ava_hurt_force").Value = (-1) * force * FsmVariables.GlobalVariables.GetFsmFloat("direction_Ava").Value;
            fControl.SendEvent("ava get hit");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled");
            FsmVariables.GlobalVariables.GetFsmFloat("ava_hurt_force").Value = (-1) * force * FsmVariables.GlobalVariables.GetFsmFloat("direction_Ava").Value;
            fControl.SendEvent("ava get hit");
        }
    }

    public void avaFainting()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava event faint");
        }
    }

    public void avaAwake()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
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
                    fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Event").SendEvent("killed by Scorpion");
                    break;
                case "MECB":
                    fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Event").SendEvent("killed by MECB");
                    break;
                case "Larvae":
                    fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Event").SendEvent("killed by Larvae");
                    break;
                case "Inuji":
                    fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Event").SendEvent("killed by Inuji");
                    break;
                case "Ambusher":
                    fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Event").SendEvent("killed by Ambusher");
                    break;
                case "Silencer":
                    fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Event").SendEvent("killed by Ambusher");
                    break;
                default:
                    ODM.errorLog(transform.name, "avaGetKilled missing mate target.", "");
                    break;
            }
        }
        else
        {
            ODM.log(transform.name,  "Ava get killed. No current mate.");
            PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Event");
            if (fControl.isActiveAndEnabled)
            {
                fControl.SendEvent("ava event dead");
            }
            else
            {
                PlayMakerFSM fControlDisabled = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Event");
                fControlDisabled.SendEvent("ava event dead");
            }
        }
    }


    public void setPlayerWait()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava wait");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled");
            fControlDisabled.SendEvent("ava wait");
        }
    }

    public void avaEventAim()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava event aim");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled");
            fControlDisabled.SendEvent("ava event aim");
        }
    }

    public void avaEventWalk()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("ava event walk");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled");
            fControlDisabled.SendEvent("ava event walk");
        }
    }
    public void setPlayerContinue()
    {
        PlayMakerFSM fControl = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control");
        if (fControl.isActiveAndEnabled)
        {
            fControl.SendEvent("jump_Idle");
        }
        else
        {
            PlayMakerFSM fControlDisabled = fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value, "Player Control Disabled");
            fControlDisabled.SendEvent("jump_Idle");
        }
    }
    public void avaLocation(GameObject _location_obj)
    {
        var avaTransform = FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value.transform.GetComponent<Transform>();
        avaTransform.position = new Vector3(_location_obj.transform.position.x, 0, 0);
    }
}
