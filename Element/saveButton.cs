using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using LitJson;

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
        saveFileGUID = db.getValue(ODMVariable.save.save_id);
        saveFileCreatedTime = db.getValue(ODMVariable.save.save_created_time);
        savedScene = db.getValue(ODMVariable.save.saved_scene);

        int int_difficulty = Int32.Parse(db.getValue(ODMVariable.save.game_difficulty)); 

        string difficulty = dataWidget.getTranslaton(dataWidget.getDifficultyText(int_difficulty));
        string sceneName = map_database_holder.GetComponent<MapDatabase>().getMap(savedScene).title;

        text_difficulty.transform.GetComponent<Text>().text = difficulty;
        text_title.transform.GetComponent<Text>().text = savedScene + " - " + sceneName + " - " +
            "HP:" + db.getValue(ODMVariable.save.ava_current_health) + "/" + "SP:" + db.getValue(ODMVariable.save.ava_current_sp);
        text_date.transform.GetComponent<Text>().text = saveFileCreatedTime;
    }

    public bool isSelected
    {
        get { return ani.GetBool(ODMVariable.animation.is_selected); }
        set {ani.SetBool(ODMVariable.animation.is_selected, value); }
    }

    public void setDead()
    {
        isSelectable = false;
        ani.SetBool(ODMVariable.animation.is_selectable, false);
        text_difficulty.transform.GetComponent<Text>().text = "";
        text_title.transform.GetComponent<Text>().text = "";
        text_date.transform.GetComponent<Text>().text = "";
    }

    public void setAlive()
    {
        isSelectable = true;
        ani.SetBool(ODMVariable.animation.is_selectable, true);
    }

}
