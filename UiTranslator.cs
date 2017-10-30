using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using HutongGames.PlayMaker;

public class UiTranslator : MonoBehaviour
{

    public string key;
    public bool specialTranslation = false;
    //public bool prefabText = false;
    public string prefabKey = "";

    private PlayMakerFSM fsmVariables;
    private string replacementKey = "[INPUT]";
    void Start()
    {
        resetText();
    }
    public void setKey(string _key)
    {
        key = _key;
        resetText();
    }
    public void resetText()
    {
        if (!String.IsNullOrEmpty(key))
        {
            if (!String.IsNullOrEmpty(prefabKey))
            {
                GetComponent<Text>().text = FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value.
                    GetComponent<UiInfomationHolder>().getText(key.Trim()).Replace(replacementKey, PlayerPrefs.GetString(prefabKey));
            }
            else
            {
                GetComponent<Text>().text = FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value.GetComponent<UiInfomationHolder>().getText(key.Trim());
            }
        }
        else
            ODM.errorLog(transform.name, "Missing key: " + key, "");
    }
    public void resetText(string _additionalText)
    {
        if (!String.IsNullOrEmpty(key))
            GetComponent<Text>().text = FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value.
                GetComponent<UiInfomationHolder>().getText(key.Trim()).Replace(replacementKey, _additionalText);
        else
            ODM.errorLog(transform.name,
                "Missing key: " + key,
                "");
    }
}
