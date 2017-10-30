using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson; //for JsonData
using System;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    public List<item> db = new List<item>();
    private JsonData itemJson;
    public TextAsset itemDB;
    void Start()
    {
        string itemDatabasePath = @"Data Collection\" + PlayerPrefs.GetString("lang") + @"\ItemDatabase";
        itemDB = Resources.Load<TextAsset>(itemDatabasePath);
        itemJson = JsonMapper.ToObject(itemDB.text);
        buildMapData();
    }

    void buildMapData()
    {
        for (int i = 0; i < itemJson.Count; i++)
        {
            db.Add(new item(
                (int)itemJson[i]["id"],
                (string)itemJson[i]["title"],
                (string)itemJson[i]["description"],
                (string)itemJson[i]["img"],
                (bool)itemJson[i]["stackable"],
                (int)itemJson[i]["amount"],
                (string)itemJson[i]["shortName"]
                ));
        }
    }

    public item getItem(int id)
    {
        return db.Where(x => x.id == id).FirstOrDefault();
    }

}
public class item
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public int entityCount { get; set; }
    public string img { get; set; }
    public Sprite sprite { get; set; }
    public bool stackable { get; set; }
    public int amount { get; set; }

    public string shortName { get; set; }

    public item(int _id, string _title, string _description, string _img, bool _stackable, int _amount, string _shortName)
    {
        this.id = _id;
        this.title = _title;
        this.description = _description;
        this.img = _img;
        this.sprite = Resources.Load<Sprite>("Item/" + this.img);
        this.stackable = _stackable;
        this.amount = _amount;
        this.shortName = _shortName;
    }

    public item()
    {
        this.id = -1;
    }
}