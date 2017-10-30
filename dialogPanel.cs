using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HutongGames.PlayMaker;

public class dialogPanel : MonoBehaviour
{
    public string[] txtContent;
    public string[] txtExplicitedContent;
    public TextAsset txtSource;
    public int structureIndex = 0;
    private int currentReadLine = 0;
    public bool isOpen = false;
    private bool readControlSwitch = false;

    public GameObject conversationVisibilityPanel;
    public GameObject descriptionVisibilityPanel;
    public GameObject conversationPlayerLogoImage;
    public GameObject nameTagText;
    public GameObject conversationDisplayText;
    public GameObject descriptionDisplayText;
    public GameObject electricSound;

    private Image playerDispalyImage;
    private Text nameTag;
    private Text txtConversation;
    private Text txtDescription;

    private PlayMakerFSM currentMessageFSM;



    public string currentSlotItem;
    private string characterImagePath = "Character Image/";
    void Start()
    {
        string textContentPath = @"Data Collection\" + PlayerPrefs.GetString("lang") + @"\Conversation";
        txtSource = Resources.Load<TextAsset>(textContentPath);
        string _s = txtSource.text.Replace("\r", "");
        txtContent = _s.Split('\n');

        playerDispalyImage = conversationPlayerLogoImage.GetComponent<Image>();
        nameTag = nameTagText.GetComponent<Text>();
        txtConversation = conversationDisplayText.GetComponent<Text>();
        txtDescription = descriptionDisplayText.GetComponent<Text>();
    }

    public void readLine()
    {
        currentReadLine++;
        if (currentReadLine == txtExplicitedContent.Length)
        {
            exitEvent();
        }
        else
        {
            Instantiate(electricSound);
            readText();
        }
    }

    public void showMessage(string _flagName)
    {
        GetComponent<actionControl>().setIdle();
        fsmHelper.getFsm(transform.gameObject, "Scene Controller").SendEvent("start hold");
        FsmVariables.GlobalVariables.GetFsmBool("isOnEvent").Value = true;
        FsmVariables.GlobalVariables.GetFsmGameObject("obj_player_status_panel").Value.GetComponent<Animator>().SetBool("isOpen", false);

        Debug.Log(FsmVariables.GlobalVariables.GetFsmGameObject("obj_player_status_panel").Value);

        string flagName = _flagName.Replace("\r", "");
        string tagName = "[" + flagName + "]";

        for (int i = 0; i < txtContent.Length; i++)
        {
            if (txtContent[i].IndexOf(tagName) != -1)
            {
                isOpen = true;
                structureIndex = ++i;// this will only use once to track line from main context
                getEventContext();
                readText();//system will crash if context has 0 line
                break;
            }
        }
        if (!isOpen)
        {
            ODM.log(transform.name, 
                "Failed to find flag, flagName: " + flagName);
        }
        fsmHelper.getFsm(transform.gameObject, "Dialog").SendEvent("start dialog");
    }

    public void readText()
    {
        readControlSwitch = false;
        string[] data = txtExplicitedContent[currentReadLine].Split('\t');
        if (!string.IsNullOrEmpty(data[2].Trim()))//When there is a event going on...
        {
            FsmVariables.GlobalVariables.GetFsmBool("LockEvent").Value = true;
            FsmVariables.GlobalVariables.GetFsmGameObject("conversationPanel").Value.GetComponent<UISlider>().isOpen = false;
            GameObject currentEventObj = FsmVariables.GlobalVariables.GetFsmGameObject("currentEventObject").Value;
            string currentEventFsmName = FsmVariables.GlobalVariables.GetFsmString("currentEventFsmName").Value;
            currentMessageFSM = fsmHelper.getFsm(currentEventObj, currentEventFsmName);
            currentMessageFSM.SendEvent(data[2].Trim());
        }
        else
        {
            FsmVariables.GlobalVariables.GetFsmGameObject("conversationPanel").Value.GetComponent<UISlider>().isOpen = true;
            string line = data[3];
            if (line.IndexOf("[INPUT]") != -1)
                line = line.Replace("[INPUT]", currentSlotItem);

            if (string.IsNullOrEmpty(data[0].Trim()))// For description only
            {
                descriptionVisibilityPanel.GetComponent<CanvasGroup>().alpha = 1f;
                conversationVisibilityPanel.GetComponent<CanvasGroup>().alpha = 0f;
                txtDescription.GetComponent<Text>().text = line;
            }
            else // for Conversation
            {
                conversationVisibilityPanel.GetComponent<CanvasGroup>().alpha = 1f;
                descriptionVisibilityPanel.GetComponent<CanvasGroup>().alpha = 0f;

                playerDispalyImage.sprite = Resources.Load<Sprite>(characterImagePath + data[0]);
                nameTag.text = data[1];

                txtConversation.text = line;
                //XXX CODE: VOICE
            }
        }

        readControlSwitch = true;
    }
    #region Control
    public void afterActionReadline()
    {
        currentReadLine++;
        if (currentReadLine == txtExplicitedContent.Length)
        {
            exitEvent();
        }
        else
        {
            readText();
        }
    }
    private void exitEvent()
    {
        isOpen = false;
        FsmVariables.GlobalVariables.GetFsmGameObject("obj_player_status_panel").Value.GetComponent<Animator>().SetBool("isOpen", true);
        FsmVariables.GlobalVariables.GetFsmGameObject("conversationPanel").Value.GetComponent<UISlider>().isOpen = false;
        fsmHelper.getFsm(transform.gameObject, "Scene Controller").SendEvent("end hold");
        fsmHelper.getFsm(transform.gameObject, "Dialog").SendEvent("end dialog");
    }
    #endregion

    #region Data Manupilation
    public void getEventContext()
    {
        System.Collections.Generic.List<string> listString = new System.Collections.Generic.List<string>();
        for (int i = structureIndex; i < txtContent.Length; i++)
        {
            if (txtContent[i].IndexOf("[END]") != -1)
            {
                break;
            }

            else
            {
                listString.Add(txtContent[i]);
            }
        }//break will end here
        txtExplicitedContent = listString.ToArray();
        currentReadLine = 0;
    }
    IEnumerator WaitForSecond(float f)
    {
        yield return new WaitForSeconds(f);
    }
    #endregion
}
