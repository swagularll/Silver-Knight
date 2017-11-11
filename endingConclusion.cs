using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using LitJson;
using HutongGames.PlayMaker;
using System;

public class endingConclusion : MonoBehaviour
{

    private string game_log_file_name = "odm_game_log.txt";
    private string game_log_file_name_full = "";
    private gameLog dataContent;

    void Start()
    {
        game_log_file_name_full = Application.dataPath + @"\" + game_log_file_name;

        if (!File.Exists(game_log_file_name_full))
        {
            dataContent = new gameLog();
            saveGameLog();
        }
        else
        {
            //Load existing data as class
            StreamReader sr = new StreamReader(game_log_file_name_full, Encoding.Default);
            string jsonString = sr.ReadToEnd();
            dataContent = JsonMapper.ToObject<gameLog>(jsonString);
            sr.Close();
        }
    }


    public void saveGameLog()
    {
        StreamWriter sw = new StreamWriter(game_log_file_name_full, false, Encoding.Default);
        JsonData jsonData = JsonMapper.ToJson(dataContent);
        sw.Write(jsonData.ToString());
        sw.Close();
    }

    public void setEndingCode(string _endingCode)
    {
        //Save current ending record
        dataContent.setEnding(_endingCode);
        saveGameLog();

        //Set ending code translation
        string endingName = FsmVariables.GlobalVariables.GetFsmGameObject("Language Translator").Value.
                    GetComponent<UiInfomationHolder>().getText(_endingCode.Trim());
        PlayerPrefs.SetString("finished_ending", endingName);
    }
    public void getConclusion()
    {
        //diaryLog data = GetComponent<diarySystem>().getDiary();
        ////transform.GetComponent<diary>().endDiary();

        ////finished_ending
        ////Set by ending object

        ////ending_count
        //dataContent.setEndingCount();

        ////gaming_time
        //TimeSpan ts = TimeSpan.FromSeconds(data.time_count);
        //PlayerPrefs.SetString("gaming_time", string.Format("{0:00}:{1:00}:{2:00}", ts.TotalHours, ts.Minutes, ts.Seconds));

        ////exploration_rate
        //string str_exploration_rate = transform.GetComponent<mapDash>().getExlorationRateString();
        //PlayerPrefs.SetString("exploration_rate", str_exploration_rate);

        ////M data
        ////requires translation...
        //PlayerPrefs.SetString("death_count", data.dead_count.ToString());
        //PlayerPrefs.SetString("best_death", data.get_best_death());
        //PlayerPrefs.SetString("best_death_count", data.get_best_death_count().ToString());
        //PlayerPrefs.SetString("mate_count", data.get_mate_count());
        //PlayerPrefs.SetString("best_mate", data.get_best_mate());
        //PlayerPrefs.SetString("best_mate_count", data.get_best_mate_count().ToString());
        //PlayerPrefs.SetString("climax_count", data.get_climax_count().ToString());
        //PlayerPrefs.SetString("best_climax", data.get_best_climax());
        //PlayerPrefs.SetString("best_climax_count", data.get_best_climax_count().ToString());
        //PlayerPrefs.SetString("feed_count", data.get_feed_count().ToString());
        //PlayerPrefs.SetString("best_feed", data.get_best_feed());
        //PlayerPrefs.SetString("best_feed_success", data.get_best_feed_success());
        //PlayerPrefs.SetString("best_feed_failure", data.get_best_feed_failure());
    }

    class gameLog
    {
        public bool clear_easy = false;
        public bool clear_normal = false;
        public bool clear_hard = false;
        public bool clear_hell = false;


        public bool ending_code_maze = false;
        public bool ending_code_inhuman = false;
        public bool ending_code_nightmare = false;
        public bool ending_code_darkness_love = false;
        public bool ending_code_empire = false;
        public bool ending_code_final_call = false;
        public bool ending_code_children = false;
        public bool ending_victoria_talent = false;

        public gameLog()
        {

        }

        public gameLog(string _json)
        {
            if (!String.IsNullOrEmpty(_json))
            {
                gameLog db = JsonMapper.ToObject<gameLog>(_json);
                clear_easy = db.clear_easy;
                clear_normal = db.clear_normal;
                clear_hard = db.clear_hard;
                clear_hell = db.clear_hell;

                ending_code_maze = db.ending_code_maze;
                ending_code_inhuman = db.ending_code_inhuman;
                ending_code_nightmare = db.ending_code_nightmare;
                ending_code_darkness_love = db.ending_code_darkness_love;
                ending_code_empire = db.ending_code_empire;
                ending_code_final_call = db.ending_code_final_call;
                ending_code_children = db.ending_code_children;
                ending_victoria_talent = db.ending_victoria_talent;
            }
        }

        public void setEndingCount()
        {
            int endingSum = 8;
            int endingCount = 0;
            if (ending_code_maze)
                endingCount++;
            if (ending_code_inhuman)
                endingCount++;
            if (ending_code_nightmare)
                endingCount++;
            if (ending_code_darkness_love)
                endingCount++;
            if (ending_code_empire)
                endingCount++;
            if (ending_code_final_call)
                endingCount++;
            if (ending_code_children)
                endingCount++;
            if (ending_victoria_talent)
                endingCount++;
            PlayerPrefs.SetString("ending_count", (endingSum - endingCount).ToString());
        }

        public void setEnding(string _endingCode)
        {
            if (_endingCode.Equals("ending code maze"))
                ending_code_maze = true;
            if (_endingCode.Equals("ending code inhuman"))
                ending_code_inhuman = true;
            if (_endingCode.Equals("ending code nightmare"))
                ending_code_nightmare = true;
            if (_endingCode.Equals("ending code darkness love"))
                ending_code_darkness_love = true;
            if (_endingCode.Equals("ending code empire"))
                ending_code_empire = true;
            if (_endingCode.Equals("ending code final call"))
                ending_code_final_call = true;
            if (_endingCode.Equals("ending code children"))
                ending_code_children = true;
            if (_endingCode.Equals("ending victoria talent"))
                ending_victoria_talent = true;
        }
    }
}
