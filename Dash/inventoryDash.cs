using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using HutongGames.PlayMaker;
using System;
using System.Linq;
using LitJson;

public class inventoryDash : MonoBehaviour
{
    //For Reference
    public GameObject InventoryPanel;

    //For Instance
    public GameObject slot;//ui img from Prefabs
    public GameObject itemImage;//ui img from Prefabs
    public GameObject txtStock;
    public GameObject itemNameTag;
    public GameObject txtPageCount;

    public GameObject inventory_display_image;
    public GameObject inventory_display_image_background;
    public GameObject inventory_description_area;
    public GameObject text_inventory_description;
    public GameObject item_title;
    public GameObject text_press_to_use;

    public List<item> inventoryCollection;
    public List<GameObject> slotCollection;

    public GameObject saveBeacon;

    private ItemDatabase itemDB;
    private bool stateControl = false;//for first time selection control
    private string img_itemBackgrounFile = "UI/[UI]Harf Transparent Side B Full";
    private string img_itemSelectedBackgrounFile = "UI/[UI]Item Selection";


    private int currentIndex = -1;
    private int slotCount = 5;
    private int itemCollectionSize = 100;//shows the limit kinds of items that a player can carry
    private int currentPage = 1;
    private int pageMax = 2;

    private audioManager aud_manager;
    private AudioSource aud;

    public bool inventoryPanelEnabled = false; //inventory interacterable
    private bool confirmCheck = false; //check panel interacterable

    public enum inventoryItem
    {
        none = -1,

        talent_necklace = 0,
        rct_pistol = 1,
        little_bastard = 2,
        ellas_hair_ornaments = 3,
        countdown_handcuffs = 4,
        parasitism_launcher = 5,
        pheromone_of_warmbugs = 6,
        refined_pheromone_of_warmbugs = 7,
        beacon_of_the_shadows = 8,

        red_core_technology_energy = 20,
        beacon_of_the_esf = 21,
        general_battery = 22,
        creature_serum = 23,

        refined_lactation = 30,
        nourished_larvae = 31,
        semen_of_mecb = 32,
        ambusher_semen = 33,
        silencer_semen = 34,

        device_battery = 40,
        metal_door_component = 41,
        advanced_integrated_cable = 42,
        bio_destruction_device = 43,
        component_courage = 44,
        component_hope = 45,
        component_love = 46,


        lockpick = 60,
        staff_id = 61,
        administrator_id = 62,
        toxic_id = 63,
        equipment_room_id = 64,
        hell_ranch_id = 65,
        deck_chip = 66,
        transportation_room_chip = 67,

        electric_authentication_A = 68,
        electric_authentication_B = 69,
        electric_authentication_C = 70

    }
    void Awake()
    {
        aud = GetComponent<AudioSource>();
        itemDB = GetComponent<ItemDatabase>();
        inventoryCollection = new List<item>();
        slotCollection = new List<GameObject>();
        aud_manager = new audioManager();

        for (int i = 0; i < itemCollectionSize; i++)
        {
            inventoryCollection.Add(new item());
        }

        for (int i = 0; i < slotCount; i++)
        {
            GameObject newSlot = Instantiate(slot);
            GameObject newItemImage = Instantiate(itemImage);
            GameObject newStock = Instantiate(txtStock);
            GameObject newTag = Instantiate(itemNameTag);

            /* Code below is replaced by setting of flexible hight at a value of 0.2f */
            //float panelHight = InventoryPanel.GetComponent<RectTransform>().rect.height;
            //newImage.GetComponent<RectTransform>().sizeDelta =
            //    new Vector2(newSlot.GetComponent<RectTransform>().rect.width, (panelHight / slotCount) - 20);

            setAlphaZero(ref newItemImage);
            newStock.transform.GetComponent<Text>().text = "";
            newTag.transform.GetComponent<Text>().text = "";
            newStock.transform.SetParent(newItemImage.transform);
            newItemImage.transform.SetParent(newSlot.transform);
            newTag.transform.SetParent(newSlot.transform);

            slotCollection.Add(newSlot);
            slotCollection[i].transform.SetParent(InventoryPanel.transform);
        }
    }

