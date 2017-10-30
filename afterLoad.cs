using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class afterLoad : MonoBehaviour
{
    private GameObject eventManager;
    private eventCenter eventCenter;

    void Start()
    {
        eventManager = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value;
        eventCenter = eventManager.GetComponent<eventCenter>();
        CMap currentMap = eventManager.GetComponent<MapDatabase>().getMap(Application.loadedLevelName);
        string mapDisplayText = currentMap.name + " " + currentMap.title;
        eventCenter.renewLocation(mapDisplayText);
        string stageFlagName = "Area" + Application.loadedLevelName;
        eventCenter.setFlagTrue(stageFlagName);
        fsmHelper.getFsm(FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value, "Fade").SendEvent("fade in");
        fsmHelper.getFsm(transform.gameObject,"FSM").SendEvent("broadcast ready");
    }
}
