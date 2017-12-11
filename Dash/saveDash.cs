using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using System;


public class saveDash : MonoBehaviour
{
    //renew code
    public GameObject saveButton;
    public GameObject buttonContainer;
    public GameObject pageText;

    public GameObject rightArror;
    public GameObject leftArror;

    private Animator ani;
    private AudioSource aud;

    private List<GameObject> listButton = new List<GameObject>();
    private List<ODM.ODMDictionary> listSave = new List<ODM.ODMDictionary>();

    private int list_button_max = 10;
    private int main_index = 0; //full scale
    private int button_index = 0; //per page
    private int page_current = 0;
    private int page_max = 0;

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

        string[] save_collection = Directory.GetFiles(ODMVariable.path.save_folder_directory, "*.json");
        foreach (string save_record_file in save_collection)
        {
            StreamReader sr = new StreamReader(save_record_file, Encoding.Default);
            saveRecord save_record = new saveRecord(sr.ReadToEnd());
            listSave.Add(save_record.save_data);
            sr.Close();
        }


        listSave.Sort(delegate (ODM.ODMDictionary x, ODM.ODMDictionary y)
        {
            if (x.getValue(ODMVariable.save.save_created_time) == null && y.getValue(ODMVariable.save.save_created_time) == null) return 0;
            else if (x.getValue(ODMVariable.save.save_created_time) == null) return -1;
            else if (y.getValue(ODMVariable.save.save_created_time) == null) return 1;
            else return y.getValue(ODMVariable.save.save_created_time).CompareTo(x.getValue(ODMVariable.save.save_created_time));
        });

        if (listSave.Count == 0)
        {
            showNoRecord();
        }
        else
        {
            page_max = listSave.Count / list_button_max;
            page_current = 0;
            main_index = 0;

            for (int i = 0; i < list_button_max; i++)
            {
                GameObject b = Instantiate(saveButton);
                b.transform.SetParent(buttonContainer.transform);
                b.transform.GetComponent<saveButton>().map_database_holder = transform.gameObject;
                listButton.Add(b);
            }
            listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
        }
        renewPage();
        fsmHelper.getFsm(transform.gameObject, ODMVariable.common.default_fsm).SendEvent(eventName.show_data);
    }
    public void renewPage()
    {
        if (listButton.Count != 0)
        {
            for (int i = 0; i < list_button_max; i++)
            {
                int idx = page_current * list_button_max + i;//loop of mainIndex
                if (idx < listSave.Count)
                {
                    listButton[i].GetComponent<saveButton>().initilizeData(listSave[idx]);
                }
                else
                {
                    listButton[i].GetComponent<saveButton>().setDead();
                }
            }
            pageText.GetComponent<Text>().text = (page_current + 1) + "/" + (page_max + 1);
            checkArrorColor();
        }
    }

    private void showNoRecord()
    {
        fsmHelper.getFsm(transform.gameObject, ODMVariable.common.default_fsm).SendEvent(eventName.no_data);
    }

    public void nextPage()
    {
        if (page_current < page_max)
        {
            listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
            page_current++;
            int idx = page_current * list_button_max + getIndex();
            if (idx > listSave.Count - 1)
                main_index = listSave.Count - 1;
            else
                main_index += list_button_max;
            renewPage();
            listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
            aud.clip = Resources.Load<AudioClip>(audioResource.page_turn);
            aud.Play();
        }
        else
        {
            aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
            aud.Play();
        }
    }

    private void nextPageButton()
    {
        listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
        page_current++;
        main_index++;
        renewPage();
        listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
        aud.clip = Resources.Load<AudioClip>(audioResource.page_turn);
        aud.Play();
    }
    private void previousPage()
    {
        if (page_current != 0)
        {
            listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
            page_current--;
            main_index -= list_button_max;
            renewPage();
            listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
            aud.clip = Resources.Load<AudioClip>(audioResource.page_turn);
            aud.Play();
        }
        else
        {
            aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
            aud.Play();
        }
    }
    private void previousPageButton()
    {
        listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
        page_current--;
        main_index--;
        renewPage();
        listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
        aud.clip = Resources.Load<AudioClip>(audioResource.page_turn);
        aud.Play();
    }
    public void nextButton()
    {
        if (getIndex() == (list_button_max - 1) && page_current < page_max)//at final button, going to switch out
        {
            nextPageButton();
        }
        else if (getIndex() < list_button_max)
        {
            if (listButton[getIndex() + 1].GetComponent<saveButton>().isSelectable)
            {
                listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
                main_index++;
                listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
                aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                aud.Play();
            }
            else
            {
                aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                aud.Play();
            }
        }
        else
        {
            aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
            aud.Play();
        }
    }
    private void previousButton()
    {
        if (getIndex() == 0 && page_current != 0)
        {
            previousPageButton();
        }
        else
        {
            if (getIndex() != 0)
            {
                listButton[getIndex()].GetComponent<saveButton>().isSelected = false;
                main_index--;
                listButton[getIndex()].GetComponent<saveButton>().isSelected = true;
                aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                aud.Play();
            }
            else
            {
                aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                aud.Play();
            }
        }
    }

    private int getIndex()
    {
        return main_index % list_button_max;
    }

    public void setCode()
    {
        ODMVariable.system.save_code = listSave[main_index].getValue(ODMVariable.save.save_id);
    }

    public void checkArrorColor()
    {
        if (page_max != 0)
        {
            if (page_current == 0)
                fsmHelper.getFsm(leftArror, ODMVariable.common.default_fsm).SendEvent(eventName.set_unselectable);
            else
                fsmHelper.getFsm(leftArror, ODMVariable.common.default_fsm).SendEvent(eventName.set_selectable);

            if (page_current == page_max)
                fsmHelper.getFsm(rightArror, ODMVariable.common.default_fsm).SendEvent(eventName.set_unselectable);
            else
                fsmHelper.getFsm(rightArror, ODMVariable.common.default_fsm).SendEvent(eventName.set_selectable);
        }
        else
        {
            fsmHelper.getFsm(leftArror, ODMVariable.common.default_fsm).SendEvent(eventName.set_unselectable);
            fsmHelper.getFsm(rightArror, ODMVariable.common.default_fsm).SendEvent(eventName.set_unselectable);
        }
    }
}
