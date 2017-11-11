using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using System;
using Assets.Script.ODM_Widget;

public class saveList : MonoBehaviour
{

    public GameObject saveButton;
    public GameObject buttonContainer;
    public GameObject SaveLoadContainer;
    public GameObject pageText;

    public GameObject rightArror;
    public GameObject leftArror;
    private Animator ani;
    private audioManager s = new audioManager();
    private AudioSource aud;

    public int buttonNumber = 10;

    private List<GameObject> listButton = new List<GameObject>();
    private int currentPage = 0;
    private int mainIndex = 0; //full scale
    private int buttonIndex = 0; //per page
    private int maxPage = 0;
    private List<ODM.ODMDictionary> listSave = new List<ODM.ODMDictionary>();

    void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    public void initialization()
    {
        listButton.Clear();
        listSave.Clear();
        foreach (Transform child in buttonContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        string[] save_collection = Directory.GetFiles(PlayerPrefs.GetString("save_folder_directory"), "*.json");
        foreach (string save_record_file in save_collection)
        {
            StreamReader sr = new StreamReader(save_record_file, Encoding.Default);
            saveRecord save_record = new saveRecord(sr.ReadToEnd());
            listSave.Add(save_record.save_data);
            sr.Close();
        }


        listSave.Sort(delegate (ODM.ODMDictionary x, ODM.ODMDictionary y)
        {
            if (x.getValue("save_created_time") == null && y.getValue("save_created_time") == null) return 0;
            else if (x.getValue("save_created_time") == null) return -1;
            else if (y.getValue("save_created_time") == null) return 1;
            else return y.getValue("save_created_time").CompareTo(x.getValue("save_created_time"));
        });

        if (listSave.Count == 0)
        {
            showNoRecord();
        }
        else
        {
            maxPage = listSave.Count / buttonNumber;
            currentPage = 0;
            mainIndex = 0;

            for (int i = 0; i < buttonNumber; i++)
            {
                GameObject b = Instantiate(saveButton);
                b.transform.SetParent(buttonContainer.transform);
                b.transform.GetComponent<saveButton>().map_database_holder = transform.gameObject;
                listButton.Add(b);
            }
            listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
        }
        renewPage();
        fsmHelper.getFsm(SaveLoadContainer, "FSM").SendEvent("show data");
    }
    public void renewPage()
    {
        if (listButton.Count != 0)
        {
            for (int i = 0; i < buttonNumber; i++)
            {
                int idx = currentPage * buttonNumber + i;//loop of mainIndex
                if (idx < listSave.Count)
                {
                    listButton[i].GetComponent<saveButton>().initilizeData(listSave[idx]);
                }
                else
                {
                    listButton[i].GetComponent<saveButton>().setDead();
                }
            }
            pageText.GetComponent<Text>().text = (currentPage + 1) + "/" + (maxPage + 1);
            checkArrorColor();
        }
    }

    private void showNoRecord()
    {
        fsmHelper.getFsm(SaveLoadContainer, "FSM").SendEvent("no data");
    }

    public void nextPage()
    {
        if (currentPage < maxPage)
        {
            listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
            currentPage++;
            int idx = currentPage * buttonNumber + getIndex();
            if (idx > listSave.Count - 1)
                mainIndex = listSave.Count - 1;
            else
                mainIndex += buttonNumber;
            renewPage();
            listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
            aud.clip = Resources.Load<AudioClip>(s.pageTurn);
            aud.Play();
        }
        else
        {
            aud.clip = Resources.Load<AudioClip>(s.selectionNegative);
            aud.Play();
        }
    }

    private void nextPageButton()
    {
        listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
        currentPage++;
        mainIndex++;
        renewPage();
        listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
        aud.clip = Resources.Load<AudioClip>(s.pageTurn);
        aud.Play();
    }
    private void previousPage()
    {
        if (currentPage != 0)
        {
            listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
            currentPage--;
            mainIndex -= buttonNumber;
            renewPage();
            listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
            aud.clip = Resources.Load<AudioClip>(s.pageTurn);
            aud.Play();
        }
        else
        {
            aud.clip = Resources.Load<AudioClip>(s.selectionNegative);
            aud.Play();
        }
    }
    private void previousPageButton()
    {
        listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
        currentPage--;
        mainIndex--;
        renewPage();
        listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
        aud.clip = Resources.Load<AudioClip>(s.pageTurn);
        aud.Play();
    }
    public void nextButton()
    {
        if (getIndex() == (buttonNumber - 1) && currentPage < maxPage)//at final button, going to switch out
        {
            nextPageButton();
        }
        else if (getIndex() < buttonNumber)
        {
            if (listButton[getIndex() + 1].GetComponent<saveButton>().isSelectable)
            {
                listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
                mainIndex++;
                listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
                aud.clip = Resources.Load<AudioClip>(s.selectionSwitch);
                aud.Play();
            }
            else
            {
                aud.clip = Resources.Load<AudioClip>(s.selectionNegative);
                aud.Play();
            }
        }
        else
        {
            aud.clip = Resources.Load<AudioClip>(s.selectionNegative);
            aud.Play();
        }
    }
    private void previousButton()
    {
        if (getIndex() == 0 && currentPage != 0)
        {
            previousPageButton();
        }
        else
        {
            if (getIndex() != 0)
            {
                listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
                mainIndex--;
                listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
                aud.clip = Resources.Load<AudioClip>(s.selectionSwitch);
                aud.Play();
            }
            else
            {
                aud.clip = Resources.Load<AudioClip>(s.selectionNegative);
                aud.Play();
            }
        }
    }

    private int getIndex()
    {
        return mainIndex % buttonNumber;
    }

    public void setCode()
    {
        PlayerPrefs.SetString("save_code", listSave[mainIndex].getValue("save_id"));
    }

    public void checkArrorColor()
    {
        if (maxPage != 0)
        {
            if (currentPage == 0)
                fsmHelper.getFsm(leftArror, "FSM").SendEvent("set unselectable");
            else
                fsmHelper.getFsm(leftArror, "FSM").SendEvent("set selectable");

            if (currentPage == maxPage)
                fsmHelper.getFsm(rightArror, "FSM").SendEvent("set unselectable");
            else
                fsmHelper.getFsm(rightArror, "FSM").SendEvent("set selectable");
        }
        else
        {
            fsmHelper.getFsm(leftArror, "FSM").SendEvent("set unselectable");
            fsmHelper.getFsm(rightArror, "FSM").SendEvent("set unselectable");
        }
    }
}
