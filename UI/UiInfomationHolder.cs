using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UiInfomationHolder : MonoBehaviour
{
    public TextAsset txt_source;
    private string[] txtContext;
    void Awake()
    {
        txt_source = Resources.Load<TextAsset>(ODMVariable.resource.ui_translation_text);
        txtContext = txt_source.text.Split('\n');
    }

    public string getText(string _key)
    {
        string translated_text = "";

        try
        {
            for (int i = 0; i < txtContext.Length; i++)
            {
                if (txtContext[i].IndexOf("[" + _key + "]") != -1)
                {
                    translated_text = txtContext[i];
                    break;
                }
            }

            string[] temp = translated_text.Split('\t');

            string lang = ODMVariable.system.lang;

            if (lang.Equals(ODMVariable.text.lang_en))
            {
                translated_text = temp[1];
            }
            if (lang.Equals(ODMVariable.text.lang_zh))
            {
                translated_text = temp[2];
            }
            if (lang.Equals(ODMVariable.text.lang_jp))
            {
                translated_text = temp[3];
            }
            translated_text = translated_text.Replace("\\n", Environment.NewLine);
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name, ex.ToString());
        }
        return translated_text;
    }

}
