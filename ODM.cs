using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using LitJson;
using System.Linq;
using HutongGames.PlayMaker;
using System;
using System.Text;
using System.Security.Cryptography;

public class ODM : MonoBehaviour
{
    #region TEXT MANUPULATION
    public static string[] getMapName()
    {
        int idx = 0;
        string[] mapNames = new string[60];
        string[] s = { "A", "B", "C", "D", "E", "F" };
        for (int i = 0; i < 10; i++)
        {
            for (int k = 0; k < 6; k++)
            {
                mapNames[idx] = s[k] + i;
                idx++;
            }
        }
        return mapNames;
    }
    public string getTranslaton(string key)
    {
        return FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value.GetComponent<UiInfomationHolder>().getText(key);
    }
    public string getTranslaton(string key, string _subjectName)
    {
        return FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value.
            GetComponent<UiInfomationHolder>().getText(key).Replace("[INPUT]", _subjectName);
    }

    public static string getDifficultyText(int _difficulty)
    {
        string difficulty_text = "";
        switch (_difficulty)
        {
            case 1:
                difficulty_text = "Easy";
                break;
            case 2:
                difficulty_text = "Normal";
                break;
            case 3:
                difficulty_text = "Hard";
                break;
            case 4:
                difficulty_text = "Hell";
                break;
            default:
                ODM.errorLog("Static", "No difficulty data!", "");
                break;
        }
        return difficulty_text;
    }
    #endregion

    #region LOG SYSTEM
    public static void log(string _holder, string _message)
    {
#if DEBUG
        string context = "";
        if (!String.IsNullOrEmpty(_message))
            context += "[Message]" + _message + Environment.NewLine;
        if (!String.IsNullOrEmpty(_holder))
            context += "[Holder]" + _holder + Environment.NewLine;
        Debug.Log(context);
#endif
    }

    public static void errorLog(string _holder,string _message, string _system_message)
    {
#if DEBUG
        string context = "";
        if (!String.IsNullOrEmpty(_system_message))
            context += "[Error]" + _system_message + Environment.NewLine;
        if (!String.IsNullOrEmpty(_holder))
            context += "[Holder]" + _holder + Environment.NewLine;
        if (!String.IsNullOrEmpty(_message))
            context += "[Display Message]" + _message + Environment.NewLine;
        Debug.LogError(context);
#endif
    }
    #endregion

    #region PROJECT DATA CLASS

    public enum inventoryItem
    {

    }

    public class systemConfiguration
    {
        public string player_nick_name { get; set; }
        public string web_base { get; set; }
        public string service_base { get; set; }
        public string player_guid { get; set; }
        public string mac { get; set; }
        public string player_email { get; set; }
        public string service_email { get; set; }
        public string warmbug_lock { get; set; }

        public systemConfiguration()
        { }
        public systemConfiguration(string _json)
        {
            systemConfiguration system_configuration = JsonMapper.ToObject<systemConfiguration>(_json);
            this.player_nick_name = system_configuration.player_nick_name;
            this.web_base = system_configuration.web_base;
            this.service_base = system_configuration.service_base;
            this.player_guid = system_configuration.player_guid;
            this.mac = system_configuration.mac;
            this.player_email = system_configuration.player_email;
            this.service_email = system_configuration.service_email;
            this.warmbug_lock = system_configuration.warmbug_lock;
        }
    }

    public class saveRecord
    {
        public string id { get; set; }
        public ODMDictionary save_data { get; set; }
        public ODMDictionary flag_collection { get; set; }
        public diaryLog diary_log { get; set; }
        public ODMDictionary lair_info_collection { get; set; }
        public saveRecord()
        {
            string str_flag = Resources.Load<TextAsset>(PlayerPrefs.GetString("base_flag_collection")).ToString();
            this.id = "default";
            this.save_data = new ODMDictionary();
            this.flag_collection = new ODMDictionary(str_flag); 
            this.diary_log = new diaryLog();
            this.lair_info_collection = new ODMDictionary();
        }
        public saveRecord(string json_string)
        {
            saveRecord save_record = JsonMapper.ToObject<saveRecord>(json_string);
            this.id = save_record.id;
            this.save_data = save_record.save_data;
            this.flag_collection = save_record.flag_collection;
            this.diary_log = save_record.diary_log;
            this.lair_info_collection = save_record.lair_info_collection;
        }

        public saveRecord(string _id, ODMDictionary _save_data, ODMDictionary _flag_collection, diaryLog _diary_log)
        {
            this.id = _id;
            this.save_data = _save_data;
            this.flag_collection = _flag_collection;
            diary_log = _diary_log;
        }

