using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class creditShowing : MonoBehaviour
{
    public TextAsset creditList;
    public Text txtTitle;
    public Text txtContent;

    private string[] string_CreditList;
    private List<CCredit> creditCollection = new List<CCredit>();
    private int currentIndex = -1;
    void Awake()
    {
        string fileLocaion = @"Data Collection\" + PlayerPrefs.GetString("lang") + @"\Credit";
        creditList = Resources.Load<TextAsset>(fileLocaion);

        string_CreditList = creditList.text.Split('\n');
        CCredit c = new CCredit();

        for (int i = 0; i < string_CreditList.Length; i++)
        {
            if (string_CreditList[i].IndexOf("[TITLE]") != -1)
            {
                c = new CCredit();
                c.Title = string_CreditList[i].Replace("[TITLE]", "");
                creditCollection.Add(c);
            }
            else
            {
                if (!String.IsNullOrEmpty(string_CreditList[i]) || string_CreditList[i] != "\r")
                    creditCollection[creditCollection.Count - 1].Content += string_CreditList[i] + "\n\n";
            }

        }
    }

    public void nextList()
    {
        if (currentIndex == (creditCollection.Count - 1))
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }
        CCredit c = creditCollection[currentIndex];
        txtTitle.GetComponent<Text>().text = c.Title;
        txtContent.GetComponent<Text>().text = c.Content;
    }
    class CCredit
    {
        string title = "";
        string content = "";
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }
    }
}
