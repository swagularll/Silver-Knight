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
    //For reference
    public GameObject InventoryPanel;

    //renew code
    //For instance
    public GameObject slot;//ui img from Prefabs
    public GameObject itemImage;//ui img from Prefabs
    public GameObject txtStock;
    public GameObject itemNameTag;
    public GameObject txtPageCount;

    //For instance
    public GameObject instance_save_beacon;

    //For item combination image display
    public GameObject inventory_selected_item_main;
    public GameObject inventory_no_item_image;

    public GameObject inventory_modification_display_panel;
    public GameObject inventory_combination_display_panel;

    public GameObject inventory_selected_item_a;
    public GameObject inventory_selected_item_b;



    public GameObject text_inventory_description;
    public GameObject item_title;
    public GameObject text_press_to_use;

    //Description Area
    public GameObject inventory_description_area;
    public GameObject inventory_modification_description_area;

    //Modification buttion control panel
    public GameObject inventory_item_modification_panel;

    public List<item> inventory_collection;
    public List<GameObject> slot_collection;

    private item selected_combine_item_a;
    private item selected_combine_item_b;


    //renew code
    //private string img_item_background = "UI/[UI]Harf Transparent Side B Full";
    //private string img_item_background_selected = "UI/[UI]Item Selection";
    //public Sprite img_item_background;
    //public Sprite img_item_background_selected;
    public Sprite no_item_image;

    private AudioSource aud;
    private ItemDatabase item_db;

    public int current_index = -1;
    private int slot_count = 5;
    private int item_collection_size = 100;//shows the limit kinds of items that a player can carry
    private int current_page = 1;
    private int page_max = 2;

    private int primary_item_index;//For combine item
    private int last_added_item_index;

    private string on_reply_use_item = "confirmUseItem";
    private string on_reply_drop_item = "confirmDropItem";
    private string fsm_name_item_modification = "Item Modification";
    private PlayMakerFSM item_modification_fsm;

    private bool state_control = false;//for first time selection control
    private bool state_control_combine_item = false;//for first time selection control

    public bool panel_enabled = false; //inventory interacterable
    private bool is_modifying_item = false; //check panel interacterable
    private bool is_combining_item = false; //check panel interacterable
    

    #region Initilization
    void Awake()
    {
        aud = GetComponent<AudioSource>();
        item_db = GetComponent<ItemDatabase>();
        inventory_collection = new List<item>();
        slot_collection = new List<GameObject>();

        for (int i = 0; i < item_collection_size; i++)
        {
            inventory_collection.Add(new item());
        }

        for (int i = 0; i < slot_count; i++)
        {
            GameObject newSlot = Instantiate(slot);
            GameObject newItemImage = Instantiate(itemImage);
            GameObject newStock = Instantiate(txtStock);
            GameObject newTag = Instantiate(itemNameTag);

            setAlphaZero(ref newItemImage);
            newStock.transform.GetComponent<Text>().text = "";
            newTag.transform.GetComponent<Text>().text = "";
            newStock.transform.SetParent(newItemImage.transform);
            newItemImage.transform.SetParent(newSlot.transform);
            newTag.transform.SetParent(newSlot.transform);
            //newTag> newSlot>newItemImage > newStock
            slot_collection.Add(newSlot);
            slot_collection[i].transform.SetParent(InventoryPanel.transform);
        }
    }
    void Start()
    {
        item_modification_fsm = fsmHelper.getFsm(inventory_item_modification_panel, fsm_name_item_modification);

        updatePage();
        refreshItemStatus();
        displayItem();

        //New game starting equipments
        if (ODMVariable.create_new_save)
        {
            addItem((int)ODMVariable.itemCatalogue.talent_necklace, 1);
            addItem((int)ODMVariable.itemCatalogue.rct_pistol, 1);
        }
        else
        {
            //add items according to inventory data
            addItemArray(ODMObject.save_builder.GetComponent<saveLoader>().inventory_collection);
        }

    }
    #endregion

    #region Panel Control
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            sortItem(0, 8);
        }
        //select first item and display
        if (panel_enabled && !state_control)
        {
            aud.clip = Resources.Load<AudioClip>(audioResource.electrical);
            aud.Play();
            state_control = true;//for the first time loading
            displayItem();
        }
        else if (panel_enabled && state_control && !ODMVariable.is_system_locked)//When the select function is enabled...
        {
            if (Input.GetKeyDown(KeyCode.C))//Use a item
            {
                if (!is_modifying_item)
                {
                    startModifyItem();
                }
                else if (!state_control_combine_item && is_combining_item)
                {
                    state_control_combine_item = true;
                    Debug.Log("start combining item");
                }
                else if (state_control_combine_item && is_combining_item)
                {
                    tryCombineItem();
                    Debug.Log("try to combine item a with item b");
                }
                else
                {
                    Debug.Log("Nothing do to...");
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {

                if (is_combining_item)
                {
                    endCombineItem();
                }
                else if (is_modifying_item)
                {
                    endModifyItem();
                }
                else
                {
                    closePanel();
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (convertToSlotIndex() == slot_count - 1)
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                    displayItem();

                }
                else//select next item
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    current_index++;
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
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    aud.Play();
                    current_index--;
                    displayItem();
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (current_page == page_max)
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    nextPage();
                }
                aud.Play();
            }


            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (current_page == 1)
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    previousPage();
                }
                aud.Play();
            }
        }
    }
    #endregion

    #region Core

    private void startModifyItem()
    {
        if (inventory_collection[current_index].id != -1)
        {
            panel_enabled = false;
            is_modifying_item = true;

            inventory_description_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, false);
            inventory_modification_description_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, true );

            //Set panel
            inventory_modification_display_panel.GetComponent<CanvasGroup>().alpha = 0;
            inventory_combination_display_panel.GetComponent<CanvasGroup>().alpha = 1;

            //Clear data
            clearCombineItems();

            //Send events
            inventory_item_modification_panel.SendMessage(eventName.system.show_gui);
            item_modification_fsm.SendEvent(eventName.show_menu);
        }
    }
    public void endModifyItem()
    {
        panel_enabled = true;
        is_modifying_item = false;
        inventory_description_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, true);
        inventory_modification_description_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, false);

        //Set panel
        inventory_modification_display_panel.GetComponent<CanvasGroup>().alpha = 1;
        inventory_combination_display_panel.GetComponent<CanvasGroup>().alpha = 0;

        inventory_item_modification_panel.SendMessage(eventName.system.show_gui);
        clearCombineItems();
        item_modification_fsm.SendEvent(eventName.hide_menu);
    }

    private void useItemConfirmationCheck()//Using item intro function
    {
        string msg = dataWidget.getTranslaton(ODMVariable.translation.trans_use_item_confirm, inventory_collection[current_index].title);

        //Set special message for particular items
        if (inventory_collection[current_index].id == (int)ODMVariable.itemCatalogue.refined_lactation ||
                inventory_collection[current_index].id == (int)ODMVariable.itemCatalogue.semen_of_mecb ||
                inventory_collection[current_index].id == (int)ODMVariable.itemCatalogue.ambusher_semen)
        {
            msg = dataWidget.getTranslaton(ODMVariable.translation.eat_warmbug_item, inventory_collection[current_index].title);
        }
        ODMObject.confirmation_panel.GetComponent<confirmPanel>().showConfirmation(transform.gameObject, on_reply_use_item, msg);
    }

    //CORE FUNCTION
    public void confirmUseItem(bool _result)
    {
        if (_result)//Player confirmed to use item
        {
            GameObject targetDoor = ODMObject.current_activate_door;
            //case: use item / default: use item to open a door
            switch (inventory_collection[current_index].id)
            {
                case (int)ODMVariable.itemCatalogue.beacon_of_the_esf:
                    checkAndTakeAway(inventory_collection[current_index].id);
                    GameObject ava = ODMObject.character_ava;
                    GameObject _saveBeacon = Instantiate(instance_save_beacon);
                    _saveBeacon.transform.position = new Vector3(ava.transform.position.x, instance_save_beacon.transform.position.y, instance_save_beacon.transform.position.z);
                    GetComponent<savePoint>().createSaveRecored(false);
                    break;
                case (int)ODMVariable.itemCatalogue.creature_serum:
                    useSerum();
                    break;
                //items for recovering hp
                case (int)ODMVariable.itemCatalogue.refined_lactation:
                    checkAndTakeAway(inventory_collection[current_index].id);
                    ODMVariable.ava_current_health += 30f;
                    ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.recover_hp);
                    break;
                case (int)ODMVariable.itemCatalogue.semen_of_mecb:
                    checkAndTakeAway(inventory_collection[current_index].id);
                    ODMVariable.ava_current_health = 100f;
                    ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.recover_hp);
                    break;
                //items for cure toxic
                case (int)ODMVariable.itemCatalogue.ambusher_semen:
                    checkAndTakeAway(inventory_collection[current_index].id);
                    ODMVariable.ava_current_poison = 0f;
                    ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.recover_toxic);
                    break;

                default:
                    if (targetDoor != null)
                    {
                        if ((int)targetDoor.GetComponent<mapSetting>().requiredItem == inventory_collection[current_index].id)
                        {
                            switch (inventory_collection[current_index].id)//Using item to open door 
                            {
                                case (int)ODMVariable.itemCatalogue.little_bastard:
                                    if (inventory_collection[current_index].amount > 0)
                                    {
                                        fsmHelper.getFsm(targetDoor.name, ODMVariable.common.default_fsm).SendEvent(eventName.use_inventory_item);
                                    }
                                    else
                                    {
                                        ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.item_connot_use);
                                    }
                                    break;
                                case (int)ODMVariable.itemCatalogue.metal_door_component:
                                    fsmHelper.getFsm(targetDoor.name, ODMVariable.common.default_fsm).SendEvent(eventName.use_inventory_item);
                                    break;
                                case (int)ODMVariable.itemCatalogue.general_battery:
                                    fsmHelper.getFsm(targetDoor.name, ODMVariable.common.default_fsm).SendEvent(eventName.use_inventory_item);
                                    break;
                                default:
                                    ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.item_connot_use);
                                    break;
                            }
                        }
                        else
                        {
                            ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.item_connot_use);
                            ODM.log(transform.name, "Cannot use use, required item does not match target door.");
                        }
                    }
                    else
                    {
                        ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.item_connot_use);
                        ODM.log(transform.name, "Cannot use use, missing target door.");
                    }
                    break;
            }
            GetComponent<menuManager>().openMenu();
        }
        else
        {
            //Player decided not to use a item
            stopUsingItem();
        }
    }
    private void stopUsingItem()
    {
        panel_enabled = true;
        item_modification_fsm.SendEvent(eventName.back);
    }
    public void startCombineItem()
    {
        panel_enabled = is_combining_item = true;
        inventory_description_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, true);
        inventory_modification_description_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, false);

        selected_combine_item_a = inventory_collection[current_index];
        primary_item_index = current_index;
        inventory_selected_item_a.GetComponent<Image>().sprite = inventory_collection[current_index].sprite;
        //FSM show combine message
    }
    public void endCombineItem()
    {
        panel_enabled = is_combining_item = false;
        
        item_modification_fsm.SendEvent(eventName.back);
        //FSM hide combine message
    }
    public void tryCombineItem()
    {
        //Previous stage*****X
        if (inventory_collection[current_index].id == -1)
        {
            //No item
            aud.clip = Resources.Load<AudioClip>(audioResource.wooden_fish);
        }
        else if (inventory_collection[current_index].id == selected_combine_item_a.id)
        {
            aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
        }
        else
        {
            selected_combine_item_b = inventory_collection[current_index];
            if (selected_combine_item_a == selected_combine_item_b)//Player trying to combine same item
            {
                ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.item_connot_use);
            }
            else
            {
                bool result = combineItem();
                if (result)
                {

                    //Successfully combined item
                    ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.confirm);
                }
                else
                {
                    //Show fail to combine item message
                    ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.cancel);
                }
            }
            refreshItemStatus();
            displayItem();
            endModifyItem();
        }
    }

    private void sortItem(int _current_item_index, int _target_item_index)
    {
        if (_current_item_index > _target_item_index)
        {
            item itemToSwitch = inventory_collection[_current_item_index];
            for (int i = _current_item_index; i >= _target_item_index; i--)
            {
                if (i != _target_item_index)
                {
                    inventory_collection[i] = inventory_collection[i - 1];
                }
                else
                {
                    inventory_collection[i] = itemToSwitch;
                }
            }
        }
        else
        {
            item itemToSwitch = inventory_collection[_current_item_index];
            for (int i = _current_item_index; i <= _target_item_index; i++)
            {
                if (i != _target_item_index)
                {
                    inventory_collection[i] = inventory_collection[i + 1];
                }
                else
                {
                    inventory_collection[i] = itemToSwitch;
                }
            }

        }
        refreshItemStatus();
        displayItem();
    }

    public bool combineItem()
    {
        //XXX check if item cannot be combined
        bool result = false;

        string[] combination_list = selected_combine_item_a.combination.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < combination_list.Length; i++)
        {
            string[] comb = combination_list[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (comb.Length == 2)
            {
                int item_b_id = Int32.Parse(comb[0]);
                int item_output_id = Int32.Parse(comb[1]);

                if (item_b_id == selected_combine_item_b.id)
                {
                    checkAndTakeAway(selected_combine_item_a.id);
                    checkAndTakeAway(selected_combine_item_b.id);
                    addItem(item_output_id);
                    result = true;
                    break;
                }
            }
            else
            {
                ODM.errorLog(transform.name, "tryCombineItem Error: Invalid data type.");
            }

        }
        if (result)
        {
            sortItem(last_added_item_index, primary_item_index);
            current_index = primary_item_index;
        }
        return result;
    }

    public void clearCombineItems()
    {
        selected_combine_item_a = null;
        selected_combine_item_b = null;
    }
    private void dropItemConfirmationCheck()//Using item intro function
    {
        if (inventory_collection[current_index].id != -1)
        {
            panel_enabled = false;
            string msg = dataWidget.getTranslaton(ODMVariable.translation.sure_drop_item);
            ODMObject.confirmation_panel.GetComponent<confirmPanel>().showConfirmation(transform.gameObject, on_reply_drop_item, msg);
        }
    }
    public void confirmDropItem(bool _result)
    {
        if (_result)
        {
            itemManager item_manager = GetComponent<itemManager>();
            item_manager.createItem(inventory_collection[current_index].id.ToString());
            checkAndTakeAway(inventory_collection[current_index].id);
            GetComponent<menuManager>().openMenu();
        }
        else
        {
            item_modification_fsm.SendEvent(eventName.back);
        }
    }

    public void stopDropItem()
    {
        itemManager item_manager = GetComponent<itemManager>();
        item_manager.createItem(inventory_collection[current_index].id.ToString());
    }

    public void useSerum()
    {
        checkAndTakeAway((int)ODMVariable.itemCatalogue.creature_serum);
        ODMVariable.fsm.serum_controller.SendEvent(eventName.activate_serum);
        ODMVariable.serum_count--;
        ODMObject.message_display_panel.GetComponent<messagePanel>().showMessage(ODMVariable.translation.use_serum);
    }

    public void onReply_messageEnd()//Showing the result of using item
    {
        panel_enabled = true;
    }
    #endregion

    #region Data Manipulation
    public void addSupplyFull()
    {
        //Fill bullets in gun
        int bulletInGun = ODMVariable.current_bullet = ODMVariable.bullet_max;

        //Get save data item
        if (!checkItemExist((int)ODMVariable.itemCatalogue.beacon_of_the_esf))
            addItem((int)ODMVariable.itemCatalogue.beacon_of_the_esf);

        //Fill bullets in bag
        for (int i = ODMVariable.bullet_stock; i < ODMVariable.bullet_stock_max; i++)
            addItem((int)ODMVariable.itemCatalogue.red_core_technology_energy);

        //Fill serum
        int serumNeeded = ODMVariable.serum_max - ODMVariable.serum_count;
        for (int i = 0; i < serumNeeded; i++)
        {
            addItem((int)ODMVariable.itemCatalogue.creature_serum);
        }
        //Reject serum
        ODMVariable.status_serum = true;
    }
    public void addItem(int _item_id)
    {
        if (_item_id == (int)ODMVariable.itemCatalogue.device_battery)
            _item_id = (int)ODMVariable.itemCatalogue.little_bastard;

        item itemToAdd = item_db.getItem(_item_id);

        if (_item_id == (int)ODMVariable.itemCatalogue.red_core_technology_energy)
        {
            ODMVariable.bullet_stock++;
        }
        if (_item_id == (int)ODMVariable.itemCatalogue.creature_serum)
        {
            ODMVariable.serum_count++;
        }
        if (checkItemExist(itemToAdd) && itemToAdd.stackable)
        {
            for (int i = 0; i < inventory_collection.Count; i++)
            {
                if (itemToAdd.id == inventory_collection[i].id)
                {
                    inventory_collection[i].amount++;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < inventory_collection.Count; i++)
            {
                if (inventory_collection[i].id == -1)//means empty slot
                {
                    last_added_item_index = i;
                    inventory_collection[i] = itemToAdd;//put selected item into collection
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
    public bool checkItemExist(item _item)
    {
        item item = inventory_collection.Where(x => x.id == _item.id).FirstOrDefault();
        if (item != null)
            return true;
        else
            return false;
    }
    public bool checkItemExist(int _itemID)
    {
        item item = inventory_collection.Where(x => x.id == _itemID).FirstOrDefault();
        if (item != null)
            return true;
        else
            return false;
    }
    public bool checkAndTakeAway(int _item_id)
    {
        bool result = false;
        item itemToUse = item_db.getItem(_item_id);
        if (checkItemExist(itemToUse) && itemToUse.stackable)
        {
            for (int i = 0; i < inventory_collection.Count; i++)//need to know which slot, cannot use LINQ
            {
                if (itemToUse.id == inventory_collection[i].id)
                {
                    item inStockItem = inventory_collection[i];

                    if (inStockItem.amount == 1 && itemToUse.id != (int)ODMVariable.itemCatalogue.little_bastard)
                    {
                        inventory_collection[i] = new item();//remove item which have only one in amount
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
            for (int i = 0; i < inventory_collection.Count; i++)
            {
                if (inventory_collection[i].id == itemToUse.id)
                {
                    //No removal for ID card
                    if (itemToUse.id != (int)ODMVariable.itemCatalogue.staff_id &&
                        itemToUse.id != (int)ODMVariable.itemCatalogue.administrator_id &&
                        itemToUse.id != (int)ODMVariable.itemCatalogue.toxic_id &&
                         itemToUse.id != (int)ODMVariable.itemCatalogue.hell_ranch_id &&
                         itemToUse.id != (int)ODMVariable.itemCatalogue.equipment_room_id)
                        inventory_collection[i] = new item();
                    result = true;
                    break;
                }
            }
        }

        //Keep item top from the list
        for (int i = 0; i < inventory_collection.Count; i++)
        {
            if (inventory_collection[i].id == -1)
            {
                for (int k = i; k < inventory_collection.Count; k++)
                {
                    if (inventory_collection[k].id != -1)
                    {
                        inventory_collection[i] = inventory_collection[k];
                        inventory_collection[k] = new item();
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
        if (ODMVariable.bullet_stock > 0)
        {
            for (int i = 0; i < needAmont; i++)
            {
                checkAndTakeAway((int)ODMVariable.itemCatalogue.red_core_technology_energy);
            }
        }
    }
    #endregion

    #region Navigation
    private void nextPage()
    {
        current_page++;
        current_index += slot_count;
        updatePage();
        refreshItemStatus();
        displayItem();
        showSlotColor();
    }

    private void previousPage()
    {
        current_page--;
        current_index -= slot_count;
        updatePage();
        refreshItemStatus();
        displayItem();
        showSlotColor();
    }

    public void openPanel()
    {
        //play sound
        panel_enabled = true;
        inventory_item_modification_panel.SendMessage(eventName.system.hide_gui);

        current_index = 0;
        setDefaultLayout();
        refreshItemStatus();
        displayItem();
    }

    public void closePanel()
    {
        aud.clip = Resources.Load<AudioClip>(audioResource.electrical_out);
        aud.Play();

        GetComponent<menuManager>().disableMiddleTab();//make the top tab goes back to previous state...
        panel_enabled = state_control = is_modifying_item = is_combining_item = false;

        ODMObject.confirmation_panel.GetComponent<confirmPanel>().quitPanel();
        item_modification_fsm.SendEvent(eventName.hide_menu);

        setDefaultLayout();
        resetVariables();
        refreshItemStatus();
        displayItem();
        showSlotColor();
    }

    public void setDefaultLayout()
    {
        inventory_description_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, false);
        inventory_modification_description_area.GetComponent<Animator>().SetBool(ODMVariable.animation.is_open, false);
    }

    #endregion

    #region Display
    private void updatePage()
    {
        txtPageCount.GetComponent<Text>().text = current_page + "/" + page_max;
    }
    public void refreshItemStatus()
    {
        for (int i = 0; i < slot_count; i++)
        {
            item item = inventory_collection[(current_page - 1) * slot_count + i];

            if (item.id == -1)
            {
                GameObject itemImage = slot_collection[i].transform.GetChild(0).gameObject;
                setAlphaZero(ref itemImage);
                slot_collection[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "";
                slot_collection[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            else
            {
                GameObject itemImage = slot_collection[i].transform.GetChild(0).gameObject;
                itemImage.GetComponent<Image>().sprite = item.sprite;
                setAlphaOne(ref itemImage);
                if (item.stackable)
                {
                    slot_collection[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.amount.ToString();
                }
                else
                {
                    slot_collection[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "";
                }
                slot_collection[i].transform.GetChild(1).GetComponent<Text>().text = item.shortName;
            }
        }
    }
    private void displayItem()
    {
        //setting slot background to selected

        if (current_index == -1 || inventory_collection[current_index].id == -1)
        {
            showNoItem();
        }
        else
        {
            showItem();
        }
        showSlotColor();
    }

    private void showSlotColor()
    {
        for (int i = 0; i < slot_count; i++)
        {
            if (convertToSlotIndex() == i)
            {
                slot_collection[i].GetComponent<Animator>().SetBool(ODMVariable.animation.is_selected, true);
                slot_collection[i].GetComponent<Animator>().SetBool(ODMVariable.animation.is_selectable, true);
            }
            else
            {
                if (current_index != -1 && selected_combine_item_a != null)
                {
                    if (inventory_collection[(current_page - 1) * slot_count + i % slot_count].id == selected_combine_item_a.id)
                    {
                        slot_collection[i].GetComponent<Animator>().SetBool(ODMVariable.animation.is_selectable, false);
                    }
                    else
                    {
                        slot_collection[i].GetComponent<Animator>().SetBool(ODMVariable.animation.is_selectable, true);
                    }
                }
                slot_collection[i].GetComponent<Animator>().SetBool(ODMVariable.animation.is_selected, false);
            }
        }
    }

    private void showNoItem()
    {
        //Show hint
        text_press_to_use.GetComponent<CanvasGroup>().alpha = 1;

        if (is_combining_item)
        {
            //Panel switch
            inventory_modification_display_panel.GetComponent<CanvasGroup>().alpha = 0;
            inventory_combination_display_panel.GetComponent<CanvasGroup>().alpha = 1;

            //Show image
            inventory_selected_item_b.GetComponent<Image>().sprite = no_item_image;

        }
        else
        {
            //Panel switch
            inventory_modification_display_panel.GetComponent<CanvasGroup>().alpha = 1;
            inventory_combination_display_panel.GetComponent<CanvasGroup>().alpha = 0;

            //Show image
            inventory_no_item_image.GetComponent<CanvasGroup>().alpha = 1;
        }

        inventory_description_area.GetComponent<Menu>().isOpen = false;
        item_title.GetComponent<Text>().text = "";
        text_press_to_use.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void showItem()
    {
        //Show hint
        text_press_to_use.GetComponent<CanvasGroup>().alpha = 1;

        if (is_combining_item)
        {


            //Show image
            inventory_selected_item_b.GetComponent<Image>().sprite = inventory_collection[current_index].sprite;
        }
        else
        {
            //Panel switch
            inventory_modification_display_panel.GetComponent<CanvasGroup>().alpha = 1;
            inventory_combination_display_panel.GetComponent<CanvasGroup>().alpha = 0;

            //Show image
            inventory_no_item_image.GetComponent<CanvasGroup>().alpha = 0;
            inventory_selected_item_main.GetComponent<Image>().sprite = inventory_collection[current_index].sprite;
        }

        //Description Area
        inventory_description_area.GetComponent<Menu>().isOpen = true;
        text_inventory_description.GetComponent<Text>().text = inventory_collection[current_index].description;
        item_title.GetComponent<Text>().text = inventory_collection[current_index].title;
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

    public string getItemArray()
    {
        List<int[]> listItemData = new List<int[]>();
        for (int i = 0; i < inventory_collection.Count; i++)
        {
            if (inventory_collection[i].id != -1)
            {
                listItemData.Add(new int[2] { inventory_collection[i].id, inventory_collection[i].amount });
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
        if (current_index == -1)
            return -1;
        int idx = current_index % slot_count;
        return idx;
    }
    private void resetVariables()
    {
        current_page = 1;
        current_index = -1;
        updatePage();
    }
    #endregion

    //renew code
    public void getItemNameByReturn(string _pack)
    {
        ODM.errorLog(transform.name, "TRIGGERED UNSURE EVENT!");
        //string[] data = _pack.Split(',');//objName, fsmName, docKey
        //item _item = itemDB.getItem(Int32.Parse(data[2].Replace("\r", "")));
        //if (_item != null)
        //{
        //    if (String.IsNullOrEmpty(_item.title))
        //        ODM.errorLog(transform.name,"getDocumentName Error. Key =" + _pack,);
        //    else
        //    {
        //        fsmHelper fsmHelper = new fsmHelper();
        //        PlayMakerFSM f = fsmHelper.getFsm(data[0], data[1]);
        //        f.FsmVariables.GetFsmString("slot item name").Value = _item.title;
        //    }
        //}
    }
}
