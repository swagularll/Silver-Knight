using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using Assets.Script.ODM_Widget;

public class menuManager : MonoBehaviour
{

    public Menu topCanvas;
    public Menu leftCanvas;
    public Menu rightCanvas;
    public Menu background;

    public bool isOpen = false; // switched to UI or not
    public bool tabSwitch = false;

    private Image currentButton;
    private List<Image> buttonCollection;
    private int buttonIndex = 0;
    private AudioSource aud;
    private audioManager aud_manager;

    void Start()
    {
        buttonCollection = new List<Image>();
        aud_manager = new audioManager();

        buttonCollection.Add(GameObject.Find("Button Inventory").GetComponent<Image>());
        buttonCollection.Add(GameObject.Find("Button Map").GetComponent<Image>());
        buttonCollection.Add(GameObject.Find("Button Document").GetComponent<Image>());
        buttonCollection.Add(GameObject.Find("Button Intelligence").GetComponent<Image>());
        //buttonCollection.Add(GameObject.Find("Button System").GetComponent<Image>());
        currentButton = buttonCollection[buttonIndex];
        aud = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (tabSwitch && isOpen) // switching between menu
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && buttonIndex < buttonCollection.Count - 1)
            {
                setTab(buttonCollection[++buttonIndex]);
                aud.clip = Resources.Load<AudioClip>(aud_manager.typer);
                aud.Play();

            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && buttonIndex > 0)
            {
                setTab(buttonCollection[--buttonIndex]);
                aud.clip = Resources.Load<AudioClip>(aud_manager.typer);
                aud.Play();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))//enable middle
            {
                enableMiddleTab();
                //GetComponent<Inventory>().currentSelectedIndex = 0;
                //GetComponent<Inventory>().inventoryTabSwitch = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!FsmVariables.GlobalVariables.GetFsmBool("isOnEvent").Value &&
               !FsmVariables.GlobalVariables.GetFsmBool("isFighting").Value)
            {
                openMenu();
            }
        }
    }

    public void openMenu()
    {
        if (!isOpen)
        {
            startMenu();
            fsmHelper.getFsm(ODMObject.event_manager, ODMVariable.fsm_scene_controller).SendEvent("start menu hold");
            isOpen = tabSwitch = true;
            GameObject.Find("Player Condition Panel").GetComponent<Menu>().isOpen = false;
        }
        else
        {
            closeMenu();
            fsmHelper.getFsm(ODMObject.event_manager, ODMVariable.fsm_scene_controller).SendEvent("end menu hold");
            isOpen = tabSwitch = false;
            GameObject.Find("Player Condition Panel").GetComponent<Menu>().isOpen = true;

        }
    }

    void enableMiddleTab()
    {
        tabSwitch = false;

        switch (currentButton.name)
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
                //case "Button System":
                //    showLeft(GameObject.Find("System Left Panel").GetComponent<Menu>());
                //    showRight(GameObject.Find("System Right Panel").GetComponent<Menu>());
                //    break;
        }
    }

    public void setTab(Image btn)
    {
        currentButton.GetComponent<Animator>().SetBool("isSelected", false);
        currentButton = btn;
        currentButton.GetComponent<Animator>().SetBool("isSelected", true);
        //customlized is possible, but this way is easy to close current UI

        switch (currentButton.name)
        {
            case "Button Inventory":
                showLeft(GameObject.Find("Inventory Left Panel").GetComponent<Menu>());
                showRight(GameObject.Find("Inventory Right Panel").GetComponent<Menu>());
                break;
            case "Button Map":
                showLeft(GameObject.Find("Map Left Panel").GetComponent<Menu>());
                showRight(GameObject.Find("Map Right Panel").GetComponent<Menu>());
                GetComponent<mapDash>().preSetting();
                break;
            case "Button Document":
                showLeft(GameObject.Find("Document Left Panel").GetComponent<Menu>());
                showRight(GameObject.Find("Document Right Panel").GetComponent<Menu>());
                break;
            case "Button Intelligence":
                showLeft(GameObject.Find("Intelligence Left Panel").GetComponent<Menu>());
                showRight(GameObject.Find("Intelligence Right Panel").GetComponent<Menu>());
                break;
                //case "Button System":
                //    showLeft(GameObject.Find("System Left Panel").GetComponent<Menu>());
                //    showRight(GameObject.Find("System Right Panel").GetComponent<Menu>());
                //    break;
        }
    }


    public void startMenu()
    {
        buttonIndex = 0;
        setTab(buttonCollection[buttonIndex]);

        aud.clip = Resources.Load<AudioClip>(aud_manager.electrical);
        aud.Play();
        topCanvas.isOpen = leftCanvas.isOpen = rightCanvas.isOpen = background.isOpen = true;
        setTab(currentButton);
        ///Loading Data for each panel
        GetComponent<mapDash>().preSetting();
        GetComponent<documentDash>().preSetting();
        GetComponent<intelligenceDash>().preSetting();
        GetComponent<inventoryDash>().refreshItemStatus();
    }

    public void closeMenu()
    {
        aud.clip = Resources.Load<AudioClip>(aud_manager.electrical);
        aud.Play();
        switch (currentButton.name)
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
                //case "Button System":
                //    break;
        }
        topCanvas.isOpen = leftCanvas.isOpen = rightCanvas.isOpen = background.isOpen = false;
    }

    public void showTop(Menu menu)
    {
        if (menu != null)
        {
            menu.isOpen = false;
        }
        topCanvas = menu;
        topCanvas.isOpen = true;
    }

    public void showLeft(Menu menu)
    {
        if (leftCanvas != null)
        {
            leftCanvas.isOpen = false;
        }
        leftCanvas = menu;
        leftCanvas.isOpen = true;
    }

    public void showRight(Menu menu)
    {
        if (rightCanvas != null)
        {
            rightCanvas.isOpen = false;
        }
        rightCanvas = menu;
        rightCanvas.isOpen = true;
    }
}
