using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class barValueDisplay : MonoBehaviour
{
    public GameObject bar;
    public string monitorVariableName;
    public bool isChangeColor;

    private float full_width;
    private float full_height;

    Color32 cPerfect = new Color32(54, 255, 233, 255);
    Color32 cSave = new Color32(69, 255, 86, 255);
    Color32 cWarning = new Color32(255, 121, 49, 255);
    Color32 cDangeous = new Color32(255, 49, 49, 255);

    void Start()
    {
        //full_width = GetComponent<RectTransform>().rect.width;
        //full_width = GetComponent<RectTransform>().rect.width;

    }
    void Update()
    {
        float percentage_scale = FsmVariables.GlobalVariables.GetFsmFloat(monitorVariableName).Value / 100;
        if (percentage_scale <= 0)
            percentage_scale = 0;
        GetComponent<RectTransform>().localScale = new Vector3(percentage_scale, GetComponent<RectTransform>().localScale.y, GetComponent<RectTransform>().localScale.z); ;
        if (isChangeColor)
        {
            if (percentage_scale > 80)
            {
                bar.GetComponent<Image>().color = cPerfect;
            }
            else if (percentage_scale > 50)
            {
                bar.GetComponent<Image>().color = cSave;
            }
            else if (percentage_scale > 30)
            {
                bar.GetComponent<Image>().color = cWarning;
            }
            else
            {
                bar.GetComponent<Image>().color = cDangeous;
            }
        }
    }
}
