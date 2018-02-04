
using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diarySystem : MonoBehaviour
{
    private bool is_counting = false;
    private diaryLog diary_log;

    void Start()
    {
        diary_log = saveRecord.getCurrentRecord().diary_log;
    }
    void Update()
    {
        if (is_counting)
        {
            diary_log.time_count += Time.deltaTime;
        }
    }
    public void startCounting()
    {
        is_counting = true;
    }
    public void endCounting()
    {
        is_counting = false;
    }
    public diaryLog getDiary()
    {
        return diary_log;
    }

    #region OUTCALL FUNCTIONS
    public void addResist()
    {
        diary_log.resist.Add(ODMObject.current_mate.GetComponent<warmbugAction>().warmbug_name);
    }

    public void addFeed()
    {
        diary_log.feed.Add(ODMObject.current_mate.GetComponent<warmbugAction>().warmbug_name);
    }

    public void addResistFailed()
    {
        diary_log.resist_failed.Add(ODMObject.current_mate.GetComponent<warmbugAction>().warmbug_name);
    }
    public void addResistSuccess()
    {
        diary_log.resist_success.Add(ODMObject.current_mate.GetComponent<warmbugAction>().warmbug_name);
    }
    public void addFeedFailed()
    {
        diary_log.feed_failed.Add(ODMObject.current_mate.GetComponent<warmbugAction>().warmbug_name);
    }
    public void addFeedSuccess()
    {
        diary_log.feed_success.Add(ODMObject.current_mate.GetComponent<warmbugAction>().warmbug_name);
    }

    public void addDeadCount()
    {
        diary_log.dead_count += 1;
        addKillerName();
    }

    public void addKillerName()
    {
        if (ODMObject.current_mate != null)
        {
            GameObject killer = ODMObject.current_mate;
            string killerName = killer.GetComponent<warmbugAction>().warmbug_name;
            diary_log.killer_name.Add(killerName);
        }
        saveRecord.getCurrentRecord().updateDiary(diary_log);
    }
    #endregion
}
