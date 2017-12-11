using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class hpBarColorDisplay : MonoBehaviour
{
    public GameObject bar;
    public string monitorVariableName;
    public bool isChangeColor;

    private float full_width;
    private float full_height;



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
                bar.GetComponent<Image>().color = ODMVariable.color.hp_perfect;
            }
            else if (percentage_scale > 50)
            {
                bar.GetComponent<Image>().color = ODMVariable.color.hp_save;
            }
            else if (percentage_scale > 30)
            {
                bar.GetComponent<Image>().color = ODMVariable.color.hp_warning;
            }
            else
            {
                bar.GetComponent<Image>().color = ODMVariable.color.hp_dangeous;
            }
        }
    }
}
