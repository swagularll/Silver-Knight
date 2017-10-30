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
    public bool isActivate;
    private bool isApproved;
    private bool resetRequest;

    public List<GameObject> lair_entity_collection;
    public List<GameObject> living_bug_entity_colleciton;
    private List<ODM.lairInfo> level_lair_info_collection;// for save record

    private eventCenter event_center;
    private string level_name;
    public string reset_flag;
    public string lair_approval_flag;
    private warmbugLairManager lair_manager;

    void Awake()
    {
        event_center = FsmVariables.GlobalVariables.GetFsmGameObject("event manager").Value.GetComponent<eventCenter>();
        lair_manager = event_center.GetComponent<warmbugLairManager>();
        level_lair_info_collection = lair_manager.getLairInfo(level_name);

        level_name = Application.loadedLevelName;
        reset_flag = level_name + " Warmbug Reset";
        lair_approval_flag = level_name + " Warmbug";

        isApproved = event_center.getFlagBool(lair_approval_flag);
        resetRequest = event_center.getFlagBool(reset_flag);
    }

    void Start()
    {
        if (isActivate || isApproved)
        {
            if (level_lair_info_collection.Count == 0)
            {
                generateWarmbug();
            }
            else
            {
                if (resetRequest)
                {
                    generateWarmbug();
                    event_center.setFlagFalse(reset_flag);
                }
                else
                {
                    deployExistWarmbug();
                }
            }
        }
    }

    private void generateWarmbug()
    {
        level_lair_info_collection.Clear();
        addNewBugs();
        setLivingBugs();
        recordLairLog();
        //XXX: requires to modify WB action (SOLO sort)


        // lairInfoCollection.Clear();
        // float sp = FsmVariables.GlobalVariables.GetFsmFloat("ava_current_sp").Value;
        //if (sp == 100)
        //{
        //    addNewBug(5);
        //}
        // if (sp > 80)
        // {
        //     addNewBug(4);
        // }
        // else if (sp > 50)
        // {
        //     addNewBug(3);
        // }
        // else if (sp > 20)
        // {
        //     addNewBug(2);
        // }
        // else
        // {
        //     addNewBug(1);
        // }
    }

    #region Add Warmbugs to scene
    private void addNewBugs()
    {
        for (int i = 0; i < lair_entity_collection.Count; i++)
        {
            //Get Warmbug
            int bug_rand_index = (int)UnityEngine.Random.Range(0, 100.0f) % lair_manager.getBugCount();
            GameObject lair_born_bug = lair_manager.getBugEntity(bug_rand_index);

            //Deploy Warmbug
            Vector3 bug_position = new Vector3(lair_entity_collection[i].transform.position.x,
            lair_born_bug.transform.position.y,
            lair_born_bug.transform.position.z);
            Instantiate(lair_born_bug, bug_position, Quaternion.identity);

            //Generate lairInfo
            ODM.lairInfo lair_info = new ODM.lairInfo();
            lair_born_bug.GetComponent<warmbugAction>().initilization(this, lair_info, false);
            lair_info.bug_name = lair_born_bug.GetComponent<warmbugAction>().getName();
            lair_info.location_x = lair_born_bug.transform.position.x;//collection x value
            // bug.transform.position = bug_position;//record WB location for reput
            level_lair_info_collection.Add(lair_info);
        }
    }

    private void setLivingBugs()
    {
        for (int i = 0; i < living_bug_entity_colleciton.Count; i++)
        {
            //Get Warmbug
            GameObject living_bug = living_bug_entity_colleciton[i];
            //Initilize Warmbug (No need to deploy)

            //Generate lairInfo
            ODM.lairInfo lair_info = new ODM.lairInfo();
            living_bug.GetComponent<warmbugAction>().initilization(this, lair_info, true);//send start command to action
            lair_info.bug_name = living_bug.GetComponent<warmbugAction>().getName();
            lair_info.location_x = living_bug.transform.position.x;//Set WB position according to scene view
            level_lair_info_collection.Add(lair_info);
        }
    }

    private void putBug(ODM.lairInfo _lair_info)
    {
        //do concern that index might be larger than collection count. 
        //GameObject bug = Instantiate(warmbugTypeCollection[_info.warmbugIndex]);
        GameObject rebirth_bug = lair_manager.getBugEntity(_lair_info.bug_name);
        Vector3 bug_position = new Vector3((float)_lair_info.location_x,
            rebirth_bug.transform.position.y,
            rebirth_bug.transform.position.z);

        //Deploy Warmbug
        GameObject bug = (GameObject)Instantiate(rebirth_bug, bug_position, Quaternion.identity);
        bug.GetComponent<warmbugAction>().initilization(this, _lair_info, false);
    }
    #endregion

    #region data manipulation methods
    private void recordLairLog()
    {
        lair_manager.setLairInfo(level_name, level_lair_info_collection);
        // StreamWriter sw = new StreamWriter(lairLevelFile, false, Encoding.Default);
        // sw.Write(JsonMapper.ToJson(lairInfoCollection));
        // sw.Close();
    }

    // private void addToLair(GameObject _item, List<GameObject> _itemCollection)
    // {
    //     if (_item != null)
    //     {
    //         _itemCollection.Add(_item);
    //     }
    // }
    private void deployExistWarmbug()
    {
        // StreamReader sr = new StreamReader(lairLevelFile, Encoding.Default);
        // lairInfoCollection = JsonMapper.ToObject<List<lairInfo>>(sr.ReadToEnd());
        // sr.Close();
        // lair_info_collection = event_center.lair_info_collection.getValue(level_name);

        for (int i = 0; i < level_lair_info_collection.Count; i++)
        {
            putBug(level_lair_info_collection[i]);
        }
        //HP will randomly generated by FSM Health System
    }

    public void setDead(string _warmbugID)
    {
        var result = level_lair_info_collection.Where(x => x.warmbug_guid.Equals(_warmbugID));
        level_lair_info_collection.RemoveAll(x => x.warmbug_guid.Equals(_warmbugID));
        recordLairLog();
        //string debugFile = warmbugDeploymentFilePath + "/" + "Lair Log.txt";
        //StreamWriter sw = new StreamWriter(debugFile, true, Encoding.Default);
        //sw.WriteLine("Current Warmbug = " + _warmbugID);
        //sw.WriteLine("Search Result = " + result);
        //sw.WriteLine(JsonMapper.ToJson(lairInfoCollection));
        //sw.WriteLine("-----------------------------------");
        //sw.Close();
    }
    #endregion

}