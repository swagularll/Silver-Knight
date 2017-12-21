using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitDash : MonoBehaviour
{
    public GameObject panel_exit;

    public bool is_open = false;
    private AudioSource aud;
    private Animator ani;
    private PlayMakerFSM fsm;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        fsm = fsmHelper.getFsm(transform.gameObject, ODMVariable.common.default_fsm);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !ODMVariable.is_menu_open)
        {
            switchMenu();
        }
    }
    public void switchMenu()
    {
        if (!is_open)
        {
            openPanel();
        }
        else
        {
            closePanel();
        }
    }

    public void openPanel()
    {
        aud.clip = Resources.Load<AudioClip>(audioResource.electrical);
        aud.Play();
        Time.timeScale = 0f;
        is_open = true;
        ani.SetBool(ODMVariable.animation.is_open, is_open);
        ODMVariable.is_system_locked = true;
        fsm.SendEvent(eventName.show_menu);
        ODMObject.character_ava.GetComponent<actionControl>().disableControl();
    }

    public void closePanel()
    {
        aud.clip = Resources.Load<AudioClip>(audioResource.electrical_out);
        aud.Play();
        Time.timeScale = 1f;
        is_open = false;
        ani.SetBool(ODMVariable.animation.is_open, is_open);
        ODMVariable.fsm.scene_controller.SendEvent(eventName.end_system_hold);
        //ODMVariable.is_system_locked = false;
        fsm.SendEvent(eventName.hide_menu);
        ODMObject.character_ava.GetComponent<actionControl>().enableControl();
    }
}
