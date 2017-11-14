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

    private List<itemSetting.itemInfo> item_distribution;
    private Dictionary<inventoryDash.inventoryItem, GameObject> item_entity_dictionary;

    private void Awake()
    {
        item_distribution = new List<itemSetting.itemInfo>();
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
    public void registerItem(GameObject _item_entity)
    {
        item_distribution.Add(_item_entity.GetComponent<itemSetting>().item_info);

    }

    public void removeItemRegistration(string _guid)
    {
        item_distribution.RemoveAll(x => x.item_guid.Equals(_guid));
    }

    public void deployLevelItems(string _level_name)
    {
        List<itemSetting.itemInfo> level_item_distribution = item_distribution.Where(x => x.located_level.Equals(_level_name.Trim())).ToList();
        for (int i = 0; i < level_item_distribution.Count; i++)
        {
            itemSetting.itemInfo item_info = level_item_distribution[i];
            GameObject item_entity = item_entity_dictionary[item_info.catalog_code];
            Vector3 deploy_location = new Vector3(item_info.location_x, item_info.location_y, item_info.location_z);
            GameObject scene_item = (GameObject)Instantiate(item_entity, deploy_location, Quaternion.identity);
            scene_item.GetComponent<itemSetting>().initilaization();
        }
    }
    public List<itemSetting.itemInfo> getItemDistribution()
    {
        return item_distribution;
    }




}
