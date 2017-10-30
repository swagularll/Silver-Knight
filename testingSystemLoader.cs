using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;
using HutongGames.PlayMaker;
using System.Text;

public class testingSystemLoader : MonoBehaviour {

    public bool isSaveLock = true;
    public string lang;
    public int difficulty = -1;

    void Awake()
    {
        PlayerPrefs.SetString("lang", lang);
        PlayerPrefs.SetString("save_code", "Test");

        switch (difficulty)
        {
            case -1:
                break;
            case 1:
                PlayerPrefs.SetInt("game_difficulty", 1);
                break;
            case 2:
                PlayerPrefs.SetInt("game_difficulty", 2);
                break;
            case 3:
                PlayerPrefs.SetInt("game_difficulty", 3);
                break;
            case 4:
                PlayerPrefs.SetInt("game_difficulty", 4);
                break;
        }

        Debug.Log("save_code: " + PlayerPrefs.GetString("save_code"));

        string fileAddress = Application.dataPath + "/ODM Settings.txt";
        StreamReader sr = new StreamReader(fileAddress);
        string s = sr.ReadLine();
        string keyContent = ODM.decryption(s);
        sr.Close();
        JsonData key = JsonMapper.ToObject(keyContent);
        PlayerPrefs.SetString("playerNickName", key["playerNickName"].ToString());
        PlayerPrefs.SetString("webBase", key["webBase"].ToString());
        PlayerPrefs.SetString("serviceBase", key["serviceBase"].ToString());
        string playerID = "745f9ca9-65b0-4370-856a-b803fe0d3f67";

        PlayerPrefs.SetString("playerGUID", playerID);

        PlayerPrefs.SetString("mac", "40E230DF24E5");
        PlayerPrefs.SetString("warmbugLock", key["warmbugLock"].ToString());//perserve for later
        PlayerPrefs.SetString("playerEmail", key["playerEmail"].ToString());
        PlayerPrefs.SetString("serviceEmail", key["serviceEmail"].ToString());
        PlayerPrefs.SetInt("countDown", (int)key["countDown"]);//perserve for later

       
    }
}
