using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class confirmPanel : MonoBehaviour
{
    public GameObject txt_left;
    public GameObject txt_right;
    public GameObject txt_message;

    private AudioSource aud;

    private GameObject sender;
    private string sender_reply_method_name;
    private PlayMakerFSM fsm_self;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        fsm_self = fsmHelper.getFsm(transform.gameObject, ODMVariable.common.default_fsm);
    }
    public void showConfirmation(GameObject _sender, string _senderReplyMethodName, string msgText)
    {
        txt_left.GetComponent<Text>().text = dataWidget.getTranslaton(ODMVariable.translation.cancel); //default
        txt_right.GetComponent<Text>().text = dataWidget.getTranslaton(ODMVariable.translation.confirm);//default
        txt_message.GetComponent<Text>().text = msgText;

        sender = _sender;
        sender_reply_method_name = _senderReplyMethodName;
        fsm_self.SendEvent(eventName.show_menu);
    }
    public void showConfirmation(GameObject _sender, string _sender_reply_method_name,
        string _left_button_text_key, string _right_button_text_key, string msg_context)
    {
        //Default buttons: Left-No, Right-Yes
        //Default translation
        txt_left.GetComponent<Text>().text = dataWidget.getTranslaton(_left_button_text_key);
        txt_right.GetComponent<Text>().text = dataWidget.getTranslaton(_right_button_text_key);
        txt_message.GetComponent<Text>().text = msg_context;

        sender = _sender;
        sender_reply_method_name = _sender_reply_method_name;
        fsm_self.SendEvent(eventName.show_menu);
    }
    public void cancelConfirmation()
    {
        fsm_self.FsmVariables.GetFsmString(ODMVariable.local.response).Value = null;//failed to answer
        fsm_self.SendEvent(eventName.hide_menu);
        replyConfirmation();
    }
    public void quitPanel()
    {
        fsm_self.SendEvent(eventName.hide_menu);
    }
    public void replyConfirmation()
    {
        sender.SendMessage(sender_reply_method_name, fsm_self.FsmVariables.GetFsmBool(ODMVariable.local.response).Value);
    }
}
