using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class confirmPanel : MonoBehaviour
{


    public GameObject txtLeft;
    public GameObject txtRight;
    public GameObject txtMessage;

    private ODM odm;
    private AudioSource aud;
    private audioManager soundHelper;

    private GameObject sender;
    private string senderReplyMethodName;
    private PlayMakerFSM confirmationFSM;
    
    void Start()
    {
        odm = new ODM();
        soundHelper = new audioManager();
        aud = GetComponent<AudioSource>();
        confirmationFSM =  fsmHelper.getFsm(transform.gameObject, "FSM");
    }
    public void showConfirmation(GameObject _sender, string _senderReplyMethodName,
        string msgText)
    {
        txtLeft.GetComponent<Text>().text = odm.getTranslaton("cancel"); //default
        txtRight.GetComponent<Text>().text = odm.getTranslaton("confirm");//default
        txtMessage.GetComponent<Text>().text = msgText;

        sender = _sender;
        senderReplyMethodName = _senderReplyMethodName;
        confirmationFSM.SendEvent("show confirmation");
    }
    public void showConfirmation(GameObject _sender, string _senderReplyMethodName,
        string _leftButtonKey, string _rightButtonKey, string msgText)//default: left-No, right-Yes
    {

        txtLeft.GetComponent<Text>().text = odm.getTranslaton(_leftButtonKey); //default
        txtRight.GetComponent<Text>().text = odm.getTranslaton(_rightButtonKey);//default
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
