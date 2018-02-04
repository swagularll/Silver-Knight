using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.UI;


public class rightDoorDisplay : MonoBehaviour
{

    public CMap mainMap;
    public CMap rightMap;
    private eventCenter event_center;

    void Start()
    {
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();
    }

    public void checkVisibility()
    {
        if (mainMap.rightDoor != null)
        {
            if (event_center.getFlagBool(ODMVariable.convert.getAreaFlag(mainMap.name)))
                GetComponent<CanvasGroup>().alpha = 1f;
            else if (rightMap != null)
            {
                if (event_center.getFlagBool(ODMVariable.convert.getAreaFlag(rightMap.name)))
                    GetComponent<CanvasGroup>().alpha = 1f;
                else
                    GetComponent<CanvasGroup>().alpha = 0f;
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 0f;
            }
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 0f;
        }

    }

    public void setColor()
    {
        if (mainMap.rightDoor != null)
        {
            string doorFlag = ODMVariable.convert.getRightDoorFlag(mainMap.name);
            if (event_center.getFlagBool(doorFlag))
                GetComponent<Image>().color = ODMVariable.color.green_accessable_door;
            else
                GetComponent<Image>().color = ODMVariable.color.red_blocked_door;
        }
    }
}
