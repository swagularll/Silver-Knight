using UnityEngine;
using System.Collections;
using System.IO;
using HutongGames.PlayMaker;
using System;
using System.Text;
using LitJson;
using System.Collections.Generic;
using System.Linq;


public class warmbugLair : MonoBehaviour
{
    public bool isActivate;//For testing
    private bool isApproved;
    private bool setLevelRequest;

    public List<GameObject> warmbug_lair_collection;
    private List<lairInfo> level_lair_info_collection;// for save record

    private eventCenter event_center;
    private warmbugLairManager warmbug_lair_manager;

    private string level_name;
    public string reset_flag;
    public string lair_approval_flag;

    private void Awake()
    {
        level_lair_info_collection = new List<lairInfo>();
        level_name = Application.loadedLevelName;
        reset_flag = ODMVariable.convert.getWarmbugResetFlag(level_name);
        lair_approval_flag = ODMVariable.convert.getWarmbugFlag(level_name);
        ODMObject.current_level_lair = transform.gameObject;
    }
    private void Start()
    {
        initializeComponent();

        isApproved = event_center.getFlagBool(lair_approval_flag);
        setLevelRequest = event_center.getFlagBool(reset_flag);
    }

    private void initializeComponent()
    {
        warmbug_lair_manager = ODMObject.event_manager.GetComponent<warmbugLairManager>();
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();

        //Register non-growing lairs
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject lair = transform.GetChild(i).gameObject;
            if (!lair.GetComponent<lairSetting>().isGrowingLair)
                warmbug_lair_collection.Add(lair);
        }
    }
    public void releaseWarmbugs()
    {
        if (isActivate || isApproved)
        {
            if (setLevelRequest)
            {
                generateLairInfo();
                //To avoid double deploy, do this after deploy bugs
                deployBugs();
                event_center.setFlagFalse(reset_flag);
            }
            else
            {
                deployBugs();
            }
            releaseGrowingLairWarmbug();
        }
    }

    /// <summary>
    /// This method will make an instance and add info data to collection
    /// </summary>
    private void releaseGrowingLairWarmbug()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject lair = transform.GetChild(i).gameObject;
            if (lair.GetComponent<lairSetting>().isGrowingLair)
            {
                lair.GetComponent<lairSetting>().initilizeGrowingLair();
            }
        }
    }
    private void generateLairInfo()
    {
        for (int i = 0; i < warmbug_lair_collection.Count; i++)
        {
            //Get Warmbug from catalogue
            int bug_rand_index = (int)UnityEngine.Random.Range(0, 100.0f) % warmbug_lair_manager.getBugCatelogCount();
            GameObject lair_born_bug = warmbug_lair_manager.getCatelogBugEntity(bug_rand_index);
            createBugFromLairInfo(lair_born_bug, warmbug_lair_collection[i]);
        }
    }
    
    public void createBugFromLairInfo(GameObject _lair_born_bug, GameObject _lair_location)
    {
        lairInfo lair_info = new lairInfo();
        _lair_born_bug.GetComponent<warmbugAction>().initilization(this, lair_info);
        lair_info.bug_name = _lair_born_bug.GetComponent<warmbugAction>().getName();
        lair_info.location_x = _lair_location.transform.position.x;
        level_lair_info_collection.Add(lair_info);
    }

    public void addBugLairInfo(lairInfo _lair_info)
    {
        level_lair_info_collection.Add(_lair_info);
        updateBugDistribution();
    }

    //public void createBugFromLivingBug(GameObject _living_bug)
    //{
    //    lairInfo lair_info = new lairInfo();
    //    _living_bug.GetComponent<warmbugAction>().initilization(this, lair_info);
    //    lair_info.bug_name = _living_bug.GetComponent<warmbugAction>().getName();
    //    lair_info.location_x = _living_bug.transform.position.x;
    //    //collection x value
    //    //bug.transform.position = bug_position;//record WB location for reput
    //    level_lair_info_collection.Add(lair_info);
    //    updateBugDistribution();
    //}

    #region WARMBUG SCENE CONTROL

    private void deployBugs()
    {
        for (int i = 0; i < level_lair_info_collection.Count; i++)
        {
            putBug(level_lair_info_collection[i]);
        }
    }
    public void putBug(lairInfo _lair_info)
    {
        GameObject catelog_bug = warmbug_lair_manager.getCatelogBugEntity(_lair_info.bug_name);
        Vector3 bug_position = new Vector3((float)_lair_info.location_x,
            catelog_bug.transform.position.y,
            catelog_bug.transform.position.z);

        //Deploy Warmbug
        GameObject bug = (GameObject)Instantiate(catelog_bug, bug_position, Quaternion.identity);
        bug.GetComponent<warmbugAction>().initilization(this, _lair_info);
    }
    public void setDead(string _warmbugID)
    {
        level_lair_info_collection.RemoveAll(x => x.warmbug_guid.Equals(_warmbugID));
        updateBugDistribution();
    }
    #endregion

    #region DISTRIBUTION MANIPULATION
    public void registerLevelLair()
    {
        generateLairInfo();
    }
    public void loadLevelLair()
    {
        level_lair_info_collection = warmbug_lair_manager.getLevelDistribution(level_name);
    }
    private void updateBugDistribution()
    {
        warmbug_lair_manager.setLairInfo(level_name, level_lair_info_collection);
    }
    #endregion



}