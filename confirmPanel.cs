using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Script.ODM_Widget;

public class confirmPanel : MonoBehaviour
{


    public GameObject txtLeft;
    public GameObject txtRight;
    public GameObject txtMessage;

    private AudioSource aud;

    private GameObject sender;
    private string senderReplyMethodName;
    private PlayMakerFSM confirmationFSM;
    
    void Start()
    {
        aud = GetComponent<AudioSource>();
        confirmationFSM =  fsmHelper.getFsm(transform.gameObject, "FSM");
    }
    public void showConfirmation(GameObject _sender, string _senderReplyMethodName,
        string msgText)
    {
        txtLeft.GetComponent<Text>().text = dataWidget.getTranslaton("cancel"); //default
        txtRight.GetComponent<Text>().text = dataWidget.getTranslaton("confirm");//default
        txtMessage.GetComponent<Text>().text = msgText;

        sender = _sender;
        senderReplyMethodName = _senderReplyMethodName;
        confirmationFSM.SendEvent("show confirmation");
    }
    public void showConfirmation(GameObject _sender, string _senderReplyMethodName,
        string _leftButtonKey, string _rightButtonKey, string msgText)//default: left-No, right-Yes
    {

        txtLeft.GetComponent<Text>().text = dataWidget.getTranslaton(_leftButtonKey); //default
        txtRight.GetComponent<Text>().text = dataWidget.getTranslaton(_rightButtonKey);//default
        txtMessage.GetComponent<Text>().text = msgText;

        sender = _sender;
        senderReplyMethodName = _senderReplyMethodName;
        confirmationFSM.SendEvent("show confirmation");
    }
    public void cancelConfirmation()
    {
        confirmationFSM.FsmVariables.GetFsmString("response").Value = null;//failed to answer
        confirmationFSM.SendEvent("hide confirmation");
        replyConfirmation();
    }
    public void quitPanel()
    {
        confirmationFSM.SendEvent("hide confirmation");
    }
    public void replyConfirmation()
    {
        sender.SendMessage(senderReplyMethodName, confirmationFSM.FsmVariables.GetFsmBool("response").Value);
    }
}
