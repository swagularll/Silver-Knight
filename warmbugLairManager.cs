using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LitJson;

public class warmbugLairManager : MonoBehaviour {

    private ODM.ODMDictionary lair_info_collection;
    private Dictionary<string, GameObject> bug_entity_collection;
    void Start()
    {
        lair_info_collection = ODM.getCurrentRecord().lair_info_collection;
        bug_entity_collection = null;// loadBugsFromSystem;
    }

    public List<ODM.lairInfo> getLairInfo(string _level_name)
    {
        return JsonMapper.ToObject<List<ODM.lairInfo>>(lair_info_collection.getValue(_level_name));
    }

    public int getBugCount()
    {
        return bug_entity_collection.Count();
    }
    public GameObject getBugEntity(string _bug_name)
    {
        return bug_entity_collection[_bug_name];
    }

    public GameObject getBugEntity(int _index)
    {
        if (_index > bug_entity_collection.Count)
            _index = 0;
        return bug_entity_collection.Values.ElementAt(_index);
    }

    public void setLairInfo(string _level_name, List<ODM.lairInfo> _lair_info_collection)
    {
        lair_info_collection.setValue(_level_name, JsonMapper.ToJson(_lair_info_collection.ToString()));
    }
}
