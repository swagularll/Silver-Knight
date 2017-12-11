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
        eventManager = ODMObject.event_manager;
        UiInfomationHolder uiInfo = ODMObject.language_translator.GetComponent<UiInfomationHolder>();

        txt_selectedLocation = uiInfo.getText(ODMVariable.translation.map_info_selected_location);
        txt_rightDoor = uiInfo.getText(ODMVariable.translation.map_info_right_door);
        txt_leftDoor = uiInfo.getText(ODMVariable.translation.map_info_left_door);
        txt_upDoor = uiInfo.getText(ODMVariable.translation.map_info_up_door);
        txt_downDoor = uiInfo.getText(ODMVariable.translation.map_info_down_door);
        txt_Unexplored = uiInfo.getText(ODMVariable.translation.map_info_unexplored);
        txt_not_explored = uiInfo.getText(ODMVariable.translation.map_info_not_explored);

        txt_accessible = uiInfo.getText(ODMVariable.translation.accessible);


        generateInfo();
        state = eventManager.GetComponent<eventCenter>().getFlagBool(ODMVariable.convert.getAreaFlag(selfRef.name));
        ani = GetComponent<Animator>();
    }

    public void setColor()
    {
        ani.SetBool(ODMVariable.animation.is_explored, state);
        ani.SetBool(ODMVariable.animation.is_selected, false);
        ani.SetBool(ODMVariable.animation.is_current, false);
    }

    public void setSelected()
    {
        ani.SetBool(ODMVariable.animation.is_selected, true);
    }

    public void setCurrent()
    {
        ani.SetBool(ODMVariable.animation.is_current, true);
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

        if (currentX != eventManager.GetComponent<mapDash>().limit_x)
        {
            CMap rightMap = eventManager.GetComponent<mapDash>().db.getMap(currentX + 1, currentY);
            lbl_right_door.GetComponent<rightDoorDisplay>().rightMap = rightMap;
        }

        if (currentY != eventManager.GetComponent<mapDash>().limit_y)
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
                string leftDoorFlag = ODMVariable.convert.getRightDoorFlag(leftMap.name);
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
                string upDoorFlag = ODMVariable.convert.getDownDoorFlag(upMap.name);
                if (!eventManager.GetComponent<eventCenter>().getFlagBool(upDoorFlag))
                    updoorInfo = txt_upDoor + upMap.downDoor.Hint + Environment.NewLine + Environment.NewLine;
                else
                    updoorInfo = txt_upDoor + txt_accessible + Environment.NewLine + Environment.NewLine;
            }
        }

        if (selfRef.rightDoor != null)
        {
            string rightDoorFlag = ODMVariable.convert.getRightDoorFlag(selfRef.name);
            if (!eventManager.GetComponent<eventCenter>().getFlagBool(rightDoorFlag))
                rightDoorInfo = txt_rightDoor + selfRef.rightDoor.Hint + Environment.NewLine + Environment.NewLine;
            else
                rightDoorInfo = txt_rightDoor + txt_accessible + Environment.NewLine + Environment.NewLine;
        }


        if (selfRef.downDoor != null)
        {
            string downDoorFlag = ODMVariable.convert.getDownDoorFlag(selfRef.name);
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
