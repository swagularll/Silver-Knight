using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LitJson;
using Assets.Script.ODM_Widget;
using HutongGames.PlayMaker;

public class warmbugLairManager : MonoBehaviour
{

    private ODM.ODMDictionary warmbug_distribution_collection;//List of lair info is named distribution
    private Dictionary<string, GameObject> catalog_bug_entity_collection;
    void Start()
    {
        warmbug_distribution_collection = saveRecord.getCurrentRecord().lair_info_collection;
        catalog_bug_entity_collection = null;// loadBugsFromSystem;XXX
    }

    public void registerLair(string _level_name, string _level_warmbug_distribution)
    {
        this.warmbug_distribution_collection.add(_level_name, _level_warmbug_distribution);
    }

    public List<lairInfo> getLevelDistribution(string _level_name)
    {
        return JsonMapper.ToObject<List<lairInfo>>(warmbug_distribution_collection.getValue(_level_name));
    }

    public ODM.ODMDictionary getWarmbugDistribution()
    {
        return warmbug_distribution_collection;
    }

    public int getBugCatelogCount()
    {
        return catalog_bug_entity_collection.Count();
    }
    public GameObject getCatelogBugEntity(string _bug_name)
    {
        if (catalog_bug_entity_collection.ContainsKey(_bug_name))
        {
            //Return Bug by given name
            return catalog_bug_entity_collection[_bug_name];
        }
        else
        {
            //If Bug name doesn't match, return random bug
            return catalog_bug_entity_collection.Values.ElementAt((int)UnityEngine.Random.Range(0, 100.0f) % getBugCatelogCount());
        }
    }

    public GameObject getCatelogBugEntity(int _index)
    {
        if (_index > catalog_bug_entity_collection.Count)
            _index = 0;
        return catalog_bug_entity_collection.Values.ElementAt(_index);
    }

    public void setLairInfo(string _level_name, List<lairInfo> _lair_info_collection)
    {
        //Will replace value if key already exists
        warmbug_distribution_collection.add(_level_name, JsonMapper.ToJson(_lair_info_collection.ToString()));
    }

    public void resetAllLevel(bool _flag_value)
    {
        string[] map_name_collection = dataWidget.getMapName();
        for (int i = 0; i < map_name_collection.Length; i++)
        {
            ODMObject.event_manager.GetComponent<eventCenter>().setFlagValue(map_name_collection[i] + " Warmbug Reset", _flag_value);
        }
    }
}
