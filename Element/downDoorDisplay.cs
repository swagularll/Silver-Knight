using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.UI;


public class downDoorDisplay : MonoBehaviour
{

    public CMap mainMap;
    public CMap downMap;
    private eventCenter event_center;
    
    void Start()
    {
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();
    }

    public void checkVisibility()
    {
        if (mainMap.downDoor != null)
        {
            if (event_center.getFlagBool(ODMVariable.convert.getAreaFlag(mainMap.name)))
                GetComponent<CanvasGroup>().alpha = 1f;
            else if (downMap != null)
            {
                if (event_center.getFlagBool(ODMVariable.convert.getAreaFlag(downMap.name)))
                    GetComponent<CanvasGroup>().alpha = 1f;
                else
                    GetComponent<CanvasGroup>().alpha = 0f;
            }
            else
                GetComponent<CanvasGroup>().alpha = 0f;
        }
        else
            GetComponent<CanvasGroup>().alpha = 0f;

    }

    public void setColor()
    {
        if (mainMap.downDoor != null)
        {
            string doorFlag = ODMVariable.convert.getDownDoorFlag(mainMap.name);
            if (event_center.getFlagBool(doorFlag))
                GetComponent<Image>().color = ODMVariable.color.green_accessable_door;
            else
                GetComponent<Image>().color = ODMVariable.color.red_blocked_door;
        }
    }
}
