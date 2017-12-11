using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using System;

public class intelligenceDash : MonoBehaviour
{
    public bool intelligence_switch = false;//called by menuManager
    private TextAsset txtSource;

    public GameObject model_btnintelligence;
    public GameObject model_txtintelligenceName;
    public GameObject Intelligence_Context_Area_Text;
    public GameObject Intelligence_Title_in_Sheet;
    public GameObject Text_Intelligence_Page_Count;
    public GameObject txtIntelligencePage;
    public GameObject Intelligence_Context_Area;
    public GameObject Read_Intelligence_Hint;
    public GameObject Select_Intelligence_Hint;
    public GameObject Intelligence_List_Button_Container;
    public GameObject Intelligence_Left_Arrow;
    public GameObject Intelligence_Right_Arrow;

    private List<CIntelligence> intelligenceCollection;
    private List<GameObject> buttonCollection;
    private List<GameObject> txtintelligenceTagCollection;

    private AudioSource aud;

    private bool state_control = false;
    private bool is_reading_intelligence = false;

    private string[] txt_content;
    private int slot_count = 8;

    private int current_selected_index = -1;//start with 0
    private int current_intelligence_page = -1;//start with 1
    private int max_page_count = -1;

    private int current_list_page = -1;
    private int max_list_page = -1;


    public Sprite img_item_selected;
    public Sprite img_item_unselected;

    private Color32 CSelectedColor = ODMVariable.color.white;
    private Color32 CUnselectedColor = ODMVariable.color.gray_transparent;
    private Color32 CBlack = ODMVariable.color.black;

    private void Awake()
    {
        //renew code
        txtSource = Resources.Load<TextAsset>(ODMVariable.path.intelligence_resource);
        intelligenceCollection = new List<CIntelligence>();
        buttonCollection = new List<GameObject>();
        txtintelligenceTagCollection = new List<GameObject>();
        aud = GetComponent<AudioSource>();
    }

    void Start()
    {
        //renew code
        txt_content = txtSource.text.Split('\n');
        initialize_Intelligence_List();
    }

    void Update()
    {
        if (intelligence_switch && !state_control)//initialize and select the first intelligence
        {
            state_control = true;//for the first time loading
            nextIntelligence();
        }
        else if (intelligence_switch && state_control)//intelligence Select section
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (getSlotIndex() == slot_count - 1)//last intelligence cannot go down
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                    aud.Play();
                }
                else
                {

                    if (is_reading_intelligence)
                        stopManagement();
                    nextIntelligence();
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    aud.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {

                if (getSlotIndex() == 0)
                {
                    closePanel();
                }
                else
                {
                    if (is_reading_intelligence)
                        stopManagement();
                    previousintelligence();
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    aud.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!is_reading_intelligence)
                {
                    if (current_list_page < max_list_page)
                    {
                        nextListPage();
                        aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                        aud.Play();
                    }
                    else//Last list
                    {
                        aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                        aud.Play();
                    }
                }
                else
                {
                    if (current_intelligence_page < intelligenceCollection[current_selected_index].Content.Count)
                    {
                        current_intelligence_page++;
                        switchintelligencePage();
                        aud.clip = Resources.Load<AudioClip>(audioResource.page_turn);
                        aud.Play();
                    }
                    else
                    {
                        aud.clip = Resources.Load<AudioClip>(audioResource.negative_small);
                        aud.Play();
                    }
                }

            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!is_reading_intelligence)
                {
                    if (current_list_page != 1)
                    {
                        previousListPage();
                        aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    }
                    else
                    {
                        aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                    }
                    aud.Play();
                }
                else
                {
                    if (current_intelligence_page != 1)
                    {
                        current_intelligence_page--;
                        switchintelligencePage();
                        aud.clip = Resources.Load<AudioClip>(audioResource.page_turn);
                        aud.Play();
                    }
                    else
                    {
                        aud.clip = Resources.Load<AudioClip>(audioResource.negative_small);
                        aud.Play();
                    }
                }

            }


