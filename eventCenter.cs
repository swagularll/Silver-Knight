using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using Assets.Script.ODM_Widget;

public class eventCenter : MonoBehaviour
{
    public GameObject obj_location_hint_text;
    public ODM.ODMDictionary flag_collection;

    private PlayMakerFSM fsm;
    private fsmHelper fsmHelper;

    void Awake()
    {
        fsmHelper = new fsmHelper();
    }

    void Start()
    {
        flag_collection = saveRecord.getCurrentRecord().flag_collection;
    }
    #region Flag functions
    public void checkFlag(string _flagString)
    {
        flagData flag = new flagData(_flagString);
        try
        {
            PlayMakerFSM f = fsmHelper.getFsm(flag.ObjName, flag.FsmName);
            bool flagValue = getFlagBool(flag.FlagName);
            if (flagValue)
            {
                f.SendEvent("true result");
            }
            else
            {
                f.SendEvent("false result");
            }
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name,
                "checkFlag can't Find Flag! flagName: " + _flagString + "",
                ex.ToString());
        }

    }
    public bool getFlagBool(string _flagName)
    {
        bool result = false;
        _flagName = _flagName.Trim();
        try
        {
            return Convert.ToBoolean(flag_collection.getValue(_flagName));
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name,
                "getFlagBool Error. flagName: " + _flagName,
                ex.ToString());
        }
        return result;
    }
    public void setFlagTrue(string _flagSet)
    {
        try
        {
            string[] flagSet = _flagSet.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < flagSet.Length; i++)
            {
                flag_collection.setValue(flagSet[i].Trim(), true.ToString());
            }
        }

        catch (Exception ex)
        {
            ODM.errorLog(transform.name,
                "setFlagTrue Error. flagName: " + _flagSet,
                ex.ToString());
        }
    }
    public void setFlagFalse(string _flagSet)
    {
        try
        {
            string[] flagSet = _flagSet.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < flagSet.Length; i++)
            {
                flag_collection.setValue(flagSet[i].Trim(), false.ToString());
            }
        }

        catch (Exception ex)
        {
            ODM.errorLog(transform.name,
                "setFlagTrue Error. flagName: " + _flagSet,
                ex.ToString());
        }
    }

    public void setFlagValue(string _flag_name, bool _flag_value)
    {
        try
        {
            flag_collection.setValue(_flag_name.Trim(), _flag_value.ToString());
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name,
                "setFlagTrue Error. flagName: " + _flag_name,
                ex.ToString());
        }
    }
    #endregion

    #region Warmbug Control

    public void setLairActivate(string _lairList)
    {
        string[] lairCollection = _lairList.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lairCollection.Length; i++)
        {
            for (int k = 0; k < 10; k++)
            {
                string levelName = lairCollection[i];
                if (levelName.IndexOf("-") != -1)
                {
                    levelName = levelName.Replace("-", "");
                    setFlagFalse(levelName + " Warmbug");
                }
                else
                {
                    setFlagTrue(levelName + " Warmbug");
                }
            }
        }
    }

    #endregion

    #region Item Control
    public bool tryUseItem(int _itemID)
    {
        inventoryDash inv = GetComponent<inventoryDash>();
        return inv.checkAndTakeAway(_itemID);
    }
    public bool checkItemExist(int _itemID)
    {
        inventoryDash inv = GetComponent<inventoryDash>();
        return inv.checkItemExist(_itemID);
    }
    #endregion

    #region UI Control
    public void renewLocation(string _str)
    {
        obj_location_hint_text.GetComponent<Text>().text = _str;
    }
    #endregion

    #region Data Class
    class flagData
    {
        string objName;
        string fsmName;
        string flagName;

        public flagData(string flagString)
        {
            string[] dataSet = flagString.Split(',');
            this.ObjName = dataSet[0].Trim();
            this.fsmName = dataSet[1].Trim();
            this.flagName = dataSet[2].Trim();
        }
        public string ObjName
        {
            get
            {
                return objName;
            }

            set
            {
                objName = value;
            }
        }

        public string FsmName
        {
            get
            {
                return fsmName;
            }

            set
            {
                fsmName = value;
            }
        }

        public string FlagName
        {
            get
            {
                return flagName;
            }

            set
            {
                flagName = value;
            }
        }

    }
    #endregion

}
