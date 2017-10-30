using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiHelper : MonoBehaviour
{
    public bool isMiddle = false;
    public bool isCanvasInvisible = false;
    public string linked_fsm = "FSM";
    public GameObject targetObj;
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
        if (targetObj != null)
            targetObj.GetComponent<CanvasGroup>().alpha = 0;
        else
            ODM.errorLog(transform.name, "hideTargetGUI error: no target object.", "");
    }
    public void showTargetGUI()
    {
        if (targetObj != null)
            targetObj.GetComponent<CanvasGroup>().alpha = 1;
        else
            ODM.errorLog(transform.name, "hideTargetGUI error: no target object.", "");
    }

    public void updateText(float v)
    {
        gameObject.GetComponent<Text>().text = v.ToString();
    }

    public void updateText(int v)
    {
        gameObject.GetComponent<Text>().text = v.ToString();
    }

    public void showMessageText(string msg)
    {
        PlayMakerFSM fsm = fsmHelper.getFsm(transform.gameObject, linked_fsm);
        fsm.FsmVariables.GetFsmString("msg").Value = msg;
        fsm.SendEvent("show message text");
    }

    private void setMiddle()
    {
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);//set to the middle while start the game
    }

    public void setResistantColor()
    {
        GetComponent<Image>().color = new Color32(126, 41, 187, 255);
    }
    public void setFeedColor()
    {
        GetComponent<Image>().color = new Color32(219, 42, 82, 255);
    }
}
