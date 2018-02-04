using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using System;
using System.Linq;
using HutongGames.PlayMaker;

public class documentDash : MonoBehaviour
{
    public bool documentSelectSwitch = false;//called by menuManager
    public TextAsset txtSource;
    public GameObject model_btnDocument;
    public GameObject model_txtDocumentName;
    public GameObject left_arror;
    public GameObject right_arror;


    public GameObject document_text_page_count;
    public GameObject document_context_area_text;
    public GameObject document_title_in_sheet;
    public GameObject document_context_area;
    public GameObject document_text_page;
    public GameObject document_hint_read;
    public GameObject document_hint_selection;

    private GameObject converText;
    

    //public bool DOCUMENT_LOCK = false;


    List<CDocument> documentCollection;
    List<GameObject> buttonCollection;
    List<GameObject> txtDocumentTagCollection;

    private GameObject soundPlayer;
    private AudioSource aud;

    private bool stateControl = false;
    private bool isReadingDocument = false;

    private string[] txtContent;
    private int slotCount = 6;
    //private int documentSinglePageSize = 10;

    private int currentSelectedIndex = -1;//start with 0
    private int currentDocumentPage = -1;//start with 1
    private int maxDocumentPage = -1;

    private int currentListPage = -1;
    private int maxListPage = -1;

    //private string img_itemSelected = "UI/[UI]Item Selection";
    //private string img_itemUnselected = "UI/[UI]Harf Transparent Side B Full";

    public Sprite img_item_selected;
    public Sprite img_item_unselected;


    private Color32 CSelectedColor = new Color32(255, 255, 255, 255);
    private Color32 CUnselectedColor = new Color32(45, 45, 45, 150);
    private Color32 CBlack = new Color32(0, 0, 0, 255);

    void Start()
    {
        documentCollection = new List<CDocument>();
        buttonCollection = new List<GameObject>();
        txtDocumentTagCollection = new List<GameObject>();
        //renew code

        txtSource = Resources.Load<TextAsset>(ODMVariable.path.document_resource);

        txtContent = txtSource.text.Split('\n');
        aud = GetComponent<AudioSource>();

        initializeDocumentList();
    }

