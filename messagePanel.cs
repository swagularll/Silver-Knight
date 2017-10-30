using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class messagePanel : MonoBehaviour
{

    public GameObject txtMessage;
    public GameObject buttonCnfoirmText;

    private GameObject sender;
    private string senderReplyMethodName;
    private AudioSource aud;

    private PlayMakerFSM messagePanelFSM;


    void Start()
    {
        aud = GetComponent<AudioSource>();
        messagePanelFSM = fsmHelper.getFsm(transform.gameObject, "FSM");
    }

    public void showMessage(string msgText)
    {
        senderReplyMethodName = null;
        txtMessage.GetComponent<Text>().text = (new ODM()).getTranslaton(msgText);
        messagePanelFSM.SendEvent("show message");
    }

    //public void endMessage()
    //{
    //    if (senderReplyMethodName != null)
    //        sender.SendMessage(senderReplyMethodName);
    //}

    public void quitPanel()
    {
        messagePanelFSM.SendEvent("hide message");
    }
}