        public void saveProgress(ODM.saveRecord _save)
        {
            //With instance created by given ID
            StreamWriter sw = new StreamWriter(PlayerPrefs.GetString("save_folder_directory") + @"\" + _save.id + ".json", false, Encoding.Default);
            sw.Write(JsonMapper.ToJson(_save).ToString());
            sw.Close();
        }

        public void updateDiary(ODM.diaryLog _diary)
        {
            //Excute partial update
            saveRecord save_record = getCurrentRecord();
            save_record.diary_log = _diary;
            saveProgress(save_record);
        }
    }
    public static saveRecord getCurrentRecord()
    {
        string player_code = PlayerPrefs.GetString("save_code");
        if (player_code.Equals("default"))
        {
            saveRecord new_save_record = new saveRecord();
            return new_save_record;
        }
        else
        {
            StreamReader sr = new StreamReader(PlayerPrefs.GetString("save_folder_directory") + @"\" + player_code + ".json", Encoding.Default);
            saveRecord save_record_instance = JsonMapper.ToObject<saveRecord>(sr.ReadToEnd());
            sr.Close();
            return save_record_instance;
        }
    }
    public class diaryLog
    {
        //Slots
        public double time_count = 0f;
        public int dead_count = 0;

        //Real data body
        public List<string> resist = new List<string>();
        public List<string> feed = new List<string>();
        public List<string> resist_failed = new List<string>();
        public List<string> resist_success = new List<string>();
        public List<string> feed_failed = new List<string>();
        public List<string> feed_success = new List<string>();
        public List<string> killer_name = new List<string>();

        public diaryLog()
        {

        }
        public diaryLog(string _json)
        {
            if (!String.IsNullOrEmpty(_json))
            {
                //Create an instance
                diaryLog db = JsonMapper.ToObject<diaryLog>(_json);
                //Map data from the instance
                this.time_count = db.time_count;
                this.dead_count = db.dead_count;
                this.resist = db.resist;
                this.feed = db.feed;
                this.resist_success = db.resist_success;
                this.feed_failed = db.feed_failed;
                this.feed_success = db.feed_success;
                this.killer_name = db.killer_name;
            }
        }
        public string getString()
        {
            JsonData jsonData = JsonMapper.ToJson(this);
            return jsonData.ToString();
        }

        #region LINQ
        public string get_best_death()
        {
            string killer = "-";
            if (killer_name.Count > 0)
            {
                killer = (from i in killer_name
                          group i by i into grp
                          orderby grp.Count() descending
                          select grp.Key).First();
                killer = (new ODM()).getTranslaton(killer);
            }
            return killer;
        }
        public int get_best_death_count()
        {
            int best_death_count = 0;
            if (killer_name.Count > 0)
            {
                best_death_count = (from i in killer_name
                                    group i by i into grp
                                    orderby grp.Count() descending
                                    select grp.Key).Count();
            }
            return best_death_count;
        }
        public string get_mate_count()
        {
            return resist.Count.ToString();
        }
        public string get_best_mate()
        {
            string mate = "-";
            if (resist.Count > 0)
            {
                mate = (from i in resist
                        group i by i into grp
                        orderby grp.Count() descending
                        select grp.Key).First();
                mate = (new ODM()).getTranslaton(mate);
            }
            return mate;
        }
        public int get_best_mate_count()
        {
            int mate_count = 0;
            if (resist.Count > 0)
            {
                mate_count = (from i in resist
                              group i by i into grp
                              orderby grp.Count() descending
                              select grp.Key).Count();
            }
            return mate_count;
        }
        public int get_climax_count()
        {
            return resist_failed.Count();
        }
        public string get_best_climax()
        {
            string get_climax_mate = "-";
            if (resist_failed.Count > 0)
            {
                get_climax_mate = (from i in resist_failed
                                   group i by i into grp
                                   orderby grp.Count() descending
                                   select grp.Key).FirstOrDefault();
                get_climax_mate = (new ODM()).getTranslaton(get_climax_mate);
            }
            return get_climax_mate;
        }
        public int get_best_climax_count()
        {
            int climax_count = 0;
            if (resist_failed.Count > 0)
            {
                climax_count = (from i in resist_failed
                                group i by i into grp
                                orderby grp.Count() descending
                                select grp.Key).Count();
            }
            return climax_count;
        }
        public int get_feed_count()
        {
            return feed.Count;
        }
        public string get_best_feed()
        {
            string best_feed_mate = "-";
            if (feed.Count > 0)
            {
                best_feed_mate = (from i in feed
                                  group i by i into grp
                                  orderby grp.Count() descending
                                  select grp.Key).First();
                best_feed_mate = (new ODM()).getTranslaton(best_feed_mate);
            }
            return best_feed_mate;
        }
        public string get_best_feed_success()
        {
            string best_feed_success_mate = "-";
            if (feed_success.Count > 0)
            {
                best_feed_success_mate = (from i in feed_success
                                          group i by i into grp
                                          orderby grp.Count() descending
                                          select grp.Key).First();
                best_feed_success_mate = (new ODM()).getTranslaton(best_feed_success_mate);
            }
            return best_feed_success_mate;
        }
        public string get_best_feed_failure()
        {
            string best_feed_failure_mate = "-";
            if (feed_failed.Count > 0)
            {
                best_feed_failure_mate = (from i in feed_failed
                                          group i by i into grp
                                          orderby grp.Count() descending
                                          select grp.Key).First();
                best_feed_failure_mate = (new ODM()).getTranslaton(best_feed_failure_mate);
            }
            return best_feed_failure_mate;
        }
        #endregion
    }
    public class ODMDictionary
    {
        private List<data> data_body = new List<data>();
        public ODMDictionary()
        {
        }

        public ODMDictionary(string json)
        {
            data_body = JsonMapper.ToObject<List<data>>(json);
        }
        public void add(string key, string value)
        {
            data d = new data(key, value);
            data_body.Add(d);
        }
        public void clear()
        {
            data_body.Clear();
        }

        public string getValue(string _key)
        {
            return data_body.Where(x => x.key == _key).FirstOrDefault().value;
        }
        public void setValue(string _key, string _value)
        {
            data_body.Where(x => x.key == _key).FirstOrDefault().value = _value;
        }

        public string getJsonString()//Important: right way to avoid serialization problem
        {
            JsonData dataJson = JsonMapper.ToJson(data_body);
            return dataJson.ToString();
        }

        public class data
        {
            public string key = "";
            public string value = "";
            public data(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
            public data()
            {
            }
        }
    }
    public class PlayerPrefBool
    {
        public static void SetBool(string name, bool booleanValue)
        {
            PlayerPrefs.SetInt(name, booleanValue ? 1 : 0);
        }
        public static bool GetBool(string name)
        {
            return PlayerPrefs.GetInt(name) == 1 ? true : false;
        }
        public static bool GetBool(string name, bool defaultValue)
        {
            if (PlayerPrefs.HasKey(name))
            {
                return GetBool(name);
            }
            return defaultValue;
        }
    }
    #endregion

    #region WARMBUG
    public class lairInfo
    {
        public string warmbug_guid;
        public string bug_name { get; set; }
        public double location_x { get; set; }

        public double easyMagnification = 0.5f;
        public double normalMagnification = Math.Round(UnityEngine.Random.Range(0.8f, 1.2f), 2);
        public double hardMagnification = Math.Round(UnityEngine.Random.Range(1.3f, 1.7f), 2);
        public double hellMagnification = Math.Round(UnityEngine.Random.Range(2.5f, 2.9f), 2);
        public lairInfo()
        {
            Guid g = Guid.NewGuid();
            warmbug_guid = g.ToString();
        }
    }



    #endregion

    #region SYSTEM
    static string key = "WarmbugFuckAvaLibertyveryhard666";

    public static string encryption(string _context)
    {
        string result = "";

        try
        {
            byte[] KeyArray = UTF8Encoding.UTF8.GetBytes(key);
            RijndaelManaged rm = new RijndaelManaged();
            rm.Key = KeyArray;
            rm.Mode = CipherMode.ECB;
            rm.Padding = PaddingMode.PKCS7;
            ICryptoTransform ct = rm.CreateEncryptor();

            byte[] targetContent = UTF8Encoding.UTF8.GetBytes(_context);
            byte[] resultArray = ct.TransformFinalBlock(targetContent, 0, targetContent.Length);
            result = Convert.ToBase64String(resultArray);
        }
        catch (Exception ex)
        {
            Debug.LogError("Format error.");
        }
        return result;
    }
    
    public static string decryption(string _context)
    {
        string result = "";
        try
        {
            byte[] KeyArray = UTF8Encoding.UTF8.GetBytes(key);

            RijndaelManaged rm = new RijndaelManaged();
            rm.Key = KeyArray;
            rm.Mode = CipherMode.ECB;
            rm.Padding = PaddingMode.PKCS7;
            ICryptoTransform ct = rm.CreateDecryptor();

            byte[] txtContent = Convert.FromBase64String(_context);
            byte[] txtResult = ct.TransformFinalBlock(txtContent, 0, txtContent.Length);
            result = UTF8Encoding.UTF8.GetString(txtResult);
        }
        catch (Exception ex)
        {
            Debug.LogError(_context);
            Debug.LogError("Format error.");
        }

        return result;
    }
    #endregion
}
