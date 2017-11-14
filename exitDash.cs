using Assets.Script.ODM_Widget;
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
        fsm = fsmHelper.getFsm(transform.gameObject, "FSM");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        aud.clip = Resources.Load<AudioClip>(audioManager.electrical);
        aud.Play();
        Time.timeScale = 0f;
        is_open = true;
        ani.SetBool("isOpen", is_open);
        FsmVariables.GlobalVariables.GetFsmBool("isSystemLock").Value = true;
        fsm.SendEvent("show menu");
        ODMObject.character_ava.GetComponent<actionControl>().disableControl();
    }

    public void closePanel()
    {
        aud.clip = Resources.Load<AudioClip>(audioManager.electricalExit);
        aud.Play();
        Time.timeScale = 1f;
        is_open = false;
        ani.SetBool("isOpen", is_open);
        FsmVariables.GlobalVariables.GetFsmBool("isSystemLock").Value = false;
        fsm.SendEvent("hide menu");
        ODMObject.character_ava.GetComponent<actionControl>().enableControl();
    }
}
