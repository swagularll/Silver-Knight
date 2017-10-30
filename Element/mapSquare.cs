using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HutongGames.PlayMaker;
using System;

public class mapSquare : MonoBehaviour
{
    public CMap selfRef;
    private CMap leftMap;
    private CMap upMap;
    private CDoor leftDoor;
    private CDoor upDoor;

    //public string flag = "";
    public bool state = false;
    public GameObject lbl_right_door;
    public GameObject lbl_down_door;

    //Conclusion information for request
    private string mapSquareInformation;
    private string mapSquareNoInformation;

    private string updoorInfo;
    private string downDoorInfo;
    private string leftDoorInfo;
    private string rightDoorInfo;



    //For translatoin result
    private string txt_selectedLocation;
    private string txt_rightDoor;
    private string txt_leftDoor;
    private string txt_upDoor;
    private string txt_downDoor;
    private string txt_Unexplored;
    private string txt_not_explored;
    private string txt_accessible;

    private GameObject eventManager;

    private Color32 CInvisible = new Color32(0, 0, 0, 0);
    private Color32 CRed = new Color32(212, 8, 8, 150);
    private Color32 CGreen = new Color32(73, 219, 61, 150);

    private Animator ani;
    void Start()
    {
        eventManager = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value;
        UiInfomationHolder uiInfo = FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value.GetComponent<UiInfomationHolder>();

        txt_selectedLocation = uiInfo.getText("Map Info selectedLocation");
        txt_rightDoor = uiInfo.getText("Map Info rightDoor");
        txt_leftDoor = uiInfo.getText("Map Info leftDoor");
        txt_upDoor = uiInfo.getText("Map Info upDoor");
        txt_downDoor = uiInfo.getText("Map Info downDoor");
        txt_Unexplored = uiInfo.getText("Map Info unexplored");
        txt_not_explored = uiInfo.getText("Map Info not explored");

        txt_accessible = uiInfo.getText("accessible");


        generateInfo();
        state = eventManager.GetComponent<eventCenter>().getFlagBool("Area" + selfRef.name);
        ani = GetComponent<Animator>();
    }

    public void setColor()
    {
        ani.SetBool("isExplored", state);
        ani.SetBool("isSelected", false);
        ani.SetBool("isCurrent", false);
    }

    public void setSelected()
    {
        ani.SetBool("isSelected", true);
    }

    public void setCurrent()
    {
        ani.SetBool("isCurrent", true);
    }

    private void generateInfo()
    {

        int currentX = System.Int32.Parse(selfRef.name.Substring(1, 1));
        int currentY = 0;
        char[] s = { 'A', 'B', 'C', 'D', 'E', 'F' };

        for (int i = 0; i < 6; i++)
        {
            if (s[i].Equals(selfRef.name[0]))
            {
                currentY = i;
                break;
            }
        }

        //No information text
        mapSquareNoInformation = txt_selectedLocation + selfRef.name + Environment.NewLine + txt_not_explored;

        //Start generating information
        mapSquareInformation = txt_selectedLocation + selfRef.name + " - " + selfRef.title + Environment.NewLine;
        mapSquareInformation += selfRef.description + Environment.NewLine + Environment.NewLine;

        lbl_right_door.GetComponent<rightDoorDisplay>().mainMap = selfRef;
        lbl_down_door.GetComponent<downDoorDisplay>().mainMap = selfRef;

        if (currentX != eventManager.GetComponent<mapDash>().X_limit)
        {
            CMap rightMap = eventManager.GetComponent<mapDash>().db.getMap(currentX + 1, currentY);
            lbl_right_door.GetComponent<rightDoorDisplay>().rightMap = rightMap;
        }

        if (currentY != eventManager.GetComponent<mapDash>().Y_limit)
        {
            CMap downMap = eventManager.GetComponent<mapDash>().db.getMap(currentX, currentY + 1);
            lbl_down_door.GetComponent<downDoorDisplay>().downMap = downMap;
        }

        if (currentX != 0)//Left door setting
        {
            leftMap = eventManager.GetComponent<mapDash>().db.getMap(currentX - 1, currentY);
            if (leftMap.rightDoor != null)
            {
                leftDoor = leftMap.rightDoor;
                string leftDoorFlag = leftMap.name + " Right Door";
                if (!eventManager.GetComponent<eventCenter>().getFlagBool(leftDoorFlag))
                    leftDoorInfo = txt_leftDoor + leftMap.rightDoor.Hint + Environment.NewLine + Environment.NewLine;
                else
                    leftDoorInfo = txt_leftDoor + txt_accessible + Environment.NewLine + Environment.NewLine;
            }
        }
        if (currentY != 0)
        {
            upMap = eventManager.GetComponent<mapDash>().db.getMap(currentX, currentY - 1);
            if (upMap.downDoor != null)
            {
                upDoor = upMap.downDoor;
                string upDoorFlag = upMap.name + " Down Door";
                if (!eventManager.GetComponent<eventCenter>().getFlagBool(upDoorFlag))
                    updoorInfo = txt_upDoor + upMap.downDoor.Hint + Environment.NewLine + Environment.NewLine;
                else
                    updoorInfo = txt_upDoor + txt_accessible + Environment.NewLine + Environment.NewLine;
            }
        }

        if (selfRef.rightDoor != null)
        {
            string rightDoorFlag = selfRef.name + " Right Door";
            if (!eventManager.GetComponent<eventCenter>().getFlagBool(rightDoorFlag))
                rightDoorInfo = txt_rightDoor + selfRef.rightDoor.Hint + Environment.NewLine + Environment.NewLine;
            else
                rightDoorInfo = txt_rightDoor + txt_accessible + Environment.NewLine + Environment.NewLine;
        }


        if (selfRef.downDoor != null)
        {
            string downDoorFlag = selfRef.name + " Down Door";
            if (!eventManager.GetComponent<eventCenter>().getFlagBool(downDoorFlag))
                downDoorInfo = txt_downDoor + selfRef.downDoor.Hint + Environment.NewLine + Environment.NewLine;
            else
                downDoorInfo = txt_downDoor + txt_accessible + Environment.NewLine + Environment.NewLine;
        }
    }

    public void renewDoorStatus()
    {
        lbl_right_door.GetComponent<rightDoorDisplay>().setColor();
        lbl_down_door.GetComponent<downDoorDisplay>().setColor();
        lbl_right_door.GetComponent<rightDoorDisplay>().checkVisibility();
        lbl_down_door.GetComponent<downDoorDisplay>().checkVisibility();
    }

    public string showInfo()
    {
        generateInfo();
        string replyText = mapSquareInformation + updoorInfo + downDoorInfo + leftDoorInfo + rightDoorInfo;
        if (state)
            return replyText;
        else
            return mapSquareNoInformation;
    }
}
