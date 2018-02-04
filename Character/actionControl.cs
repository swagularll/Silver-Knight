using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class actionControl : MonoBehaviour
{
    private PlayMakerFSM getActiveFsm()
    {
        return ODMVariable.fsm.player_control.isActiveAndEnabled ? ODMVariable.fsm.player_control : ODMVariable.fsm.player_control_disabled;
    }
    public void setIdle()
    {
        getActiveFsm().SendEvent(eventName.ava_jump_Idle);
    }
    public void avaPickUpItem()
    {
        getActiveFsm().SendEvent(eventName.ava_pick_item);
    }
    public void avaDownCheck()
    {
        getActiveFsm().SendEvent(eventName.ava_down_check);
    }
    public void avaGetHurt()
    {
        getActiveFsm().FsmVariables.GetFsmFloat(ODMVariable.local.ava_hurt_force).Value = 0;
        getActiveFsm().SendEvent(eventName.ava_event_hurt);
    }
    public void avaGetHit(float force)
    {
        ODMVariable.ava_hurt_force = (-1) * force * ODMVariable.ava_direction;
        getActiveFsm().SendEvent(eventName.ava_get_hit);
    }
    public void avaFainting()
    {
        getActiveFsm().SendEvent(eventName.ava_event_faint);
    }
    public void avaAwake()
    {
        getActiveFsm().SendEvent(eventName.ava_event_awake);
    }
    public void setPlayerWait()
    {
        getActiveFsm().SendEvent(eventName.ava_wait);
    }
    public void avaEventAim()
    {
        getActiveFsm().SendEvent(eventName.ava_event_aim);
    }

    public void avaEventWalk()
    {
        getActiveFsm().SendEvent(eventName.ava_event_walk);
    }
    public void setPlayerContinue()
    {
        getActiveFsm().SendEvent(eventName.ava_jump_Idle);
    }
    public void avaLocation(GameObject _location_obj)
    {
        var ava_transform = ODMObject.character_ava.transform.GetComponent<Transform>();
        ava_transform.position = new Vector3(_location_obj.transform.position.x, 0, 0);
    }
    public void enableControl()
    {
        getActiveFsm().SendEvent(eventName.resume_fsm);
    }
    public void disableControl()
    {
        PlayMakerFSM fsm = getActiveFsm();
        fsm.FsmVariables.GetFsmString(ODMVariable.common.previous_state).Value = fsm.ActiveStateName;
        fsm.SendEvent(eventName.hold_fsm);
    }

    public void reloadCheck()
    {

        inventoryDash inventory_dash = ODMObject.event_manager.GetComponent<inventoryDash>();
        if (
            //Full bullets - no need to reload
            inventory_dash.getPistolBulletAmount() == ODMVariable.bullet_max ||
            //No bullets remaining
            (inventory_dash.getItemAmount((int)ODMVariable.itemCatalogue.red_core_technology_energy) == 0 &&
            inventory_dash.getItemAmount((int)ODMVariable.itemCatalogue.empire_bullet) == 0) ||
            //No RCT bullets stock but with RCT bullets in pistol
            ((ODMVariable.ava_current_weapon == 1 || ODMVariable.ava_current_weapon == 3) &&
            inventory_dash.getItemAmount((int)ODMVariable.itemCatalogue.red_core_technology_energy) == 0 &&
            inventory_dash.getItemAmount((int)ODMVariable.itemCatalogue.rct_pistol) != 0) ||
            //No Empire bullets stock but with Empire bullets in pistol
            (ODMVariable.ava_current_weapon == 2 && inventory_dash.getItemAmount((int)ODMVariable.itemCatalogue.empire_bullet) == 0 &&
            inventory_dash.getItemAmount((int)ODMVariable.itemCatalogue.rct_pistol) != 0)
            )

        {
            setIdle();
        }
        else
        {
            ODMObject.event_manager.GetComponent<inventoryDash>().reload();
            getActiveFsm().SendEvent(eventName.start_reload);
        }
    }

    public void updateSlot()
    {
        BroadcastMessage(eventName.sys.check_weapon, SendMessageOptions.DontRequireReceiver);
    }

    public void avaGetKilled()
    {
        //second fix
        ODMObject.obj_matebar_canvas.GetComponent<CanvasGroup>().alpha = 0f;
        GameObject currentMate = ODMObject.current_mate;
        if (currentMate != null)
        {
            string mateName = currentMate.GetComponent<warmbugAction>().warmbug_name;
            switch (mateName)
            {
                case "Scorpion":
                    ODMVariable.fsm.event_controller.SendEvent("killed by Scorpion");
                    break;
                case "MECB":
                    ODMVariable.fsm.event_controller.SendEvent("killed by MECB");
                    break;
                case "Larvae":
                    ODMVariable.fsm.event_controller.SendEvent("killed by Larvae");
                    break;
                case "Inuji":
                    ODMVariable.fsm.event_controller.SendEvent("killed by Inuji");
                    break;
                case "Ambusher":
                    ODMVariable.fsm.event_controller.SendEvent("killed by Ambusher");
                    break;
                case "Silencer":
                    ODMVariable.fsm.event_controller.SendEvent("killed by Ambusher");
                    break;
                default:
                    ODM.errorLog(transform.name, "avaGetKilled missing mate target.");
                    break;
            }
        }
        else
        {
            ODM.log(transform.name, "Ava get killed without current mate.");
            ODMVariable.fsm.event_controller.SendEvent(eventName.ava_event_dead);
        }
    }
}
