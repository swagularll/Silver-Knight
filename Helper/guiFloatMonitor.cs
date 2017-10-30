using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class guiFloatMonitor : MonoBehaviour
{
    public string variable_name = "";
    public string monitor_variable = "";
    void Update()
    {
        GetComponent<Text>().text = variable_name+ ": " + FsmVariables.GlobalVariables.GetFsmFloat(monitor_variable).Value.ToString();
    }
}
