using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using LitJson;
using Assets.Script.ODM_Widget;
using System;

public class saveButton : MonoBehaviour {

    public GameObject text_difficulty;
    public GameObject text_title;
    public GameObject text_date;
    public GameObject map_database_holder;

    public string saveFileGUID;
    public bool isSelectable = true;

    private string saveFileCreatedTime;
    private string savedScene;

    public Animator ani;

    void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void initilizeData(ODM.ODMDictionary db)
    {
        setAlive();
        saveFileGUID = db.getValue("save_id");
        saveFileCreatedTime = db.getValue("save_created_time");
        savedScene = db.getValue("saved_scene");

        int int_difficulty = Int32.Parse(db.getValue("game_difficulty")); 

        string difficulty = dataWidget.getTranslaton(dataWidget.getDifficultyText(int_difficulty));
        string sceneName = map_database_holder.GetComponent<MapDatabase>().getMap(savedScene).title;

        text_difficulty.transform.GetComponent<Text>().text = difficulty;
        text_title.transform.GetComponent<Text>().text = savedScene + " - " + sceneName + " - " +
            "HP:" + db.getValue("ava_current_health") + "/" + "SP:" + db.getValue("ava_current_sp");
        text_date.transform.GetComponent<Text>().text = saveFileCreatedTime;
    }

    public bool isSelected
    {
        get { return ani.GetBool("isSelected"); }
        set {ani.SetBool("isSelected", value); }
    }

    public void setDead()
    {
        isSelectable = false;
        ani.SetBool("isSelectable", false);
        text_difficulty.transform.GetComponent<Text>().text = "";
        text_title.transform.GetComponent<Text>().text = "";
        text_date.transform.GetComponent<Text>().text = "";
    }

    public void setAlive()
    {
        isSelectable = true;
        ani.SetBool("isSelectable", true);
    }

}
