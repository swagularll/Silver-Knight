using Assets.Script.ODM_Widget;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class itemManager : MonoBehaviour
{
    //Scene drag item 
    
    public List<GameObject> item_entity_collection;

    private List<itemInfo> item_distribution;
    private Dictionary<inventoryDash.inventoryItem, GameObject> item_entity_dictionary;

    private void Awake()
    {
        item_distribution = new List<itemInfo>();
        item_entity_dictionary = new Dictionary<inventoryDash.inventoryItem, GameObject>();
        loadItemEntityDictionary();
    }
    public void loadItemEntityDictionary()
    {
        foreach (var item in item_entity_collection)
        {
            item_entity_dictionary.Add(item.GetComponent<itemSetting>().catalog_code, item);
        }
    }
    public void registerItem(string _level_name, GameObject _item_entity)
    {
        itemInfo item_info = new itemInfo();
        item_info.item_guid = (new Guid()).ToString();
        item_info.catalog_code = _item_entity.GetComponent<itemSetting>().catalog_code;
        item_info.located_level = _level_name;
        item_info.location_x = _item_entity.transform.position.x;
        item_info.location_y = _item_entity.transform.position.y;
        item_info.location_z = _item_entity.transform.position.z;
        item_distribution.Add(item_info);
    }

    //public void deployItem(string _item_guid)
    //{

    //}
    public void deployLevelItems(string _level_name)
    {
        List<itemInfo> level_item_distribution = item_distribution.Where(x => x.located_level.Equals(_level_name.Trim())).ToList();
        for (int i = 0; i < level_item_distribution.Count; i++)
        {
            itemInfo item_info = level_item_distribution[i];
            GameObject item_entity = item_entity_dictionary[item_info.catalog_code];
            Vector3 deploy_location = new Vector3(item_info.location_x, item_info.location_y, item_info.location_z);
            GameObject scene_item = (GameObject)Instantiate(item_entity, deploy_location, Quaternion.identity);
            scene_item.GetComponent<itemSetting>().initilaization();
        }
    }
    public List<itemInfo> getItemDistribution()
    {
        return item_distribution;
    }

    public class itemInfo
    {
        public string item_guid { get; set; }
        public inventoryDash.inventoryItem catalog_code { get; set; }
        public string located_level { get; set; }
        public float location_x { get; set; }
        public float location_y { get; set; }
        public float location_z { get; set; }

        public itemInfo()
        {
        }

        public itemInfo(string _json)
        {
            itemInfo item_info = JsonMapper.ToObject<itemInfo>(_json);
            this.catalog_code = item_info.catalog_code;
            this.location_x = item_info.location_x;
            this.location_y = item_info.location_y;
            this.location_z = item_info.location_z;
        }
        public string getJsonString()
        {
            JsonData dataJson = JsonMapper.ToJson(this);
            return dataJson.ToString();
        }
    }



}
