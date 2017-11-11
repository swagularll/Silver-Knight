using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using System;
using Assets.Script.ODM_Widget;

public class intelligenceDash : MonoBehaviour
{
    public bool intelligenceSelectSwitch = false;//called by menuManager
    public TextAsset txtSource;
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

    private audioManager aud_manager;
    private AudioSource aud;

    private bool stateControl = false;
    private bool isReadingintelligence = false;

    private string[] txtContent;
    private int slotCount = 8;

    private int currentSelectedIndex = -1;//start with 0
    private int currentintelligencePage = -1;//start with 1
    private int maxintelligencePage = -1;

    private int currentListPage = -1;
    private int maxListPage = -1;

    private string img_itemSelected = "UI/[UI]Item Selection";
    private string img_itemUnselected = "UI/[UI]Harf Transparent Side B Full";

    private Color32 CSelectedColor = new Color32(255, 255, 255, 255);
    private Color32 CUnselectedColor = new Color32(45, 45, 45, 150);
    private Color32 CBlack = new Color32(0, 0, 0, 255);

    void Start()
    {
        intelligenceCollection = new List<CIntelligence>();
        buttonCollection = new List<GameObject>();
        txtintelligenceTagCollection = new List<GameObject>();
        aud_manager = new audioManager();


        string intelligencePath = @"Data Collection\" + PlayerPrefs.GetString("lang") + @"\Intelligence";
        txtSource = Resources.Load<TextAsset>(intelligencePath);

        txtContent = txtSource.text.Split('\n');
        aud = GetComponent<AudioSource>();
        //Text_Intelligence_Page_Count = GameObject.Find("Text Intelligence Page Count");
        //Intelligence_Context_Area_Text = GameObject.Find("Intelligence Context Area Text");
        //Intelligence_No_Data_Text = GameObject.Find("Intelligence No Data Text");
        Intelligence_Title_in_Sheet = GameObject.Find("Intelligence Title in Sheet");
        //Intelligence_Context_Area = GameObject.Find("Intelligence Context Area");
        //txtIntelligencePage = GameObject.Find("txtIntelligencePage");
        //Read_Intelligence_Hint = GameObject.Find("Read Intelligence Hint");
        //Select_Intelligence_Hint = GameObject.Find("Select Intelligence Hint");
        initialize_Intelligence_List();
    }