            if (Input.GetKeyDown(KeyCode.C))
            {
                if (intelligenceCollection[current_selected_index].Unlocked)
                {
                    if (!is_reading_intelligence)
                    {
                        aud.clip = Resources.Load<AudioClip>(audioResource.electrical);
                        aud.Play();
                        startManagement();
                        showIntelligence();
                        switchintelligencePage();
                    }
                    else
                    {
                        aud.clip = Resources.Load<AudioClip>(audioResource.negative_small);
                        aud.Play();
                    }
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.negative_small);
                    aud.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                if (is_reading_intelligence)
                {
                    stopManagement();
                    aud.clip = Resources.Load<AudioClip>(audioResource.electrical_out);
                    aud.Play();
                }
                else
                {
                    closePanel();
                }
            }

        }
    }

    //Main script structure
    private void initialize_Intelligence_List()
    {
        for (int i = 0; i < txt_content.Length; i++)
        {
            if (txt_content[i].IndexOf("[") != -1 && txt_content[i].IndexOf(ODMVariable.tag.end) == -1 && txt_content[i].IndexOf(ODMVariable.tag.pause) == -1)
            {
                CIntelligence d = new CIntelligence();
                StringBuilder sb = new StringBuilder();

                //flagName: [NAME] => NAME
                string flagName = txt_content[i].Substring(1, txt_content[i].IndexOf("]") - 1);
                d.Key = flagName.Replace("\r", "").Replace("\n", "");//FileName: [INT1] => INT1
                d.Unlocked = ODMObject.event_manager.GetComponent<eventCenter>().getFlagBool(flagName);
                d.Name = txt_content[i].Replace("[" + flagName + "]", "");//FileName: [intelligence02] => intelligence02
                i++;

                for (int k = i; k < txt_content.Length; k++)
                {
                    if (txt_content[k].ToString().IndexOf(ODMVariable.tag.end) != -1)
                    {
                        d.Content.Add(sb.ToString());
                        i = k;
                        intelligenceCollection.Add(d);
                        break;
                    }
                    else if (txt_content[k].ToString().IndexOf(ODMVariable.tag.pause) != -1)
                    {
                        d.Content.Add(sb.ToString());
                        sb = new StringBuilder();
                    }
                    else
                    {
                        sb.AppendLine(txt_content[k]);
                    }
                }
            }
        }

        for (int i = 0; i < slot_count; i++)
        {
            var b = Instantiate(model_btnintelligence);
            buttonCollection.Add(b);
            var txtName = Instantiate(model_txtintelligenceName);
            txtName.transform.SetParent(buttonCollection[i].transform);
            txtintelligenceTagCollection.Add(txtName);
            buttonCollection[i].transform.SetParent(Intelligence_List_Button_Container.transform);
        }
    }

    private void nextIntelligence()
    {
        current_selected_index++;
        setSelection();
        setCover();
    }
    private void previousintelligence()
    {
        current_selected_index--;
        setSelection();
        setCover();
    }



    private void nextListPage()
    {
        current_list_page++;
        current_selected_index += slot_count;
        setSelection();
        setCover();
        updateListPageText();
    }
    private void previousListPage()
    {
        current_list_page--;
        current_selected_index -= slot_count;
        setSelection();
        setCover();
        updateListPageText();
    }
    private void stopManagement()
    {
        is_reading_intelligence = false;
        txtIntelligencePage.GetComponent<CanvasGroup>().alpha = 0;
        Intelligence_Left_Arrow.GetComponent<CanvasGroup>().alpha = 0;
        Intelligence_Right_Arrow.GetComponent<CanvasGroup>().alpha = 0;
        Intelligence_Title_in_Sheet.GetComponent<CanvasGroup>().alpha = 1;
        Read_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 1;
        Intelligence_Context_Area_Text.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void startManagement()
    {
        is_reading_intelligence = true;
        Intelligence_Title_in_Sheet.GetComponent<CanvasGroup>().alpha = 0;
        Read_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 0;
        Intelligence_Context_Area_Text.GetComponent<CanvasGroup>().alpha = 1;
        txtIntelligencePage.GetComponent<CanvasGroup>().alpha = 1;
    }


    private void setCover()
    {
        if (current_selected_index != -1)
        {
            refreshUnlocked();
            CIntelligence d = intelligenceCollection[current_selected_index];
            if (d.Unlocked)
            {
                Select_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 1;
                Intelligence_Title_in_Sheet.GetComponent<CanvasGroup>().alpha = 1;
                Read_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 1;
                Intelligence_Title_in_Sheet.GetComponent<Text>().text = d.Name;
                Intelligence_Context_Area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, true);
            }
            else
            {
                Select_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 1;
                Intelligence_Title_in_Sheet.GetComponent<CanvasGroup>().alpha = 0;
                Read_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 0;
                Intelligence_Context_Area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, false);
            }
        }
        else
        {
            Select_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 0;

            Intelligence_Left_Arrow.GetComponent<CanvasGroup>().alpha = 0;
            Intelligence_Right_Arrow.GetComponent<CanvasGroup>().alpha = 0;
            txtIntelligencePage.GetComponent<CanvasGroup>().alpha = 0;
            Intelligence_Title_in_Sheet.GetComponent<CanvasGroup>().alpha = 0;
            Read_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 0;
            Intelligence_Context_Area_Text.GetComponent<CanvasGroup>().alpha = 0;
            Intelligence_Context_Area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, false);
        }
    }

    private void refreshUnlocked()
    {
        for (int i = 0; i < intelligenceCollection.Count; i++)
        {
            intelligenceCollection[i].Unlocked = ODMObject.event_manager.GetComponent<eventCenter>().getFlagBool(intelligenceCollection[i].Key);
        }
    }
    private void updateListPageText()
    {
        max_list_page = intelligenceCollection.Count / slot_count;
        if (intelligenceCollection.Count % slot_count != 0)
        {
            max_list_page++;
        }
        Text_Intelligence_Page_Count.GetComponent<Text>().text = current_list_page + "/" + max_list_page;
    }

    private void setSelection()
    {
        for (int i = 0; i < buttonCollection.Count; i++)
        {
            CIntelligence d = intelligenceCollection[(current_list_page - 1) * slot_count + i];
            if (d.Unlocked)
                txtintelligenceTagCollection[i].GetComponent<Text>().text = d.Name;
            else
                txtintelligenceTagCollection[i].GetComponent<Text>().text = ODMVariable.text.no_data;

            if (getSlotIndex() == i)//selected
            {
                buttonCollection[i].GetComponent<Image>().sprite = img_item_selected;
                buttonCollection[i].GetComponent<Image>().color = CSelectedColor;//is selected
            }
            else if (d.Unlocked)//locked
            {
                buttonCollection[i].GetComponent<Image>().sprite = img_item_unselected;
                buttonCollection[i].GetComponent<Image>().color = CUnselectedColor; // unlocked
            }
            else
            {
                buttonCollection[i].GetComponent<Image>().sprite = img_item_unselected;
                buttonCollection[i].GetComponent<Image>().color = CBlack; // locked
            }
        }
    }

    private void showIntelligence()
    {
        //Show intelligence text
        CIntelligence d = intelligenceCollection[current_selected_index];
        max_page_count = d.Content.Count;
        current_intelligence_page = 1;
        setDocuemntPageText();
        if (max_page_count > 1)
        {
            Intelligence_Left_Arrow.GetComponent<CanvasGroup>().alpha = 0;
            Intelligence_Right_Arrow.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            Intelligence_Left_Arrow.GetComponent<CanvasGroup>().alpha = 0;
            Intelligence_Right_Arrow.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    private void switchintelligencePage()
    {
        CIntelligence d = intelligenceCollection[current_selected_index];
        Intelligence_Context_Area_Text.GetComponent<Text>().text = d.Content[current_intelligence_page - 1];

        if (current_intelligence_page == d.Content.Count)//final page
        {
            Intelligence_Left_Arrow.GetComponent<CanvasGroup>().alpha = 1;
            Intelligence_Right_Arrow.GetComponent<CanvasGroup>().alpha = 0;
        }
        else if (current_intelligence_page == 1)
        {
            Intelligence_Left_Arrow.GetComponent<CanvasGroup>().alpha = 0;
            Intelligence_Right_Arrow.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            Intelligence_Left_Arrow.GetComponent<CanvasGroup>().alpha = 1;
            Intelligence_Right_Arrow.GetComponent<CanvasGroup>().alpha = 1;
        }
        setDocuemntPageText();
    }

    private void setDocuemntPageText()
    {
        if (current_intelligence_page == -1)
        {
            txtIntelligencePage.GetComponent<Text>().text = "";
        }
        else
        {
            txtIntelligencePage.GetComponent<Text>().text = current_intelligence_page + "/" + max_page_count;
        }
    }

    private int getSlotIndex()
    {
        int idx = -1;
        if (current_selected_index != -1)
            idx = current_selected_index % slot_count;
        return idx;
    }

    public void preSetting()
    {
        refreshUnlocked();
        setListFirstPage();
        setSelection();
        setCover();
    }

    public void openPanel()
    {
        aud.clip = Resources.Load<AudioClip>(audioResource.electrical);
        aud.Play();
        intelligence_switch = true;
        preSetting();
    }

    public void closePanel()
    {
        aud.clip = Resources.Load<AudioClip>(audioResource.electrical_out);
        aud.Play();
        GetComponent<menuManager>().disableMiddleTab();
        intelligence_switch = false;
        state_control = false;
        setListFirstPage();
        current_selected_index = -1;
        stopManagement();
        preSetting();
    }

    private void setListFirstPage()
    {
        current_list_page = 1;
        updateListPageText();
    }

    public CIntelligence getIntelligence(string _intelligenceFlagName)
    {
        CIntelligence intel = intelligenceCollection.Where(x => x.Key.Equals(_intelligenceFlagName)).FirstOrDefault();
        if (intel == null)
            ODM.errorLog(transform.name, "getIntelligence Error. Key: " + _intelligenceFlagName);
        return intel;
    }

    #region LOCAL CLASS
    public class CIntelligence
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public List<string> Content = new List<string>();
        public bool Unlocked { get; set; }
    }
    #endregion

}