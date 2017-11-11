﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using LitJson;
using System.Linq;
using HutongGames.PlayMaker;
using System;
using System.Text;
using System.Security.Cryptography;

public class ODM
{
    #region DATA STRUCTURE
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
        public void add(string _key, string _value)
        {
            if (!peek(_key))
            {
                data d = new data(_key, _value);
                data_body.Add(d);
            }
            else
            {
                data_body[data_body.FindIndex(x => x.key.Equals(_key))].value = _value;
            }
        }
        public void clear()
        {
            data_body.Clear();
        }
        public bool peek(string _key)
        {
            if (data_body.Where(x => x.key == _key).FirstOrDefault() == null)
                return false;
            return true;
        }
        public string getValue(string _key)
        {
            try
            {
                return data_body.Where(x => x.key == _key).FirstOrDefault().value;

            }
            catch (Exception ex)
            {
                errorLog("_key = " + _key, "", ex.ToString());
            }
            return "";
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

    #region ENCRYPTION
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
            ODM.errorLog("Static", "Encryption error.", ex.ToString());
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
            ODM.errorLog("Static", "Decryption error.", ex.ToString());
        }

        return result;
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

    public static void errorLog(string _holder, string _message, string _system_message)
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

}
