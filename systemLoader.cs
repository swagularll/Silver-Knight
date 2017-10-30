using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Net.NetworkInformation;
using System.Linq;
using LitJson;
using System.Xml;

public class systemLoader : MonoBehaviour
{
    public bool isSaveLock = true;
    private string setting_files;
    private string connectionCode = "";

    private string url_getConnectionCode = "getConnectionCode";
    private string url_checkMac = "checkMac";

    private PlayMakerFSM fsm;
    void Awake()
    {

        setting_files = Application.dataPath + "/PPODM_SETTING.txt";
        //Screen.SetResolution(1024, 768, true);
        //Game Settings that will hardly change

        string save_folder_directory = Application.dataPath + @"\Save";
        PlayerPrefs.SetString("save_folder_directory", save_folder_directory); //PlayerPrefs.SetString("save folder", saveFolderPath);

        //string runtime_save_file = save_folder_directory + @"\Current.json";
        //PlayerPrefs.SetString("runtime_save_file", runtime_save_file);

        string auto_save_file = save_folder_directory + @"\Auto.json";
        PlayerPrefs.SetString("auto_save_file", auto_save_file);

        string base_flag_collection = @"Data Collection\FLAG_COLLECTION";
        PlayerPrefs.SetString("base_flag_collection", base_flag_collection);


        //Default settings
        PlayerPrefs.SetString("save_code", "default");

        //warmbugDeploymentFilePath = PlayerPrefs.GetString("saveSlot") + @"/" + "Lair";

        if (!Directory.Exists(save_folder_directory))
            Directory.CreateDirectory(save_folder_directory);
    }

    void Start()
    {
        fsm = fsmHelper.getFsm(transform.name, "FSM");
    }

    #region player data check
    public void checkKey()
    {
        if (File.Exists(setting_files))
        {
            try
            {
                //Read configuration from a txt file
                StreamReader sr = new StreamReader(setting_files);
                string sr_json = sr.ReadLine();
                ODM.systemConfiguration system_configuration = new ODM.systemConfiguration(ODM.decryption(sr_json));
                sr.Close();

                PlayerPrefs.SetString("player_nick_name", system_configuration.player_nick_name);
                PlayerPrefs.SetString("web_base", system_configuration.web_base);
                PlayerPrefs.SetString("service_base", system_configuration.service_base);
                PlayerPrefs.SetString("player_guid", system_configuration.player_guid);
                PlayerPrefs.SetString("mac", ""); //system_configuration.mac
                PlayerPrefs.SetString("player_email", system_configuration.player_email);
                PlayerPrefs.SetString("service_email", system_configuration.service_email);
                PlayerPrefs.SetString("warmbug_lock", system_configuration.warmbug_lock);

                fsm.SendEvent("has key");
            }
            catch (Exception ex)
            {
                ODM.errorLog(transform.name, "Fail to load key.", ex.ToString());
            }
        }
        else
        {
            fsm.SendEvent("no key");
        }
    }
    public void checkInternet()
    {
        StartCoroutine(request_GetConnectionCode((returnedValue) =>
        {
            connectionCode = returnedValue;
            if (connectionCode != null)
            {
                ODM.log(transform.name, "conneciton successed.");
                fsm.SendEvent("has internet");
            }
            else
            {
                ODM.log(transform.name, "conneciton failed.");
                fsm.SendEvent("no internet");
            }
        }));
    }
    public void checkMac()
    {
        StartCoroutine(request_checkMac((returnedValueMac) =>
        {
            string result = returnedValueMac;
            if (result == "mac success")
            {
                PlayerPrefs.SetString("Authentication", "Authentication Content");
                PlayerPrefs.SetString("MacAuthentication", "Mac Authentication Content");
                //PlayerPrefs.SetString("Authentication", ODM.encryption("Thank you for working so hard. If you see this, you already cracked the decryption."));
                //PlayerPrefs.SetString("MacAuthentication", ODM.encryption("As a reminder, the protection will dismiss in one year. Don't waste too much time on this."));
                ODM.log(transform.name, "mac success");
                fsm.SendEvent("mac success");
            }
            else
            {
                ODM.log(transform.name, "mac failed.");
                fsm.SendEvent("mac failed");
            }
        }));
    }
    #endregion

