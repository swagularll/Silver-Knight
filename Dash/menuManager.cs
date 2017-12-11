using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class menuManager : MonoBehaviour
{
    public List<GameObject> tab_collection;
    public GameObject background_panel;
    public GameObject tab_panel;

    public bool isOpen = false; // Menu status to UI or not
    public bool tabSwitch = false;

    private GameObject current_activate_tab;
    private int current_selected_index = 0;
    private Menu background_panel_menu;
    private Menu tab_panel_menu;
    private AudioSource aud;

    void Start()
    {
        //buttonCollection.Add(obj_button_inventory.GetComponent<Image>());
        //buttonCollection.Add(obj_button_map.GetComponent<Image>());
        //buttonCollection.Add(obj_button_document.GetComponent<Image>());
        //buttonCollection.Add(obj_button_intelligence.GetComponent<Image>());

        background_panel_menu = background_panel.GetComponent<Menu>();
        tab_panel_menu = tab_panel.GetComponent<Menu>();

        current_activate_tab = tab_collection[current_selected_index];
        aud = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (tabSwitch && isOpen && !ODMVariable.is_system_locked) // switching between menu
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && current_selected_index < tab_collection.Count - 1)
            {
                setTab(tab_collection[++current_selected_index]);
                aud.clip = Resources.Load<AudioClip>(audioResource.typer_switch);
                aud.Play();

            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && current_selected_index > 0)
            {
                setTab(tab_collection[--current_selected_index]);
                aud.clip = Resources.Load<AudioClip>(audioResource.typer_switch);
                aud.Play();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))//enable middle
            {
                enableMiddleTab();
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!ODMVariable.is_on_event &&
               !ODMVariable.is_fighting &&
               !ODMVariable.is_system_locked)
            {
                openMenu();
            }
        }
    }

    public void openMenu()
    {
        if (!isOpen)
        {
            //renew code
            startMenu();
            ODMVariable.fsm.scene_controller.SendEvent(eventName.start_menu_hold);
            isOpen = tabSwitch = true;
            ODMObject.player_condition_panel.GetComponent<Menu>().isOpen = false;
        }
        else
        {
            closeMenu();
            ODMVariable.fsm.scene_controller.SendEvent(eventName.end_menu_hold);
            isOpen = tabSwitch = false;
            ODMObject.player_condition_panel.GetComponent<Menu>().isOpen = true;

        }
    }

    private void enableMiddleTab()
    {
        tabSwitch = false;
        current_activate_tab.GetComponent<tabController>().isActivate = true;

        switch (current_activate_tab.name)
        {
            case "Button Inventory":
                GetComponent<inventoryDash>().openPanel();//calling a method from other script, should not open until down arrow pressed
                break;
            case "Button Map":
                GetComponent<mapDash>().openPanel();
                break;
            case "Button Document":
                GetComponent<documentDash>().openPanel();
                break;
            case "Button Intelligence":
                GetComponent<intelligenceDash>().openPanel();
                break;
        }
    }
    public void disableMiddleTab()
    {
        tabSwitch = true;
        current_activate_tab.GetComponent<tabController>().isActivate = false;
    }

    public void setTab(GameObject btn)
    {
        current_activate_tab.GetComponent<tabController>().hidePanel();
        current_activate_tab = btn;
        current_activate_tab.GetComponent<tabController>().showPanel();
    }


    public void startMenu()
    {
        current_selected_index = 0;
        setTab(tab_collection[current_selected_index]);

        aud.clip = Resources.Load<AudioClip>(audioResource.electrical);
        aud.Play();
        current_activate_tab.GetComponent<tabController>().showPanel();
        tab_panel_menu.isOpen = background_panel_menu.isOpen = true;
        setTab(current_activate_tab);
        ///Loading Data for each panel
        GetComponent<mapDash>().preSetting();
        GetComponent<documentDash>().preSetting();
        GetComponent<intelligenceDash>().preSetting();
        GetComponent<inventoryDash>().refreshItemStatus();
    }

    public void closeMenu()
    {
        aud.clip = Resources.Load<AudioClip>(audioResource.electrical);
        aud.Play();
        switch (current_activate_tab.name)
        {
            case "Button Inventory":
                GetComponent<inventoryDash>().closePanel();
                break;
            case "Button Map":
                GetComponent<mapDash>().closePanel();
                break;
            case "Button Document":
                GetComponent<documentDash>().closePanel();
                break;
            case "Button Intelligence":
                GetComponent<intelligenceDash>().closePanel();
                break;
        }
        current_activate_tab.GetComponent<tabController>().hidePanel();
        tab_panel_menu.isOpen = background_panel_menu.isOpen = false;
    }
}
