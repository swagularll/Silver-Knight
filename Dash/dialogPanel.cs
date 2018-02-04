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


    private string fsm_dialog = "Dialog";


    public string currentSlotItem;
    void Start()
    {
        txtSource = Resources.Load<TextAsset>(ODMVariable.path.conversation_file_resource);
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
        ODMObject.character_ava.GetComponent<actionControl>().setIdle();
        ODMVariable.fsm.scene_controller.SendEvent(eventName.start_hold);
        ODMVariable.is_on_event = true;
        ODMObject.player_condition_panel.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, false);

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
            ODM.log(transform.name, "Failed to find flag, flagName: " + flagName);
        }
        fsmHelper.getFsm(transform.gameObject, fsm_dialog).SendEvent(eventName.start_function);
    }

    public void readText()
    {
        readControlSwitch = false;
        string[] data = txtExplicitedContent[currentReadLine].Split('\t');
        if (!string.IsNullOrEmpty(data[2].Trim()))//When there is a event going on...
        {
            //Renew code
            //FsmVariables.GlobalVariables.GetFsmBool("LockEvent").Value = true;
            ODMVariable.is_lock_event = true;
            ODMObject.conversation_panel.GetComponent<UISlider>().isOpen = false;
            currentMessageFSM = fsmHelper.getFsm(ODMObject.current_event_object, ODMVariable.current_event_fsm_name);
            currentMessageFSM.SendEvent(data[2].Trim());
        }
        else
        {
            ODMObject.conversation_panel.GetComponent<UISlider>().isOpen = true;
            string line = data[3];
            if (line.IndexOf(ODMVariable.tag.input) != -1)
                line = line.Replace(ODMVariable.tag.input, currentSlotItem);

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

                playerDispalyImage.sprite = Resources.Load<Sprite>(ODMVariable.path.character_image_directory + data[0]);
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
        ODMObject.player_condition_panel.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, true);
        ODMObject.conversation_panel.GetComponent<UISlider>().isOpen = false;
        ODMVariable.fsm.scene_controller.SendEvent(eventName.end_hold);
        fsmHelper.getFsm(transform.gameObject, fsm_dialog).SendEvent(eventName.end_function);
    }
    #endregion

    #region Data Manupilation
    public void getEventContext()
    {
        System.Collections.Generic.List<string> listString = new System.Collections.Generic.List<string>();
        for (int i = structureIndex; i < txtContent.Length; i++)
        {
            if (txtContent[i].IndexOf(ODMVariable.tag.end) != -1)
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
