using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class guiIntMonitor : MonoBehaviour
{
    public string monitor_variable = "";
    void Update()
    {
        GetComponent<Text>().text = FsmVariables.GlobalVariables.GetFsmInt(monitor_variable).Value.ToString();
    }
}
