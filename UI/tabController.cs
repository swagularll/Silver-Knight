using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tabController : MonoBehaviour
{
    public List<GameObject> dash_collection;
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void showPanel()
    {
        foreach (GameObject dash in dash_collection)
        {
            dash.GetComponent<Menu>().isOpen = true;
        }
        isSelected = true;
    }

    public void hidePanel()
    {
        foreach (GameObject dash in dash_collection)
        {
            dash.GetComponent<Menu>().isOpen = false;
        }
        isSelected = false;
    }
    public bool isSelected
    {
        get { return ani.GetBool(ODMVariable.animation.is_selected); }
        set { ani.SetBool(ODMVariable.animation.is_selected, value); }
    }

    public bool isActivate
    {
        get { return ani.GetBool(ODMVariable.animation.is_activate); }
        set { ani.SetBool(ODMVariable.animation.is_activate, value); }
    }
}
