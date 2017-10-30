using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UiInfomationHolder : MonoBehaviour
{
    public TextAsset txtSource;
    private string lang;
    private string[] txtContext;
    private GameObject eventManager;
    void Awake()
    {
        txtContext = txtSource.text.Split('\n');
        //GetComponent<EventCenter>
    }

    public string getText(string key)
    {
        string s = "";

        try
        {
            lang = PlayerPrefs.GetString("lang");
            for (int i = 0; i < txtContext.Length; i++)
            {
                if (txtContext[i].IndexOf("[" + key + "]") != -1)
                {
                    s = txtContext[i];
                    break;
                }
            }
            //will fix this latter => lang = eventManager....

            string[] temp = s.Split('\t');

            if (lang.Equals("EN"))
            {
                s = temp[1];
            }
            if (lang.Equals("ZH"))
            {
                s = temp[2];
            }
            if (lang.Equals("JP"))
            {
                s = temp[3];
            }
            s = s.Replace("\\n", Environment.NewLine);
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name,
                "key: " + key, ex.ToString());
        }
        return s;
    }

}