    void Update()
    {
        if (documentSelectSwitch && !stateControl)//initialize and select the first document
        {
            stateControl = true;//for the first time loading
            nextDocument();
        }
        else if (documentSelectSwitch && stateControl && !ODMVariable.is_system_locked)//Document Select section
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (getSlotIndex() == slotCount - 1)//last document cannot go down
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                    aud.Play();
                }
                else
                {

                    if (isReadingDocument)
                        stopManagement();
                    nextDocument();
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
                    if (isReadingDocument)
                        stopManagement();
                    previousDocument();
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    aud.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!isReadingDocument)
                {
                    if (currentListPage < maxListPage)//List
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
                    if (currentDocumentPage < documentCollection[currentSelectedIndex].Content.Count)
                    {
                        currentDocumentPage++;
                        switchDocumentPage();
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
                if (!isReadingDocument)
                {
                    if (currentListPage != 1)//List
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
                    if (currentDocumentPage != 1)
                    {
                        currentDocumentPage--;
                        switchDocumentPage();
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
                if (documentCollection[currentSelectedIndex].Unlocked)
                {
                    if (!isReadingDocument)
                    {
                        aud.clip = Resources.Load<AudioClip>(audioResource.electrical);
                        aud.Play();
                        startManagement();
                        showDocument();
                        switchDocumentPage();
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
                if (isReadingDocument)
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
    private void initializeDocumentList()
    {
        var listContainer = GameObject.Find("Document List Button Container");

        for (int i = 0; i < txtContent.Length; i++)
        {
            if (txtContent[i].IndexOf("[") != -1 && txtContent[i].IndexOf("[END]") == -1 && txtContent[i].IndexOf("[PAUSE]") == -1)
            {
                CDocument d = new CDocument();
                StringBuilder sb = new StringBuilder();

                string flagName = txtContent[i].Substring(1, txtContent[i].IndexOf("]") - 1);//flagName: [NAME] => NAME
                d.Key = flagName.Replace("\r", "").Replace("\n", "");//FileName: [Document02] => Document02
                d.Unlocked = ODMObject.event_manager.GetComponent<eventCenter>().getFlagBool(flagName);
                d.Name = txtContent[i].Replace("[" + flagName + "]", "");//FileName: [Document02] => Document02
                i++;
                for (int k = i; k < txtContent.Length; k++)
                {
                    if (txtContent[k].ToString().IndexOf("[END]") != -1)
                    {
                        d.Content.Add(sb.ToString());
                        i = k;
                        documentCollection.Add(d);
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
            var b = Instantiate(model_btnDocument);
            buttonCollection.Add(b);
            var txtName = Instantiate(model_txtDocumentName);
            txtName.transform.SetParent(buttonCollection[i].transform);
            txtDocumentTagCollection.Add(txtName);
            buttonCollection[i].transform.SetParent(listContainer.transform);
        }
    }
    private void nextDocument()
    {
        currentSelectedIndex++;
        setSelection();
        setCover();
    }
    private void previousDocument()
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
        isReadingDocument = false;
        document_text_page.GetComponent<CanvasGroup>().alpha = 0;
        left_arror.GetComponent<CanvasGroup>().alpha = 0;
        right_arror.GetComponent<CanvasGroup>().alpha = 0;
        document_title_in_sheet.GetComponent<CanvasGroup>().alpha = 1;
        document_hint_read.GetComponent<CanvasGroup>().alpha = 1;
        document_context_area_text.GetComponent<CanvasGroup>().alpha = 0;
    }
    private void startManagement()
    {
        isReadingDocument = true;
        document_title_in_sheet.GetComponent<CanvasGroup>().alpha = 0;
        document_hint_read.GetComponent<CanvasGroup>().alpha = 0;
        document_context_area_text.GetComponent<CanvasGroup>().alpha = 1;
        document_text_page.GetComponent<CanvasGroup>().alpha = 1;
    }
    private void setCover()
    {
        if (currentSelectedIndex != -1)
        {
            refreshUnlocked();
            CDocument d = documentCollection[currentSelectedIndex];
            if (d.Unlocked)
            {
                document_hint_selection.GetComponent<CanvasGroup>().alpha = 1;

                document_title_in_sheet.GetComponent<CanvasGroup>().alpha = 1;
                document_hint_read.GetComponent<CanvasGroup>().alpha = 1;
                document_title_in_sheet.GetComponent<Text>().text = d.Name;
                document_context_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, true);
            }
            else
            {
                document_hint_selection.GetComponent<CanvasGroup>().alpha = 1;
                document_title_in_sheet.GetComponent<CanvasGroup>().alpha = 0;
                document_hint_read.GetComponent<CanvasGroup>().alpha = 0;
                document_context_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, false);
            }
        }
        else
        {
            document_hint_selection.GetComponent<CanvasGroup>().alpha = 0;

            left_arror.GetComponent<CanvasGroup>().alpha = 0;
            right_arror.GetComponent<CanvasGroup>().alpha = 0;
            document_text_page.GetComponent<CanvasGroup>().alpha = 0;
            document_title_in_sheet.GetComponent<CanvasGroup>().alpha = 0;
            document_hint_read.GetComponent<CanvasGroup>().alpha = 0;
            document_context_area_text.GetComponent<CanvasGroup>().alpha = 0;
            document_context_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, false);
        }
    }
    private void refreshUnlocked()
    {
        for (int i = 0; i < documentCollection.Count; i++)
        {
            documentCollection[i].Unlocked = ODMObject.event_manager.GetComponent<eventCenter>().getFlagBool(documentCollection[i].Key);
        }
    }
    private void updateListPageText()
    {
        maxListPage = documentCollection.Count / slotCount;
        if (documentCollection.Count % slotCount != 0)
        {
            maxListPage++;
        }
        document_text_page_count.GetComponent<Text>().text = currentListPage + "/" + maxListPage;
    }
    private void setSelection()
    {
        for (int i = 0; i < buttonCollection.Count; i++)
        {
            CDocument d = documentCollection[(currentListPage - 1) * slotCount + i];
            if (d.Unlocked)
                txtDocumentTagCollection[i].GetComponent<Text>().text = d.Name;
            else
                txtDocumentTagCollection[i].GetComponent<Text>().text = "NO DATA";

            if (getSlotIndex() == i)//selected
            {
                buttonCollection[i].GetComponent<Image>().sprite = img_item_selected; // Resources.Load<Sprite>(img_itemSelected);
                buttonCollection[i].GetComponent<Image>().color = CSelectedColor;//is selected
            }
            else if (d.Unlocked)//locked
            {
                buttonCollection[i].GetComponent<Image>().sprite = img_item_unselected;// Resources.Load<Sprite>(img_itemUnselected);
                buttonCollection[i].GetComponent<Image>().color = CUnselectedColor; // unlocked
            }
            else
            {
                buttonCollection[i].GetComponent<Image>().sprite = img_item_unselected;//Resources.Load<Sprite>(img_itemUnselected);
                buttonCollection[i].GetComponent<Image>().color = CBlack; // locked
            }
        }
    }

    private void showDocument()
    {
        //Show document text
        CDocument d = documentCollection[currentSelectedIndex];
        maxDocumentPage = d.Content.Count;
        currentDocumentPage = 1;
        setDocuemntPageText();
        if (maxDocumentPage > 1)
        {
            left_arror.GetComponent<CanvasGroup>().alpha = 0;
            right_arror.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            left_arror.GetComponent<CanvasGroup>().alpha = 0;
            right_arror.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
    private void switchDocumentPage()
    {
        CDocument d = documentCollection[currentSelectedIndex];
        document_context_area_text.GetComponent<Text>().text = d.Content[currentDocumentPage - 1];

        if (currentDocumentPage == d.Content.Count)//final page
        {
            left_arror.GetComponent<CanvasGroup>().alpha = 1;
            right_arror.GetComponent<CanvasGroup>().alpha = 0;
        }
        else if (currentDocumentPage == 1)
        {
            left_arror.GetComponent<CanvasGroup>().alpha = 0;
            right_arror.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            left_arror.GetComponent<CanvasGroup>().alpha = 1;
            right_arror.GetComponent<CanvasGroup>().alpha = 1;
        }
        setDocuemntPageText();
    }
    private void setDocuemntPageText()
    {
        if (currentDocumentPage == -1)
        {
            GameObject.Find("txtDocumentPage").GetComponent<Text>().text = "";
        }
        else
        {
            GameObject.Find("txtDocumentPage").GetComponent<Text>().text = currentDocumentPage + "/" + maxDocumentPage;
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
        aud.clip = Resources.Load<AudioClip>(audioResource.electrical);
        aud.Play();
        documentSelectSwitch = true;
        preSetting();
    }
    public void closePanel()
    {
        aud.clip = Resources.Load<AudioClip>(audioResource.electrical_out);
        aud.Play();
        GetComponent<menuManager>().disableMiddleTab();
        documentSelectSwitch = false;
        stateControl = false;
        currentSelectedIndex = -1;
        stopManagement();
        preSetting();
    }
    private void setListFirstPage()
    {
        currentListPage = 1;
        updateListPageText();
    }
    public CDocument getDocument(string _documentFlagName)
    {
        CDocument doc = documentCollection.Where(d => d.Key.Equals(_documentFlagName)).FirstOrDefault();
        if (doc == null)
            ODM.errorLog(transform.name, "getDocument Error. Key: " + _documentFlagName);
        return doc;
    }
}

#region LOCAL CLASS
public class CDocument
{
    public string Key { get; set; }
    public string Name { get; set; }
    public bool Unlocked { get; set; }
    public List<string> Content
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

    private List<string> content = new List<string>();
}
#endregion