    void Start()
    {
        //add items according to play inventory data
        updatePage();
        refreshItemStatus();
        displayItem();//display things properly...

        if (FsmVariables.GlobalVariables.GetFsmBool("create_new_save").Value)
        {
            //new save basic equipment
            addItem((int)inventoryItem.talent_necklace, 1);
            addItem((int)inventoryItem.rct_pistol, 1);
            //addItem((int)inventoryItem.little_bastard, 1);
            //addItem((int)inventoryItem.beacon_of_the_esf, 3);
        }
        else
        {
            addItemArray(FsmVariables.GlobalVariables.GetFsmGameObject("saveBuilder").Value.GetComponent<saveLoader>().inventory_collection);
        }

    }
    void Update()
    {
        //select first item and display
        if (inventoryPanelEnabled && !stateControl)
        {
            aud.clip = Resources.Load<AudioClip>(aud_manager.electrical);
            aud.Play();
            stateControl = true;//for the first time loading
            displayItem();
        }
        else if (inventoryPanelEnabled && stateControl)//When the select function is enabled...
        {
            if (Input.GetKeyDown(KeyCode.C))//Use a item
            {
                if (!confirmCheck)
                    useItemConfirmationCheck();
                else
                    confirmCheck = false;

            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                closePanel();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (convertToSlotIndex() == slotCount - 1)
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionNegative);
                    displayItem();

                }
                else//select next item
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                    currentIndex++;
                    displayItem();
                }
                aud.Play();
            }


            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (convertToSlotIndex() == 0)
                {
                    closePanel();
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                    aud.Play();
                    currentIndex--;
                    displayItem();
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentPage == pageMax)
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionNegative);
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                    nextPage();
                }
                aud.Play();
            }


            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentPage == 1)
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionNegative);
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                    previousPage();
                }
                aud.Play();
            }
        }

    }
    #region Data Manipulation

    public void addItem(int index)
    {
        if (index == (int)inventoryItem.device_battery)
            index = (int)inventoryItem.little_bastard;

        item itemToAdd = itemDB.getItem(index);

        if (index == (int)inventoryItem.red_core_technology_energy)
        {
            FsmVariables.GlobalVariables.GetFsmInt("bulletStock").Value++;
        }
        if (index == (int)inventoryItem.creature_serum)
        {
            FsmVariables.GlobalVariables.GetFsmInt("serum_count").Value++;
        }
        if (checkItemExist(itemToAdd) && itemToAdd.stackable)
        {
            for (int i = 0; i < inventoryCollection.Count; i++)
            {
                if (itemToAdd.id == inventoryCollection[i].id)
                {
                    inventoryCollection[i].amount++;
                    break;
                }
            }
        }

        else
        {
            for (int i = 0; i < inventoryCollection.Count; i++)
            {
                if (inventoryCollection[i].id == -1)//means empty slot
                {
                    inventoryCollection[i] = itemToAdd;//put selected item into collection
                    break;
                }
            }
        }
        updatePage();
    }
    public void addItem(int index, int amount)
    {
        // check space
        for (int i = 0; i < amount; i++)
        {
            addItem(index);
        }
    }
    public void addSupplyFull()
    {
        //Bullets in gun
        int bulletInGun = FsmVariables.GlobalVariables.GetFsmInt("currentBullet").Value =
            FsmVariables.GlobalVariables.GetFsmInt("bullet_max").Value;

        //Add Save Item
        if (!checkItemExist((int)inventoryItem.beacon_of_the_esf))
            addItem((int)inventoryItem.beacon_of_the_esf);

        //Bullets in bag
        int bulletStockMax = FsmVariables.GlobalVariables.GetFsmInt("bullet_stock_max").Value;
        int currentBulletStock = FsmVariables.GlobalVariables.GetFsmInt("bulletStock").Value;
        for (int i = currentBulletStock; i < bulletStockMax; i++)
            addItem((int)inventoryItem.red_core_technology_energy);

        //Serum

        int serumNeeded = FsmVariables.GlobalVariables.GetFsmInt("serum_max").Value - FsmVariables.GlobalVariables.GetFsmInt("serum_count").Value;
        for (int i = 0; i < serumNeeded; i++)
        {
            addItem((int)inventoryItem.creature_serum);
        }
        FsmVariables.GlobalVariables.GetFsmBool("status_serum").Value = true;
    }

    public bool checkItemExist(item _item)
    {
        item item = inventoryCollection.Where(x => x.id == _item.id).FirstOrDefault();
        if (item != null)
            return true;
        else
            return false;
    }

    public bool checkItemExist(int _itemID)
    {
        item item = inventoryCollection.Where(x => x.id == _itemID).FirstOrDefault();
        if (item != null)
            return true;
        else
            return false;
    }
    public bool checkAndTakeAway(int index)
    {
        bool result = false;
        item itemToUse = itemDB.getItem(index);
        if (checkItemExist(itemToUse) && itemToUse.stackable)
        {
            for (int i = 0; i < inventoryCollection.Count; i++)//need to know which slot, cannot use LINQ
            {
                if (itemToUse.id == inventoryCollection[i].id)
                {
                    item inStockItem = inventoryCollection[i];

                    if (inStockItem.amount == 1 && itemToUse.id != (int)inventoryItem.little_bastard)
                    {
                        inventoryCollection[i] = new item();//remove item which have only one in amount
                    }
                    else
                    {
                        inStockItem.amount--;
                    }
                    result = true;
                    break;
                }
            }
        }

        else if (checkItemExist(itemToUse) && !itemToUse.stackable)
        {
            for (int i = 0; i < inventoryCollection.Count; i++)
            {
                if (inventoryCollection[i].id == itemToUse.id)
                {
                    //No removal for ID card
                    if (itemToUse.id != (int)inventoryItem.staff_id &&
                        itemToUse.id != (int)inventoryItem.administrator_id &&
                        itemToUse.id != (int)inventoryItem.toxic_id &&
                         itemToUse.id != (int)inventoryItem.hell_ranch_id &&
                         itemToUse.id != (int)inventoryItem.equipment_room_id)
                        inventoryCollection[i] = new item();
                    result = true;
                    break;
                }
            }
        }

        //Keep item top from the list
        for (int i = 0; i < inventoryCollection.Count; i++)
        {
            if (inventoryCollection[i].id == -1)
            {
                for (int k = i; k < inventoryCollection.Count; k++)
                {
                    if (inventoryCollection[k].id != -1)
                    {
                        inventoryCollection[i] = inventoryCollection[k];
                        inventoryCollection[k] = new item();
                        break;
                    }
                }
            }
        }
        updatePage();

        return result;
    }
    public void reload(int needAmont)
    {
        int bullet = FsmVariables.GlobalVariables.GetFsmInt("bulletStock").Value;
        if (bullet > 0)
        {
            for (int i = 0; i < needAmont; i++)
            {
                checkAndTakeAway((int)inventoryItem.red_core_technology_energy);
            }
        }
    }

    #endregion



    #region Confirmation of using item
    private void useItemConfirmationCheck()//Using item intro function
    {
        if (inventoryCollection[currentIndex].id != -1)
        {
            confirmCheck = true; //For state control, will automatically turn on with inventoryPanelEnabled
            inventoryPanelEnabled = false;

            string msg = (new ODM()).getTranslaton("confirm use item sure", inventoryCollection[currentIndex].title);

            if (inventoryCollection[currentIndex].id == (int)inventoryItem.refined_lactation ||
                    inventoryCollection[currentIndex].id == (int)inventoryItem.semen_of_mecb ||
                    inventoryCollection[currentIndex].id == (int)inventoryItem.ambusher_semen)
            {
                msg = (new ODM()).getTranslaton("eat item", inventoryCollection[currentIndex].title);
            }
            FsmVariables.GlobalVariables.GetFsmGameObject("obj_ConfirmPanel").Value.GetComponent<confirmPanel>().
                showConfirmation(transform.gameObject, "onReply_useItem", msg);
        }
    }
    //Main functions of using items
    public void onReply_useItem(bool _result)
    {
        if (_result)//Player confirmed to use item
        {
            GameObject targetDoor = FsmVariables.GlobalVariables.GetFsmGameObject("currentActivateDoor").Value;//This value is cleared by PM trigger.

            //case: use item / default: use item to open a door
            switch (inventoryCollection[currentIndex].id)
            {
                case (int)inventoryItem.beacon_of_the_esf:
                    checkAndTakeAway(inventoryCollection[currentIndex].id);
                    //inventoryPanelEnabled = true;//???
                    GameObject ava = FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value;
                    GameObject _saveBeacon = Instantiate(saveBeacon);
                    _saveBeacon.transform.position = new Vector3(ava.transform.position.x, saveBeacon.transform.position.y, saveBeacon.transform.position.z);
                    //GetComponent<menuManager>().openMenu();
                    GetComponent<savePoint>().createSaveRecored(false);
                    break;
                case (int)inventoryItem.creature_serum:
                    useSerum();
                    break;
                //items for recovering hp
                case (int)inventoryItem.refined_lactation:
                    checkAndTakeAway(inventoryCollection[currentIndex].id);
                    FsmVariables.GlobalVariables.GetFsmFloat("ava_current_health").Value = FsmVariables.GlobalVariables.GetFsmFloat("ava_current_health").Value + 30f;
                    FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value.
                        GetComponent<messagePanel>().showMessage("recover hp");
                    break;
                case (int)inventoryItem.semen_of_mecb:
                    checkAndTakeAway(inventoryCollection[currentIndex].id);
                    FsmVariables.GlobalVariables.GetFsmFloat("ava_current_health").Value = 100f;
                    FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value.
                        GetComponent<messagePanel>().showMessage("recover hp");
                    break;
                //items for cure toxic
                case (int)inventoryItem.ambusher_semen:
                    checkAndTakeAway(inventoryCollection[currentIndex].id);
                    FsmVariables.GlobalVariables.GetFsmFloat("ava_current_poison").Value = 0f;
                    FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value.
                        GetComponent<messagePanel>().showMessage("recover toxic");
                    break;

                default:
                    if (targetDoor != null)
                    {
                        if ((int)targetDoor.GetComponent<mapSetting>().requiredItem == inventoryCollection[currentIndex].id)
                        {
                            switch (inventoryCollection[currentIndex].id)//Using item to open door 
                            {
                                case (int)inventoryItem.little_bastard:
                                    if (inventoryCollection[currentIndex].amount > 0)
                                    {
                                        fsmHelper.getFsm(targetDoor.name, "FSM").SendEvent("use inventory item");
                                    }
                                    else
                                    {
                                        FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value.
                                           GetComponent<messagePanel>().showMessage("Item connot use");
                                    }

                                    break;
                                case (int)inventoryItem.metal_door_component:
                                    //inventoryPanelEnabled = true;
                                    fsmHelper.getFsm(targetDoor.name, "FSM").SendEvent("use inventory item");
                                    break;
                                case (int)inventoryItem.general_battery:
                                    //inventoryPanelEnabled = true;
                                    fsmHelper.getFsm(targetDoor.name, "FSM").SendEvent("use inventory item");
                                    break;
                                default:
                                    FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value.
                                        GetComponent<messagePanel>().showMessage("Item connot use");
                                    break;
                            }
                        }
                        else
                        {
                            FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value.
                                       GetComponent<messagePanel>().showMessage("Item connot use");
                            ODM.log(transform.name, "Item cannot use, required item does not match target door.");
                        }
                    }
                    else
                    {
                        FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value
                            .GetComponent<messagePanel>().showMessage("Item connot use");
                        ODM.log(transform.name, "Item cannot use, there is no target door.");
                    }
                    break;
            }
            GetComponent<menuManager>().openMenu();
        }
        else//Player decided not to use a item
        {
            stopUsingItem();
        }
    }

    public void useSerum()
    {
        checkAndTakeAway((int)inventoryItem.creature_serum);
        fsmHelper.getFsm(transform.gameObject, "Serum").SendEvent("activate serum");
        FsmVariables.GlobalVariables.GetFsmInt("serum_count").Value--;
        FsmVariables.GlobalVariables.GetFsmGameObject("messageDisplayPanel").Value.
            GetComponent<messagePanel>().showMessage("use serum");
    }

    private void stopUsingItem()
    {
        inventoryPanelEnabled = true;
        ODM.log(transform.name, "Stop using item.");
    }
    public void onReply_messageEnd()//Showing the result of using item
    {
        inventoryPanelEnabled = true;
    }
    #endregion

    #region Navigation
    private void nextPage()
    {
        currentPage++;
        currentIndex += slotCount;
        updatePage();
        refreshItemStatus();
        displayItem();
    }

    private void previousPage()
    {
        currentPage--;
        currentIndex -= slotCount;
        updatePage();
        refreshItemStatus();
        displayItem();
    }

    public void openPanel()
    {
        //play sound
        inventoryPanelEnabled = true;
        currentIndex = 0;
        refreshItemStatus();
        displayItem();
    }

    public void closePanel()
    {
        aud.clip = Resources.Load<AudioClip>(aud_manager.electricalExit);
        aud.Play();
        GetComponent<menuManager>().tabSwitch = true; //make the top tab goes back to previous state...
        stateControl = confirmCheck = inventoryPanelEnabled = false;

        FsmVariables.GlobalVariables.GetFsmGameObject("obj_ConfirmPanel").Value.GetComponent<confirmPanel>().quitPanel();

        resetVariables();
        refreshItemStatus();
        displayItem();
    }

    #endregion

    #region Display
    private void updatePage()
    {
        //int currentItemNumber = 0;
        //for (int i = 0; i < inventoryCollection.Count; i++)
        //{
        //    if (inventoryCollection[i].id != -1)
        //    {
        //        currentItemNumber++;
        //    }
        //}

        //pageMax = currentItemNumber / slotCount;
        //if (currentItemNumber % slotCount != 0)
        //{
        //    pageMax++;
        //}
        txtPageCount.GetComponent<Text>().text = currentPage + "/" + pageMax;
    }
    public void refreshItemStatus()
    {
        for (int i = 0; i < slotCount; i++)
        {
            item item = inventoryCollection[(currentPage - 1) * slotCount + i];

            if (item.id == -1)
            {
                GameObject itemImage = slotCollection[i].transform.GetChild(0).gameObject;
                setAlphaZero(ref itemImage);
                slotCollection[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "";
                slotCollection[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            else
            {
                GameObject itemImage = slotCollection[i].transform.GetChild(0).gameObject;
                itemImage.GetComponent<Image>().sprite = item.sprite;
                setAlphaOne(ref itemImage);
                if (item.stackable)
                {
                    slotCollection[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.amount.ToString();
                }
                else
                {
                    slotCollection[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "";
                }
                slotCollection[i].transform.GetChild(1).GetComponent<Text>().text = item.shortName;
            }
        }
    }
    private void displayItem()
    {
        //setting slot background to selected
        //if currentSelectedIndex = -1, all will return to unselected

        for (int i = 0; i < slotCount; i++)
        {

            if (convertToSlotIndex() == i)
            {
                slotCollection[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(img_itemSelectedBackgrounFile);
                slotCollection[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                slotCollection[i].transform.GetChild(1).GetComponent<Text>().color = new Color32(0, 0, 0, 255);
            }
            else
            {
                slotCollection[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(img_itemBackgrounFile);
                slotCollection[i].GetComponent<Image>().color = new Color32(70, 70, 70, 180);
                slotCollection[i].transform.GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
        }

        //Display item in the middle...
        //var img = GameObject.Find("Inventory Display Image");
        //var background = GameObject.Find("Inventory Display Image Background");
        //var txt = GameObject.Find("No Data Text");
        //var box = GameObject.Find("Inventory Description Area");
        //var txtDescription = GameObject.Find("Text Inventory Description");
        //var txtItemTitle = GameObject.Find("Item Title");
        //var txtPress = GameObject.Find("text press to use");

        //Show item data in the middle

        if (currentIndex == -1 || inventoryCollection[currentIndex].id == -1)
        {
            inventory_display_image.GetComponent<CanvasGroup>().alpha = 0;
            inventory_display_image_background.GetComponent<CanvasGroup>().alpha = 1;
            inventory_description_area.GetComponent<Menu>().isOpen = false;
            //text_inventory_description.GetComponent<Text>().text = "";
            item_title.GetComponent<Text>().text = "";
            text_press_to_use.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            inventory_display_image.GetComponent<Image>().sprite = inventoryCollection[currentIndex].sprite;
            inventory_display_image.GetComponent<CanvasGroup>().alpha = 1;
            inventory_display_image_background.GetComponent<CanvasGroup>().alpha = 0;
            inventory_description_area.GetComponent<Menu>().isOpen = true;
            text_inventory_description.GetComponent<Text>().text = inventoryCollection[currentIndex].description;
            item_title.GetComponent<Text>().text = inventoryCollection[currentIndex].title;
            text_press_to_use.GetComponent<CanvasGroup>().alpha = 1;
        }

    }
    private void setAlphaZero(ref GameObject obj)
    {
        Color c = obj.transform.GetComponent<Image>().color;
        c.a = 0;
        obj.transform.GetComponent<Image>().color = c;
    }

    private void setAlphaOne(ref GameObject obj)
    {
        Color c = obj.transform.GetComponent<Image>().color;
        c.a = 1f;
        obj.transform.GetComponent<Image>().color = c;
    }
    #endregion

    #region Data Manipulation
    public void getItemNameByReturn(string _pack)
    {
        string[] data = _pack.Split(',');//objName, fsmName, docKey
        item _item = itemDB.getItem(Int32.Parse(data[2].Replace("\r", "")));
        if (_item != null)
        {
            if (String.IsNullOrEmpty(_item.title))
                ODM.errorLog(transform.name,
                    "getDocumentName Error. Key =" + _pack,
                    "");
            else
            {
                fsmHelper fsmHelper = new fsmHelper();
                PlayMakerFSM f = fsmHelper.getFsm(data[0], data[1]);
                f.FsmVariables.GetFsmString("slot item name").Value = _item.title;
            }
        }
    }
    public string getItemArray()
    {
        List<int[]> listItemData = new List<int[]>();
        for (int i = 0; i < inventoryCollection.Count; i++)
        {
            if (inventoryCollection[i].id != -1)
            {
                listItemData.Add(new int[2] { inventoryCollection[i].id, inventoryCollection[i].amount });
            }
        }
        JsonData inventoryJson = JsonMapper.ToJson(listItemData);
        return inventoryJson.ToString();
    }
    public void addItemArray(List<int[]> _listItem)
    {
        for (int i = 0; i < _listItem.Count; i++)
        {
            addItem(_listItem[i][0], _listItem[i][1]);
        }
    }
    private int convertToSlotIndex()
    {
        if (currentIndex == -1)
            return -1;
        int idx = currentIndex % slotCount;
        return idx;
    }
    private void resetVariables()
    {
        currentPage = 1;
        currentIndex = -1;
        updatePage();
    }
    #endregion


}