    void Update()
    {
        if (intelligenceSelectSwitch && !stateControl)//initialize and select the first intelligence
        {
            stateControl = true;//for the first time loading
            nextIntelligence();
        }
        else if (intelligenceSelectSwitch && stateControl)//intelligence Select section
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (getSlotIndex() == slotCount - 1)//last intelligence cannot go down
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionNegative);
                    aud.Play();
                }
                else
                {

                    if (isReadingintelligence)
                        stopManagement();
                    nextIntelligence();
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
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
                    if (isReadingintelligence)
                        stopManagement();
                    previousintelligence();
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                    aud.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!isReadingintelligence)
                {
                    if (currentListPage < maxListPage)
                    {
                        nextListPage();
                        aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                        aud.Play();
                    }
                    else//Last list
                    {
                        aud.clip = Resources.Load<AudioClip>(aud_manager.selectionNegative);
                        aud.Play();
                    }
                }
                else
                {
                    if (currentintelligencePage < intelligenceCollection[currentSelectedIndex].Content.Count)
                    {
                        currentintelligencePage++;
                        switchintelligencePage();
                        aud.clip = Resources.Load<AudioClip>(aud_manager.pageTurn);
                        aud.Play();
                    }
                    else
                    {
                        aud.clip = Resources.Load<AudioClip>(aud_manager.negativeSmall);
                        aud.Play();
                    }
                }

            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!isReadingintelligence)
                {
                    if (currentListPage != 1)
                    {
                        previousListPage();
                        aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                    }
                    else
                    {
                        aud.clip = Resources.Load<AudioClip>(aud_manager.selectionNegative);
                    }
                    aud.Play();
                }
                else
                {
                    if (currentintelligencePage != 1)
                    {
                        currentintelligencePage--;
                        switchintelligencePage();
                        aud.clip = Resources.Load<AudioClip>(aud_manager.pageTurn);
                        aud.Play();
                    }
                    else
                    {
                        aud.clip = Resources.Load<AudioClip>(aud_manager.negativeSmall);
                        aud.Play();
                    }
                }

            }


            if (Input.GetKeyDown(KeyCode.C))
            {
                if (intelligenceCollection[currentSelectedIndex].Unlocked)
                {
                    if (!isReadingintelligence)
                    {
                        aud.clip = Resources.Load<AudioClip>(aud_manager.electrical);
                        aud.Play();
                        startManagement();
                        showIntelligence();
                        switchintelligencePage();
                    }
                    else
                    {
                        aud.clip = Resources.Load<AudioClip>(aud_manager.negativeSmall);
                        aud.Play();
                    }
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.negativeSmall);
                    aud.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                if (isReadingintelligence)
                {
                    stopManagement();
                    aud.clip = Resources.Load<AudioClip>(aud_manager.electricalExit);
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
        for (int i = 0; i < txtContent.Length; i++)
        {
            if (txtContent[i].IndexOf("[") != -1 && txtContent[i].IndexOf("[END]") == -1 && txtContent[i].IndexOf("[PAUSE]") == -1)
            {
                CIntelligence d = new CIntelligence();
                StringBuilder sb = new StringBuilder();

                //flagName: [NAME] => NAME
                string flagName = txtContent[i].Substring(1, txtContent[i].IndexOf("]") - 1);
                d.Key = flagName.Replace("\r", "").Replace("\n", "");//FileName: [INT1] => INT1
                d.Unlocked = ODMObject.event_manager.GetComponent<eventCenter>().getFlagBool(flagName);
                d.Name = txtContent[i].Replace("[" + flagName + "]", "");//FileName: [intelligence02] => intelligence02
                i++;

                for (int k = i; k < txtContent.Length; k++)
                {
                    if (txtContent[k].ToString().IndexOf("[END]") != -1)
                    {
                        d.Content.Add(sb.ToString());
                        i = k;
                        intelligenceCollection.Add(d);
                        break;
                    }
                    else if (txtContent[k].ToString().IndexOf("[PAUSE]") != -1)
                    {
                        d.Content.Add(sb.ToString());
                        sb = new StringBuilder();
                    }
                    else
                    {
                        sb.AppendLine(txtContent[k]);
                    }
                }
            }
        }

        for (int i = 0; i < slotCount; i++)
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
        currentSelectedIndex++;
        setSelection();
        setCover();
    }
    private void previousintelligence()
    {
        currentSelectedIndex--;
        setSelection();
        setCover();
    }



    private void nextListPage()
    {
        currentListPage++;
        currentSelectedIndex += slotCount;
        setSelection();
        setCover();
        updateListPageText();
    }
    private void previousListPage()
    {
        currentListPage--;
        currentSelectedIndex -= slotCount;
        setSelection();
        setCover();
        updateListPageText();
    }
    private void stopManagement()
    {
        isReadingintelligence = false;
        txtIntelligencePage.GetComponent<CanvasGroup>().alpha = 0;
        Intelligence_Left_Arrow.GetComponent<CanvasGroup>().alpha = 0;
        Intelligence_Right_Arrow.GetComponent<CanvasGroup>().alpha = 0;
        Intelligence_Title_in_Sheet.GetComponent<CanvasGroup>().alpha = 1;
        Read_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 1;
        Intelligence_Context_Area_Text.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void startManagement()
    {
        isReadingintelligence = true;
        Intelligence_Title_in_Sheet.GetComponent<CanvasGroup>().alpha = 0;
        Read_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 0;
        Intelligence_Context_Area_Text.GetComponent<CanvasGroup>().alpha = 1;
        txtIntelligencePage.GetComponent<CanvasGroup>().alpha = 1;
    }


    private void setCover()
    {
        if (currentSelectedIndex != -1)
        {
            refreshUnlocked();
            CIntelligence d = intelligenceCollection[currentSelectedIndex];
            if (d.Unlocked)
            {
                Select_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 1;
                Intelligence_Title_in_Sheet.GetComponent<CanvasGroup>().alpha = 1;
                Read_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 1;
                Intelligence_Title_in_Sheet.GetComponent<Text>().text = d.Name;
                Intelligence_Context_Area.GetComponent<Animator>().SetBool("isOpen", true);
            }
            else
            {
                Select_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 1;
                Intelligence_Title_in_Sheet.GetComponent<CanvasGroup>().alpha = 0;
                Read_Intelligence_Hint.GetComponent<CanvasGroup>().alpha = 0;
                Intelligence_Context_Area.GetComponent<Animator>().SetBool("isOpen", false);
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
            Intelligence_Context_Area.GetComponent<Animator>().SetBool("isOpen", false);
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
        maxListPage = intelligenceCollection.Count / slotCount;
        if (intelligenceCollection.Count % slotCount != 0)
        {
            maxListPage++;
        }
        Text_Intelligence_Page_Count.GetComponent<Text>().text = currentListPage + "/" + maxListPage;
    }

    private void setSelection()
    {
        for (int i = 0; i < buttonCollection.Count; i++)
        {
            CIntelligence d = intelligenceCollection[(currentListPage - 1) * slotCount + i];
            if (d.Unlocked)
                txtintelligenceTagCollection[i].GetComponent<Text>().text = d.Name;
            else
                txtintelligenceTagCollection[i].GetComponent<Text>().text = "NO DATA";

            if (getSlotIndex() == i)//selected
            {
                buttonCollection[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(img_itemSelected);
                buttonCollection[i].GetComponent<Image>().color = CSelectedColor;//is selected
            }
            else if (d.Unlocked)//locked
            {
                buttonCollection[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(img_itemUnselected);
                buttonCollection[i].GetComponent<Image>().color = CUnselectedColor; // unlocked
            }
            else
            {
                buttonCollection[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(img_itemUnselected);
                buttonCollection[i].GetComponent<Image>().color = CBlack; // locked
            }
        }
    }

    private void showIntelligence()
    {
        //Show intelligence text
        CIntelligence d = intelligenceCollection[currentSelectedIndex];
        maxintelligencePage = d.Content.Count;
        currentintelligencePage = 1;
        setDocuemntPageText();
        if (maxintelligencePage > 1)
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
        CIntelligence d = intelligenceCollection[currentSelectedIndex];
        Intelligence_Context_Area_Text.GetComponent<Text>().text = d.Content[currentintelligencePage - 1];

        if (currentintelligencePage == d.Content.Count)//final page
        {
            Intelligence_Left_Arrow.GetComponent<CanvasGroup>().alpha = 1;
            Intelligence_Right_Arrow.GetComponent<CanvasGroup>().alpha = 0;
        }
        else if (currentintelligencePage == 1)
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
        if (currentintelligencePage == -1)
        {
            txtIntelligencePage.GetComponent<Text>().text = "";
        }
        else
        {
            txtIntelligencePage.GetComponent<Text>().text = currentintelligencePage + "/" + maxintelligencePage;
        }
    }

    private int getSlotIndex()
    {
        int idx = -1;
        if (currentSelectedIndex != -1)
            idx = currentSelectedIndex % slotCount;
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
        aud.clip = Resources.Load<AudioClip>(aud_manager.electrical);
        aud.Play();
        intelligenceSelectSwitch = true;
        preSetting();
    }

    public void closePanel()
    {
        aud.clip = Resources.Load<AudioClip>(aud_manager.electricalExit);
        aud.Play();
        GetComponent<menuManager>().tabSwitch = true;
        intelligenceSelectSwitch = false;
        stateControl = false;
        setListFirstPage();
        currentSelectedIndex = -1;
        stopManagement();
        preSetting();
    }

    private void setListFirstPage()
    {
        currentListPage = 1;
        updateListPageText();
    }

    public CIntelligence getIntelligence(string _intelligenceFlagName)
    {
        CIntelligence intel = intelligenceCollection.Where(x => x.Key.Equals(_intelligenceFlagName)).FirstOrDefault();
        if (intel == null)
            ODM.errorLog(transform.name,
                "getIntelligence Error. Key: " + _intelligenceFlagName,
                "");
        return intel;
    }
    public class CIntelligence
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public List<string> Content = new List<string>();
        public bool Unlocked { get; set; }
    }

}