    #region internect connection
    IEnumerator request_GetConnectionCode(System.Action<string> callback)
    {
        WWW requestConnectionCode = new WWW(PlayerPrefs.GetString("serviceBase") + url_getConnectionCode);
        StartCoroutine(WaitForRequest(requestConnectionCode));
        yield return requestConnectionCode;

        string htmlResponce = requestConnectionCode.text.ToString();

        if (requestConnectionCode.error == null)
        {
            XmlTextReader xtr = new XmlTextReader(new StringReader(htmlResponce));

            while (xtr.Read())
            {
                if (xtr.NodeType.Equals(XmlNodeType.Text))
                {
                    connectionCode = xtr.Value;
                    ODM.log(transform.name, "getConnectionCode success");
                    callback(connectionCode);
                }
            }
        }
        else
        {
            callback(null);
            ODM.log(transform.name, "getConnectionCode Error: " + requestConnectionCode.error);
        }
    }

    IEnumerator request_checkMac(System.Action<string> callback)
    {
        CMacPack m = new CMacPack();
        m.connectionCode = connectionCode;
        m.playerID = PlayerPrefs.GetString("player_guid");
        m.mac = getMac();
        string result = "";

        WWWForm form = new WWWForm();
        JsonData json = JsonMapper.ToJson(m);
        form.AddField("_log", ODM.encryption(json.ToString()));
        WWW requestMacUpload = new WWW(PlayerPrefs.GetString("service_base") + url_checkMac, form);
        StartCoroutine(WaitForRequest(requestMacUpload));
        yield return requestMacUpload;

        string htmlResponce = requestMacUpload.text.ToString();


        if (requestMacUpload.error == null)
        {
            XmlTextReader xtr = new XmlTextReader(new StringReader(htmlResponce));

            while (xtr.Read())
            {
                if (xtr.NodeType.Equals(XmlNodeType.Text))
                {
                    result = ODM.encryption(xtr.Value);
                    callback(result);
                }
            }
        }
        else
        {
            callback(null);
            ODM.errorLog(transform.name,
                "request_checkMac Error.",
                requestMacUpload.error);
        }
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        string htmlResponce = www.text.ToString();
        string dataString = "";

        if (www.error == null)
        {
            XmlTextReader xtr = new XmlTextReader(new StringReader(htmlResponce));

            while (xtr.Read())
            {
                if (xtr.NodeType.Equals(XmlNodeType.Text))
                {
                    dataString = xtr.Value;
                    ODM.log(transform.name, "Webservice replied: " + dataString);
                    break;
                }
            }
        }
        else
        {
            ODM.errorLog(transform.name,
                "WWW Error.",
                www.error);
        }
    }
    #endregion

    #region data manupulation

    private string getMac()
    {
        string result = "";
        try
        {
            string macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                              where nic.OperationalStatus == OperationalStatus.Up
                              select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            result = macAddr;
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name,
                "Fail to identify user mac.",
                ex.ToString());
        }

        return result;
    }

    public void welcomeMessage()
    {
        PlayMakerFSM f = fsmHelper.getFsm("Little Bastard", "FSM");
        string path = Application.dataPath + f.FsmVariables.GetFsmString("fileName").Value.ToString();
        if (File.Exists(path))
        {
            StreamReader sr = new StreamReader(path);
            string s = sr.ReadLine();
            sr.Close();
            if (s.Equals(f.FsmVariables.GetFsmString("welcome").Value.ToString()))
            {
                fsm.SendEvent("blessed by Ava");
            }
            else
            {
                fsm.SendEvent("next");
            }
        }
        else
        {
            fsm.SendEvent("next");
        }
    }

    #endregion

    #region data class
    class CMacPack
    {
        public string connectionCode { get; set; }
        public string playerID { get; set; }
        public string mac { get; set; }
    }
    #endregion

}
