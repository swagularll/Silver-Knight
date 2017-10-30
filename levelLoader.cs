using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class levelLoader : MonoBehaviour
{
    public AudioClip doorSound;
    private eventCenter eventCenter;
    private AudioSource aud;
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.clip = doorSound;
        eventCenter = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value.GetComponent<eventCenter>();
        PlayMakerFSM fsmBGM =  fsmHelper.getFsm("BGM Manager", "FSM");
        if (!fsmBGM.FsmVariables.GetFsmBool("isDefault").Value)
            fsmBGM.SendEvent("set default bgm");

        if (fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value, "Scene Controller").
            FsmVariables.GetFsmBool("isFirstLoad").Value)//Things need to do when player first time enter the game...
        {
            fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value, "Scene Controller").FsmVariables.GetFsmBool("isFirstLoad").Value = false;
            Debug.Log("First Load Detected");
        }
        else
        {
            setPlayerPosition();
            eventCenter.GetComponent<actionControl>().setIdle();
        }
    }

    public void setPlayerPosition()
    {
        string intoLocation = FsmVariables.GlobalVariables.GetFsmString("fromDoor").Value;
        var avaTransform = FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value.transform.GetComponent<Transform>();
        float door_x = 0f;
        switch (intoLocation)
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
        avaTransform.position = new Vector3(door_x, 0, 0);
    }

}
