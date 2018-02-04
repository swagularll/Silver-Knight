using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiHelper : MonoBehaviour
{
    public bool isMiddle = false;
    public bool isCanvasInvisible = false;
    public string linked_fsm = "FSM";
    public GameObject target_obj;
    void Start()
    {
        if (isMiddle)
        {
            setMiddle();
        }
        if (isCanvasInvisible)
        {
            hideGUI();
        }
    }
    public void hideGUI()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }
    public void showGUI()
    {
        GetComponent<CanvasGroup>().alpha = 1;
    }

    public void hideTargetGUI()
    {
        if (target_obj != null)
            target_obj.GetComponent<CanvasGroup>().alpha = 0;
        else
            ODM.errorLog(transform.name, "hideTargetGUI error: no target object.");
    }
    public void showTargetGUI()
    {
        if (target_obj != null)
            target_obj.GetComponent<CanvasGroup>().alpha = 1;
        else
            ODM.errorLog(transform.name, "hideTargetGUI error: no target object.");
    }

    public void updateText(float v)
    {
        gameObject.GetComponent<Text>().text = v.ToString();
    }

    public void updateText(int v)
    {
        gameObject.GetComponent<Text>().text = v.ToString();
    }
    //Must be for mate bar
    public void showMessageText(string _msg)
    {
        //XXX
        ODM.errorLog(transform.name, "guiHelper USELESS MESSAGE CALLED");
        //PlayMakerFSM fsm = fsmHelper.getFsm(transform.gameObject, linked_fsm);
        //fsm.FsmVariables.GetFsmString(ODMVariable.local.msg).Value = _msg;
        //fsm.SendEvent(eventName.show_message);
    }

    private void setMiddle()
    {
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);//set to the middle while start the game
    }

    public void setResistantColor()
    {
        GetComponent<Image>().color = ODMVariable.color.resistant_bar;
    }
    public void setFeedColor()
    {
        GetComponent<Image>().color = ODMVariable.color.feed_bar;
    }
}
