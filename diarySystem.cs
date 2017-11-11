using Assets.Script.ODM_Widget;
using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diarySystem : MonoBehaviour
{
    private bool isReady = false;
    public diaryLog diary_log;

    void Start()
    {
        diary_log = new diaryLog();
    }
    void Update()
    {
        //if (isReady)
        //{
        //    diary_log.time_count += Time.deltaTime;
        //}
    }

    public diaryLog getDiary()
    {
        return diary_log;
    }

    #region OUTCALL FUNCTIONS
    public void addResist()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        diary_log.resist.Add(materName);
    }

    public void addFeed()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        diary_log.feed.Add(materName);
    }

    public void addResistFailed()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        diary_log.resist_failed.Add(materName);
    }
    public void addResistSuccess()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        diary_log.resist_success.Add(materName);
    }
    public void addFeedFailed()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        diary_log.feed_failed.Add(materName);
    }
    public void addFeedSuccess()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        diary_log.feed_success.Add(materName);
    }

    public void addDeadCount()
    {
        diary_log.dead_count += 1;
        addKillerName();
    }

    public void addKillerName()
    {
        if (FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value != null)
        {
            GameObject killer = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value;
            string killerName = killer.GetComponent<warmbugAction>().warmbug_name;
            diary_log.killer_name.Add(killerName);
        }
        (new saveRecord()).updateDiary(diary_log);
    }
    #endregion
}
