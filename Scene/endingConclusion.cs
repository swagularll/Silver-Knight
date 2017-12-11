using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using LitJson;
using HutongGames.PlayMaker;
using System;

public class endingConclusion : MonoBehaviour
{
    /// <summary>
    /// This script concludes all endings that a player has completed.
    /// </summary>
    private gameLog data_body;
    void Start()
    {
        //Load existe
        if (!File.Exists(ODMVariable.path.game_log_file))
        {
            data_body = new gameLog();
            saveGameLog();
        }
        else
        {
            //Load existing data as class
            data_body = JsonMapper.ToObject<gameLog>(ODM.read(ODMVariable.path.game_log_file));
        }
    }

    public void saveGameLog()
    {
        ODM.writeToJson(ODMVariable.path.game_log_file, data_body);
    }

    public void setEndingCode(string _ending_code)
    {
        data_body.setEnding(_ending_code);
        saveGameLog();

        //Set ending code translation
        string ending_name = ODMObject.language_translator.GetComponent<UiInfomationHolder>().getText(_ending_code.Trim());
        ODMVariable.endingText.finished_ending = ending_name;
    }
    public void getConclusion()//Method is called by ending object
    {
        diaryLog data = ODMObject.event_manager.GetComponent<diarySystem>().getDiary();

        //Stop time count down
        ODMObject.event_manager.GetComponent<diarySystem>().endCounting();

        //ending_count
        this.data_body.setEndingCount();

        //gaming_time
        TimeSpan ts = TimeSpan.FromSeconds(data.time_count);
        ODMVariable.endingText.gaming_time = string.Format("{0:00}:{1:00}:{2:00}", ts.TotalHours, ts.Minutes, ts.Seconds);

        //exploration_rate
        ODMVariable.endingText.exploration_rate = transform.GetComponent<mapDash>().getExlorationRateString();

        //M data
        ODMVariable.endingText.death_count = data.dead_count.ToString();
        ODMVariable.endingText.best_death = data.get_best_death();
        ODMVariable.endingText.best_death_count = data.get_best_death_count().ToString();
        ODMVariable.endingText.mate_count = data.get_mate_count();
        ODMVariable.endingText.best_mate = data.get_best_mate();
        ODMVariable.endingText.best_mate_count = data.get_best_mate_count().ToString();
        ODMVariable.endingText.climax_count = data.get_climax_count().ToString();
        ODMVariable.endingText.best_climax = data.get_best_climax();
        ODMVariable.endingText.best_climax_count = data.get_best_climax_count().ToString();
        ODMVariable.endingText.feed_count = data.get_feed_count().ToString();
        ODMVariable.endingText.best_feed = data.get_best_feed();
        ODMVariable.endingText.best_feed_success = data.get_best_feed_success();
        ODMVariable.endingText.best_feed_failure = data.get_best_feed_failure();
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
            ODMVariable.ending.ending_count = endingSum - endingCount;
        }

        public void setEnding(string _ending_code)
        {
            if (_ending_code.Equals(ODMVariable.ending.ending_code_maze))
                ending_code_maze = true;
            if (_ending_code.Equals(ODMVariable.ending.ending_code_inhuman))
                ending_code_inhuman = true;
            if (_ending_code.Equals(ODMVariable.ending.ending_code_nightmare))
                ending_code_nightmare = true;
            if (_ending_code.Equals(ODMVariable.ending.ending_code_darkness_love))
                ending_code_darkness_love = true;
            if (_ending_code.Equals(ODMVariable.ending.ending_code_empire))
                ending_code_empire = true;
            if (_ending_code.Equals(ODMVariable.ending.ending_code_final_call))
                ending_code_final_call = true;
            if (_ending_code.Equals(ODMVariable.ending.ending_code_children))
                ending_code_children = true;
            if (_ending_code.Equals(ODMVariable.ending.ending_victoria_talent))
                ending_victoria_talent = true;
            ODMVariable.ending.ending_code = _ending_code;
        }
    }
}
