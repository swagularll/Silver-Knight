using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using HutongGames.PlayMaker;

public class UiTranslator : MonoBehaviour
{

    public string key;
    public bool is_replacement_translation = false;
    public string prefabKey = "";

    private PlayMakerFSM fsmVariables;
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
            GetComponent<Text>().text = ODMObject.language_translator.GetComponent<UiInfomationHolder>().getText(key.Trim());
            //renew code
            //if (!String.IsNullOrEmpty(prefabKey))
            //{
            //    GetComponent<Text>().text = ODMObject.language_translator.
            //        GetComponent<UiInfomationHolder>().getText(key.Trim()).Replace(ODMVariable.tag.input, PlayerPrefs.GetString(prefabKey));
            //}
            //else
            //{
            //}
        }
        else
        {
            ODM.errorLog(transform.name, "Missing key: " + key);
        }
    }
    public void resetText(string _additionalText)
    {
        if (!String.IsNullOrEmpty(key))
            GetComponent<Text>().text = ODMObject.language_translator.
                GetComponent<UiInfomationHolder>().getText(key.Trim()).Replace(ODMVariable.tag.input, _additionalText);
        else
            ODM.errorLog(transform.name,"Missing key: " + key);
    }
}
