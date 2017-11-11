using UnityEngine;
using System.Collections;
using System;
using HutongGames.PlayMaker;
using Assets.Script.ODM_Widget;

public class mapSetting : MonoBehaviour
{
    public inventoryDash.inventoryItem requiredItem;
    public string successfulMsg;

    public bool isElectricalDoor = false;
    public bool allowAutoItem = true;

    public GameObject hint;

    private bool stateControl = false;

    private string flagName; //Will auto generate by current Level
    private string nextSceneName;

    private eventCenter eventCenter;
    private dialogPanel dialogue;
    private ItemDatabase itemDB;

    public AudioClip electricUnlocked;
    public AudioClip unlockSound;
    public AudioClip lockedSound;

    private AudioSource aud;

    private GameObject eventManager;

    void Start()
    {

        successfulMsg = "showOpenDoor";
        requiredItem = inventoryDash.inventoryItem.none;

        eventManager = ODMObject.event_manager;

        nextSceneName = setLocationString();
        flagName = identifyDoorFlagName();
        setDoorTexture();
        eventCenter = eventManager.GetComponent<eventCenter>();
        dialogue = eventManager.GetComponent<dialogPanel>();
        itemDB = eventManager.GetComponent<ItemDatabase>();
        aud = GetComponent<AudioSource>();

    }

    private void allowUseItem()
    {
        allowAutoItem = true;
    }

    private void tryLoad()
    {
        bool isOpen = eventCenter.getFlagBool(flagName);//default door rely on flag only
        if (isOpen)
        {
            eventCenter.setFlagTrue(flagName);
            loadScene();
        }
        else
        {
            if (requiredItem != inventoryDash.inventoryItem.none)
            {
                int requiredItemID = (int)requiredItem;
                bool hasItem = eventCenter.checkItemExist(requiredItemID);
                if (hasItem && allowAutoItem) // Player have enough item
                {
                    eventCenter.tryUseItem(requiredItemID);
                    //FsmVariables.GlobalVariables.GetFsmGameObject("currentEventObject").Value = transform.gameObject;
                    dialogue.currentSlotItem = itemDB.getItem(requiredItemID).title;
                    dialogue.showMessage(successfulMsg);
                    eventCenter.setFlagTrue(flagName);
                    setDoorTexture();
                    if (isElectricalDoor)
                        aud.clip = electricUnlocked;
                    else
                        aud.clip = unlockSound;
                    aud.Play();
                }
                else//Player has insufficient item
                {
                    aud.clip = lockedSound;
                    aud.Play();
                    dialogue.showMessage(flagName);
                }
            }
            else
            {
                aud.clip = lockedSound;
                aud.Play();
                dialogue.showMessage(flagName);
                ODM.errorLog(transform.name, "mapSetting missing required item.", "");
            }
        }
    }

    public string setLocationString()
    {
        try
        {
            string senderName = transform.name;
            string currentSceneName = Application.loadedLevelName;
            switch (senderName)
            {
                case "Map Right Path":
                    nextSceneName = currentSceneName[0] + char.ConvertFromUtf32(System.Text.Encoding.ASCII.GetBytes(currentSceneName[1].ToString())[0] + 1);
                    break;
                case "Map Left Path":
                    nextSceneName = currentSceneName[0] + char.ConvertFromUtf32(System.Text.Encoding.ASCII.GetBytes(currentSceneName[1].ToString())[0] - 1);
                    break;
                case "Map Up Door":
                    nextSceneName = char.ConvertFromUtf32(System.Text.Encoding.ASCII.GetBytes(currentSceneName[0].ToString())[0] - 1) + currentSceneName[1];
                    break;
                case "Map Down Door":
                    nextSceneName = char.ConvertFromUtf32(System.Text.Encoding.ASCII.GetBytes(currentSceneName[0].ToString())[0] + 1) + currentSceneName[1];
                    break;
                case "Map Left Door":
                    nextSceneName = currentSceneName[0] + char.ConvertFromUtf32(System.Text.Encoding.ASCII.GetBytes(currentSceneName[1].ToString())[0] - 1);
                    break;
                case "Map Right Door":
                    nextSceneName = currentSceneName[0] + char.ConvertFromUtf32(System.Text.Encoding.ASCII.GetBytes(currentSceneName[1].ToString())[0] + 1);
                    break;
            }
            //transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<UiTranslator>().resetText(nextSceneName);
            hint.GetComponent<UiTranslator>().resetText(nextSceneName);
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name, "setLocationString Error.", ex.ToString());
        }
        return nextSceneName;
    }
    public void loadScene()
    {
        FsmVariables.GlobalVariables.GetFsmString("fromDoor").Value = transform.name;
        Application.LoadLevel(nextSceneName);
    }

    public void setDoorTexture()
    {
        if (isElectricalDoor)
        {
            try
            {
                if (eventManager.GetComponent<eventCenter>().getFlagBool(flagName))
                {
                    if (transform.name.IndexOf("Left") != -1)
                        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Background/Left Door Unlocked");
                    if (transform.name.IndexOf("Right") != -1)
                        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Background/Right Door Unlocked");
                    if (transform.name.IndexOf("Up") != -1 || transform.name.IndexOf("Down") != -1)
                        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Background/Door Unlocked");
                }
                else
                {
                    if (transform.name.IndexOf("Left") != -1)
                        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Background/Left Door Locked");
                    if (transform.name.IndexOf("Right") != -1)
                        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Background/Right Door Locked");
                    if (transform.name.IndexOf("Up") != -1 || transform.name.IndexOf("Down") != -1)
                        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Background/Door Locked");
                }
            }
            catch (Exception ex)
            {
                ODM.errorLog(transform.name, "setDoorTexture Error.", ex.ToString());
            }
        }
    }

    public string identifyDoorFlagName()
    {
        string currentSceneName = Application.loadedLevelName;
        string targetFlagName = "";
        if (transform.name.IndexOf("Map Up") != -1)
        {
            targetFlagName = char.ConvertFromUtf32(System.Text.Encoding.ASCII.GetBytes(currentSceneName[0].ToString())[0] - 1) + currentSceneName[1] + " Down Door";
        }
        if (transform.name.IndexOf("Map Down") != -1)
        {
            targetFlagName = currentSceneName + " Down Door";
        }
        if (transform.name.IndexOf("Map Right") != -1)
        {
            targetFlagName = currentSceneName + " Right Door";
        }
        if (transform.name.IndexOf("Map Left") != -1)
        {
            targetFlagName = currentSceneName[0] + char.ConvertFromUtf32(System.Text.Encoding.ASCII.GetBytes(currentSceneName[1].ToString())[0] - 1) + " Right Door";
        }
        return targetFlagName;
    }
}