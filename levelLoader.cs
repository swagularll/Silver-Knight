using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

using System.Collections.Generic;

public class levelLoader : MonoBehaviour
{
    public AudioClip sound_door_open;
    private eventCenter event_center;
    private AudioSource aud;

    private void InitialzeScript()
    {
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();
        aud = GetComponent<AudioSource>();
    }
    void Start()
    {
        InitialzeScript();

        //Open door sound
        aud.clip = sound_door_open;

        //BGM setting
        if (!ODMVariable.fsm.bgm_manager.FsmVariables.GetFsmBool(ODMVariable.local.is_default).Value)
            ODMVariable.fsm.bgm_manager.SendEvent(eventName.set_default_bgm);

        //Things need to do when player first time enter the game.
        if (ODMVariable.fsm.scene_controller.FsmVariables.GetFsmBool(ODMVariable.local.is_first_load).Value)
        {
            //Load from initialization scene
            ODMVariable.fsm.scene_controller.FsmVariables.GetFsmBool(ODMVariable.local.is_first_load).Value = false;
        }
        else
        {
            setPlayerPosition();//Set player position by map
            ODMObject.character_ava.GetComponent<actionControl>().setIdle();
        }
    }

    public void setPlayerPosition()
    {
        string intro_location = ODMVariable.world.from_door;
        var ava_transform = ODMObject.character_ava.transform.GetComponent<Transform>();
        float door_x = 0f;
        switch (intro_location)
        {
            case "Map Right Path":
                door_x = GameObject.Find("Map Left Path").GetComponent<Transform>().position.x + 3;
                break;
            case "Map Left Path":
                door_x = GameObject.Find("Map Right Path").GetComponent<Transform>().position.x - 3;
                break;
            case "Map Up Door":
                aud.Play();
                door_x = GameObject.Find("Map Down Door").GetComponent<Transform>().position.x;
                break;
            case "Map Down Door":
                aud.Play();
                door_x = GameObject.Find("Map Up Door").GetComponent<Transform>().position.x;
                break;
            case "Map Left Door":
                aud.Play();
                door_x = GameObject.Find("Map Right Door").GetComponent<Transform>().position.x - 3;
                break;
            case "Map Right Door":
                aud.Play();
                door_x = GameObject.Find("Map Left Door").GetComponent<Transform>().position.x + 3;
                break;
        }
        ava_transform.position = new Vector3(door_x, -0.3f, 0);
    }

}
