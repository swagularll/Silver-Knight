using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class guiBoolMonitor : MonoBehaviour {

    public string monitor_variable = "";
    public string true_key = "";
    public string false_key = "";
    void Update () {
       //if(FsmVariables.GlobalVariables.GetFsmBool(monitor_variable).Value)
       //     GetComponent<Text>().text = ODMObj.language_translator.GetComponent<UiInfomationHolder>().getText(true_key);
       //else
       //     GetComponent<Text>().text = ODMObj.language_translator.GetComponent<UiInfomationHolder>().getText(false_key);
    }
}
