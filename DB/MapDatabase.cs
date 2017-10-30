using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using System;
using System.Linq;

public class MapDatabase : MonoBehaviour
{

    private List<CMap> db = new List<CMap>();
    private JsonData mapJson;
    public TextAsset mapDB;
    void Start()
    {
        string mapDatabasePath = @"Data Collection\" + PlayerPrefs.GetString("lang") + @"\MapDatabase";
        mapDB = Resources.Load<TextAsset>(mapDatabasePath);
        mapJson = JsonMapper.ToObject(mapDB.text);
        buildMapData();
    }
    void buildMapData()
    {
        int log = 0;
        try
        {
            for (int i = 0; i < mapJson.Count; i++)
            {
                CDoor doorRight = null;
                CDoor doorDown = null;
                if (existKey(mapJson[i], "Right Door"))
                {
                    doorRight = new CDoor();
                    doorRight.Hint = (string)mapJson[i]["Right Door"]["Hint"];
                    //doorRight.Flag = (string)mapJson[i]["Right Door"]["Property"];
                }
                if (existKey(mapJson[i], "Down Door"))
                {
                    doorDown = new CDoor();
                    doorDown.Hint = (string)mapJson[i]["Down Door"]["Hint"];
                    //doorDown.Flag = (string)mapJson[i]["Down Door"]["Property"];
                }
                log = i;
                db.Add(new CMap((string)mapJson[i]["name"], (string)mapJson[i]["title"], (string)mapJson[i]["description"], doorRight, doorDown));
            }
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name,
                "", ex.ToString());
        }
    }
    public bool existKey(JsonData jd, string key)
    {
        if (((IDictionary)jd).Contains(key))
        {
            //string valuestr = (string)jd["KeyName"];
            return true;
        }
        return false;
    }

    public CMap getMap(string mapName)
    {
        return db.Where(m => m.name == mapName).FirstOrDefault();
    }

    public CMap getMap(int index_X, int index_Y)
    {
        string[] s = { "A", "B", "C", "D", "E", "F", };
        return db.Where(m => m.name.Equals(s[index_Y] + index_X.ToString())).FirstOrDefault();
    }
}
public class CMap
{
    public string name { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public CDoor rightDoor { get; set; }
    public CDoor downDoor { get; set; }

    public CMap(string _name, string _title, string _description, CDoor _rightDoor, CDoor _downDoor)
    {
        this.name = _name;
        this.title = _title;
        this.description = _description;
        this.rightDoor = _rightDoor;
        this.downDoor = _downDoor;
    }
}
public class CDoor
{
    public string Hint { get; set; }
    public string Flag { get; set; }
}