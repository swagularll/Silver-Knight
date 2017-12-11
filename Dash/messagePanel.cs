using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class messagePanel : MonoBehaviour
{
    //renew code
    public GameObject txtMessage;
    public GameObject buttonCnfoirmText;

    private GameObject sender;
    private string sender_reply_method_name;
    private AudioSource aud;
    private PlayMakerFSM fsm;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        fsm = fsmHelper.getFsm(transform.gameObject, ODMVariable.common.default_fsm);
    }

    public void showMessage(string _msg_translation_key)
    {
        sender_reply_method_name = null;
        txtMessage.GetComponent<Text>().text = dataWidget.getTranslaton(_msg_translation_key);
        fsm.SendEvent(eventName.show_message);
    }

    public void quitPanel()
    {
        fsm.SendEvent(eventName.hide_message);
    }
}
