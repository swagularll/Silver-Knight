using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using LitJson;
using System;
using System.Linq;

public class diary : MonoBehaviour
{
    private bool isReady = false;
    public ODM.diaryLog data_body;

    void Start()
    {
        ODM.diaryLog data_body = new ODM.diaryLog();
    }
    void Update()
    {
        //if (isReady)
        //{
        //    data_body.time_count += Time.deltaTime;
        //}
    }

    //public string getDiaryString()
    //{
    //   return data_body.getString();
    //}

    #region OUTCALL FUNCTIONS
    public void addResist()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        data_body.resist.Add(materName);
    }

    public void addFeed()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        data_body.feed.Add(materName);
    }

    public void addResistFailed()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        data_body.resist_failed.Add(materName);
    }
    public void addResistSuccess()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        data_body.resist_success.Add(materName);
    }
    public void addFeedFailed()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        data_body.feed_failed.Add(materName);
    }
    public void addFeedSuccess()
    {
        string materName = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value.GetComponent<warmbugAction>().warmbug_name;
        data_body.feed_success.Add(materName);
    }

    public void addDeadCount()
    {
        data_body.dead_count += 1;
        addKillerName();
    }

    public void addKillerName()
    {
        if (FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value != null)
        {
            GameObject killer = FsmVariables.GlobalVariables.GetFsmGameObject("currentMate").Value;
            string killerName = killer.GetComponent<warmbugAction>().warmbug_name;
            data_body.killer_name.Add(killerName);
        }
        (new ODM.saveRecord()).updateDiary(data_body);
    }
    #endregion
}
