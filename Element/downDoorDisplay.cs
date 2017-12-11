using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.UI;


public class downDoorDisplay : MonoBehaviour
{

    public CMap mainMap;
    public CMap downMap;
    private eventCenter eventCenter;

    private Color32 CGreen = new Color32(73, 219, 61, 255);
    private Color32 CRed = new Color32(212, 8, 8, 255);


    void Start()
    {
        eventCenter = ODMObject.event_manager.GetComponent<eventCenter>();
    }

    public void checkVisibility()
    {
        if (mainMap.downDoor != null)
        {
            if (eventCenter.getFlagBool(ODMVariable.convert.getAreaFlag(mainMap.name)))
                GetComponent<CanvasGroup>().alpha = 1f;
            else if (downMap != null)
            {
                if (eventCenter.getFlagBool(ODMVariable.convert.getAreaFlag(downMap.name)))
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
            if (eventCenter.getFlagBool(doorFlag))
                GetComponent<Image>().color = CGreen;
            else
                GetComponent<Image>().color = CRed;
        }
    }
}
