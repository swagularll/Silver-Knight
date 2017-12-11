using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using HutongGames.PlayMaker;


public class eventCenter : MonoBehaviour
{
    public GameObject obj_location_hint_text;
    public ODM.ODMDictionary flag_collection;

    private PlayMakerFSM fsm;

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
            PlayMakerFSM f = fsmHelper.getFsm(flag.obj_name, flag.fsm_name);
            bool flagValue = getFlagBool(flag.flag_name);
            if (flagValue)
            {
                f.SendEvent(eventName.true_result);
            }
            else
            {
                f.SendEvent(eventName.false_result);
            }
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name, "checkFlag can't Find Flag! flagName: " + _flagString + ", " + ex.ToString());
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
            ODM.errorLog(transform.name,"getFlagBool Error. flagName: " + _flagName + ", " + ex.ToString());
        }
        return result;
    }
    public void setFlagTrue(string _flagSet)
    {
        try
        {
            string[] flagSet = _flagSet.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < flagSet.Length; i++)
            {
                flag_collection.setValue(flagSet[i].Trim(), true.ToString());
            }
        }

        catch (Exception ex)
        {
            ODM.errorLog(transform.name, "setFlagTrue Error. flagName: " + _flagSet + ", " + ex.ToString());
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
            ODM.errorLog(transform.name, "setFlagTrue Error. flagName: " + _flagSet + ", " + ex.ToString());
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
            ODM.errorLog(transform.name, "setFlagTrue Error. flagName: " + _flag_name + ", " + ex.ToString());
        }
    }
    #endregion

    #region Warmbug Control
    //XXX
    //public void setLairActivate(string _lairList)
    //{
    //    string[] lairCollection = _lairList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

    //    for (int i = 0; i < lairCollection.Length; i++)
    //    {
    //        for (int k = 0; k < 10; k++)
    //        {
    //            string levelName = lairCollection[i];
    //            if (levelName.IndexOf("-") != -1)
    //            {
    //                levelName = levelName.Replace("-", "");
    //                setFlagFalse(ODMVariable.convert.getWarmbugFlag(levelName));
    //            }
    //            else
    //            {
    //                setFlagTrue(ODMVariable.convert.getWarmbugFlag(levelName));
    //            }
    //        }
    //    }
    //}

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
        string _obj_name;
        string _fsm_name;
        string _flag_name;

        public flagData(string flagString)
        {
            string[] dataSet = flagString.Split(',');
            this.obj_name = dataSet[0].Trim();
            this._fsm_name = dataSet[1].Trim();
            this._flag_name = dataSet[2].Trim();
        }
        public string obj_name
        {
            get
            {
                return _obj_name;
            }

            set
            {
                _obj_name = value;
            }
        }

        public string fsm_name
        {
            get
            {
                return _fsm_name;
            }

            set
            {
                _fsm_name = value;
            }
        }

        public string flag_name
        {
            get
            {
                return _flag_name;
            }

            set
            {
                _flag_name = value;
            }
        }

    }
    #endregion

}
