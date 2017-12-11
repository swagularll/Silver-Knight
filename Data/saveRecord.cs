using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class saveRecord
{
    public string id { get; set; }
    public ODM.ODMDictionary save_data { get; set; }
    public ODM.ODMDictionary flag_collection { get; set; }
    public diaryLog diary_log { get; set; }
    public ODM.ODMDictionary lair_info_collection { get; set; }
    public List<itemSetting.sceneItemInfo> item_collection { get; set; }

    public saveRecord()
    {
        string str_flag = Resources.Load<TextAsset>(ODMVariable.resource.base_flag_collection).ToString();
        this.id = ODMVariable.common.default_save_code;
        this.save_data = new ODM.ODMDictionary();
        this.flag_collection = new ODM.ODMDictionary(str_flag);
        this.diary_log = new diaryLog();
        this.lair_info_collection = new ODM.ODMDictionary();
        this.item_collection = new List<itemSetting.sceneItemInfo>();
    }
    public saveRecord(string json_string)
    {
        ODM.ODMDictionary dic = new ODM.ODMDictionary(json_string);
        this.id = dic.getValue("id");
        this.save_data = new ODM.ODMDictionary(dic.getValue("save_data"));
        this.flag_collection = new ODM.ODMDictionary(dic.getValue("flag_collection"));
        this.diary_log = new diaryLog(dic.getValue("diary_log"));
        this.lair_info_collection = new ODM.ODMDictionary(dic.getValue("lair_info_collection"));
        this.item_collection = JsonMapper.ToObject<List<itemSetting.sceneItemInfo>>(dic.getValue("item_collection"));
    }

    public saveRecord(string _id, ODM.ODMDictionary _save_data, ODM.ODMDictionary _flag_collection,
        diaryLog _diary_log, ODM.ODMDictionary _lair_info_collection, List<itemSetting.sceneItemInfo> _item_collection)
    {
        this.id = _id;
        this.save_data = _save_data;
        this.flag_collection = _flag_collection;
        this.diary_log = _diary_log;
        this.lair_info_collection = _lair_info_collection;
        this.item_collection = _item_collection;
    }

    public void saveProgress(saveRecord _save)
    {
        //With instance created by given ID
        StreamWriter sw = new StreamWriter(PlayerPrefs.GetString("save_folder_directory") + @"\" + _save.id + ".json", false, Encoding.Default);
        ODM.ODMDictionary dic = new ODM.ODMDictionary();
        dic.add("id", _save.id);
        dic.add("save_data", _save.save_data.getJsonString());
        dic.add("flag_collection", _save.flag_collection.getJsonString());
        dic.add("diary_log", _save.diary_log.getJsonString());
        dic.add("lair_info_collection", _save.lair_info_collection.getJsonString());
        dic.add("item_collection", JsonMapper.ToJson(_save.item_collection));
        sw.Write(dic.getJsonString());
        sw.Close();
    }

    public void updateDiary(diaryLog _diary)
    {
        //Excute partial update
        saveRecord save_record = getCurrentRecord();
        save_record.diary_log = _diary;
        saveProgress(save_record);
    }
    public static saveRecord getCurrentRecord()
    {
        string player_code = ODMVariable.system.save_code;
        if (player_code.Equals(ODMVariable.common.default_save_code))
        {
            saveRecord new_save_record = new saveRecord();
            return new_save_record;
        }
        else
        {
            StreamReader sr = new StreamReader(ODMVariable.path.save_folder_directory + @"\" + player_code + ".json", Encoding.Default);
            //ODM.ODMDictionary dic = JsonMapper.ToObject<ODM.ODMDictionary>(sr.ReadToEnd());
            saveRecord save_record_instance = new saveRecord(sr.ReadToEnd());
            sr.Close();
            return save_record_instance;
        }
    }
}
