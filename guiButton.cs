using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class guiButton : MonoBehaviour {

    public List<GameObject> text_colleciton;
    public Color32 text_selected_color;
    public Color32 button_selected_color;
    public Color32 text_unselected_color;
    public Color32 button_unselected_color;

    public Color32 text_unselectable_color;
    public Color32 button_unselectable_color;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setSelected(bool _selected)
    {
        if (_selected)
        {
            if (text_colleciton != null)
            {
                for (int i = 0; i < text_colleciton.Count; i++)
                {
                    text_colleciton[i].GetComponent<Text>().color = text_selected_color;
                }
                GetComponent<Image>().color = button_selected_color;
            }
        }
        else
        {
            if (text_colleciton != null)
            {
                for (int i = 0; i < text_colleciton.Count; i++)
                {
                    text_colleciton[i].GetComponent<Text>().color = text_unselected_color;
                }
                GetComponent<Image>().color = button_unselected_color;

            }
        }
    }

    public void setUnselectable()
    {
        if (text_colleciton != null)
        {
            for (int i = 0; i < text_colleciton.Count; i++)
            {
                text_colleciton[i].GetComponent<Text>().color = text_unselectable_color;
            }
            GetComponent<Image>().color = button_unselectable_color;
        }
    }
}
