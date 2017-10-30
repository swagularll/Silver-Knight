using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class rightDoorDisplay : MonoBehaviour
{

    public CMap mainMap;
    public CMap rightMap;
    private eventCenter eventCenter;

    private Color32 CGreen = new Color32(73, 219, 61, 255);
    private Color32 CRed = new Color32(212, 8, 8, 255);


    void Start()
    {
        eventCenter = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value.GetComponent<eventCenter>();
    }

    public void checkVisibility()
    {
        if (mainMap.rightDoor != null)
        {
            if (eventCenter.getFlagBool("Area" + mainMap.name))
                GetComponent<CanvasGroup>().alpha = 1f;
            else if (rightMap != null)
            {
                if (eventCenter.getFlagBool("Area" + rightMap.name))
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
            string doorFlag = mainMap.name + " Right Door";
            if (eventCenter.getFlagBool(doorFlag))
                GetComponent<Image>().color = CGreen;
            else
                GetComponent<Image>().color = CRed;
        }
    }
}
