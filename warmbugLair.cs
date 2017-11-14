using UnityEngine;
using System.Collections;
using System.IO;
using HutongGames.PlayMaker;
using System;
using System.Text;
using LitJson;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.ODM_Widget;

public class warmbugLair : MonoBehaviour
{
    public bool isActivate;
    private bool isApproved;
    private bool resetRequest;

    public List<GameObject> lair_entity_collection;
    public List<GameObject> living_bug_entity_colleciton = new List<GameObject>();


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
        reset_flag = level_name + " Warmbug Reset";
        lair_approval_flag = level_name + " Warmbug";
    }
    private void InitialzeScript()
    {
        warmbug_lair_manager = ODMObject.event_manager.GetComponent<warmbugLairManager>();
        event_center = ODMObject.event_manager.GetComponent<eventCenter>();
    }

    private void Start()
    {
        InitialzeScript();

        isApproved = event_center.getFlagBool(lair_approval_flag);
        resetRequest = event_center.getFlagBool(reset_flag);
    }


    private void generateLairInfo()
    {
        //For empty lairs
        for (int i = 0; i < lair_entity_collection.Count; i++)
        {
            //Get Warmbug from catalogue
            int bug_rand_index = (int)UnityEngine.Random.Range(0, 100.0f) % warmbug_lair_manager.getBugCatelogCount();
            GameObject lair_born_bug = warmbug_lair_manager.getCatelogBugEntity(bug_rand_index);

            createToLairCollection(lair_born_bug);
        }

        //For living bugs
        for (int i = 0; i < living_bug_entity_colleciton.Count; i++)
        {
            createToLairCollection(living_bug_entity_colleciton[i]);
        }
    }

    public void createToLairCollection(GameObject _lair_born_bug)
    {
        lairInfo lair_info = new lairInfo();
        _lair_born_bug.GetComponent<warmbugAction>().initilization(this, lair_info, false);
        lair_info.bug_name = _lair_born_bug.GetComponent<warmbugAction>().getName();
        lair_info.location_x = _lair_born_bug.transform.position.x;
        //collection x value
        //bug.transform.position = bug_position;//record WB location for reput
        level_lair_info_collection.Add(lair_info);
    }

    public void releaseWarmbugs()
    {
        if (isActivate || isApproved)
        {
            if (resetRequest)
            {
                generateLairInfo();
                deployBugs();
                event_center.setFlagFalse(reset_flag);
            }
            else
            {
                deployBugs();
            }
        }
    }

    #region WARMBUG SCENE CONTROL

    private void deployBugs()
    {
        for (int i = 0; i < level_lair_info_collection.Count; i++)
        {
            putBug(level_lair_info_collection[i]);
        }
    }
    private void putBug(lairInfo _lair_info)
    {
        GameObject catelog_bug = Instantiate(warmbug_lair_manager.getCatelogBugEntity(_lair_info.bug_name));
        Vector3 bug_position = new Vector3((float)_lair_info.location_x,
            catelog_bug.transform.position.y,
            catelog_bug.transform.position.z);

        //Deploy Warmbug
        GameObject bug = (GameObject)Instantiate(catelog_bug, bug_position, Quaternion.identity);
        bug.GetComponent<warmbugAction>().initilization(this, _lair_info, false);
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
        warmbug_lair_manager.registerLair(level_name, JsonMapper.ToJson(living_bug_entity_colleciton).ToString());
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