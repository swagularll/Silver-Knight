
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class itemManager : MonoBehaviour
{
    //Scene drag item 

    public List<GameObject> item_entity_collection;

    private List<itemSetting.sceneItemInfo> item_distribution;
    private Dictionary<string, GameObject> item_entity_dictionary;

    #region Initialization
    private void Awake()
    {
        item_distribution = new List<itemSetting.sceneItemInfo>();
        item_entity_dictionary = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
        loadItemEntityDictionary();
    }
    public void loadItemEntityDictionary()
    {
        for (int i = 0; i < item_entity_collection.Count; i++)
        {
            item_entity_dictionary.Add(((int)item_entity_collection[i].GetComponent<itemSetting>().catalog_code).ToString(), item_entity_collection[i]);
        }
    }
    #endregion

    #region Accessibility
    public GameObject getItemEntity(string _catalog_code)
    {
        GameObject item_entity;
        if (item_entity_dictionary.TryGetValue(_catalog_code, out item_entity)) // Returns true.
            return item_entity_dictionary[_catalog_code];
        return null;
    }
    #endregion

    #region Data manipulation

    public void registerItem(GameObject _item_entity)
    {
        item_distribution.Add(_item_entity.GetComponent<itemSetting>().item_info);

    }
    public void removeItemRegistration(string _guid)
    {
        item_distribution.RemoveAll(x => x.item_guid.Equals(_guid));
    }
    #endregion

    public void deployLevelItems(string _level_name)
    {
        List<itemSetting.sceneItemInfo> level_item_distribution = item_distribution.Where(x => x.located_level.Equals(_level_name.Trim())).ToList();
        for (int i = 0; i < level_item_distribution.Count; i++)
        {
            itemSetting.sceneItemInfo item_info = level_item_distribution[i];
            GameObject item_entity = item_entity_dictionary[((int)item_info.catalog_code).ToString()];
            Vector3 deploy_location = new Vector3((float)item_info.location_x, (float)item_info.location_y, (float)item_info.location_z);
            GameObject scene_item = (GameObject)Instantiate(item_entity, deploy_location, Quaternion.identity);
            scene_item.GetComponent<itemSetting>().item_info.item_guid = item_info.item_guid;
            scene_item.GetComponent<itemSetting>().item_info.amount = item_info.amount;
            scene_item.GetComponent<itemSetting>().initilaization();
        }
    }

    public void createItem(string _item_id, int _amout , float _drop_position_x)
    {
        GameObject item_entity = item_entity_dictionary[_item_id];//item_id = itemCatalogue.ToString();
        Vector3 deploy_location = new Vector3(_drop_position_x, item_entity.transform.position.y, item_entity.transform.position.z);
        GameObject scene_item = (GameObject)Instantiate(item_entity, deploy_location, Quaternion.identity);
        scene_item.GetComponent<itemSetting>().initilaization();
        scene_item.GetComponent<itemSetting>().resetPositionInfo(deploy_location);
        scene_item.GetComponent<itemSetting>().item_amount = _amout;
        registerItem(scene_item);
    }

    public List<itemSetting.sceneItemInfo> getItemDistribution()
    {
        return item_distribution;
    }
}
