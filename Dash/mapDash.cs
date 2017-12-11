using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using LitJson;
using HutongGames.PlayMaker;

public class mapDash : MonoBehaviour
{
    //renew code
    public GameObject mapSquare;
    public GameObject mapSquareContainer;
    public GameObject holder;//for setting grid size
    public GameObject txtMapInfoGeneralInformation;
    public GameObject txtMapInfoCurrentSelected;
    public GameObject panelSelectedMapInformationDisplay;

    public List<List<GameObject>> map_square_collection;
    public MapDatabase db;

    private AudioSource aud;
    private eventCenter event_center;

    private CMap current_selected_map;
    private CMap current_level_map;

    private int explored_map_number = 0;

    private int current_selected_x = -1;
    private int current_selected_y = -1;
    public int limit_x = 9;
    public int limit_y = 5;

    private bool panel_enabled = false;
    private bool state_control = false;

    private string[] map_name_collection;

    private string txt_currentLocation;
    private string txt_explored;
    private string txt_unexplored;
    private string txt_exploredRate;

    void Awake()
    {
        map_square_collection = new List<List<GameObject>>();
        db = GetComponent<MapDatabase>();
        aud = GetComponent<AudioSource>();
        event_center = GetComponent<eventCenter>();
    }
    void Start()
    {
        map_name_collection = dataWidget.getMapName();
        initializeMap();
        txt_currentLocation = dataWidget.getTranslaton(ODMVariable.translation.map_info_currentlocation);
        txt_explored = dataWidget.getTranslaton(ODMVariable.translation.map_info_explored);
        txt_unexplored = dataWidget.getTranslaton(ODMVariable.translation.map_info_unexplored);
        txt_exploredRate = dataWidget.getTranslaton(ODMVariable.translation.map_info_explored_rate);
    }
    void Update()
    {
        if (panel_enabled && !state_control && !ODMVariable.is_system_locked)
        {
            aud.clip = Resources.Load<AudioClip>(audioResource.electrical);
            aud.Play();
            state_control = true;
            selectCurrentMap();//(-1, -1);
        }
        else if (panel_enabled && state_control && !ODMVariable.is_system_locked)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                closePanel();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (current_selected_y == limit_y)
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                }
                else//select next item
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    current_selected_y++;
                    selectCurrentMap();//(0, -1);
                }
                aud.Play();
            }


            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (current_selected_y == 0)
                {
                    closePanel();
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    aud.Play();
                    current_selected_y--;
                    selectCurrentMap();//(0, +1);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (current_selected_x == limit_x)
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    current_selected_x++;
                    selectCurrentMap();//(-1, 0);
                }
                aud.Play();
            }


            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (current_selected_x == 0)
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_negative);
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(audioResource.selection_switch);
                    current_selected_x--;
                    selectCurrentMap();//(1, 0);
                }
                aud.Play();
            }
        }
    }
    private void selectCurrentMap()
    {
        resetMapColor();
        showSelectedMapInfo();
    }
    private void getExploreRate()
    {
        explored_map_number = 0;
        for (int i = 0; i < map_name_collection.Length; i++)
        {
            if (event_center.getFlagBool(ODMVariable.convert.getAreaFlag(map_name_collection[i])))
                explored_map_number++;
        }
    }
    public string getExlorationRateString()
    {
        getExploreRate();
        double exloreRate = Convert.ToDouble(explored_map_number) / 60;
        return (exloreRate * 100).ToString("F2") + "%";
    }
    private void showCurrentMapInfo()
    {
        current_level_map = db.getMap(Application.loadedLevelName);
        getExploreRate();
        double exloreRate = Convert.ToDouble(explored_map_number) / 60;
        if (current_level_map != null)
        {
            string generalInfo =
            txt_currentLocation + current_level_map.name + " - " + current_level_map.title + Environment.NewLine;
            generalInfo += txt_explored + explored_map_number.ToString() + Environment.NewLine;
            generalInfo += txt_unexplored + (60 - explored_map_number).ToString() + Environment.NewLine;
            generalInfo += txt_exploredRate + (exloreRate * 100).ToString("F2") + "%" + Environment.NewLine;

            txtMapInfoGeneralInformation.GetComponent<Text>().text = generalInfo;
        }
    }
    private void initializeMap()
    {
        try
        {
            string[] s = { "A", "B", "C", "D", "E", "F", };
            for (int k = 0; k < 6; k++)
            {
                GameObject hold = Instantiate(holder);
                List<GameObject> mapLine = new List<GameObject>();
                for (int i = 0; i < 10; i++)
                {
                    GameObject square = Instantiate(mapSquare);
                    square.name = s[k] + i.ToString();
                    square.transform.GetComponent<mapSquare>().selfRef = db.getMap(square.name);
                    square.transform.GetChild(0).GetComponent<Text>().text = square.name;
                    square.transform.SetParent(hold.transform);
                    mapLine.Add(square);
                }
                map_square_collection.Add(mapLine);
                hold.transform.SetParent(mapSquareContainer.transform);
            }
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name, "Failed to initilize map. Message:" + ex.ToString());
        }
    }
    public void refreshMapSquareDoors()
    {
        for (int i = 0; i < map_square_collection.Count; i++)
        {
            for (int j = 0; j < map_square_collection[i].Count; j++)
            {
                map_square_collection[i][j].GetComponent<mapSquare>().renewDoorStatus();
            }
        }
    }
    public void closePanel()
    {
        panelSelectedMapInformationDisplay.GetComponent<CanvasGroup>().alpha = 0;
        aud.clip = Resources.Load<AudioClip>(audioResource.electrical_out);
        aud.Play();
        GetComponent<menuManager>().disableMiddleTab();
        resetVariables();
        resetMapColor();
        getExploreRate();
    }
    public void openPanel()
    {
        panelSelectedMapInformationDisplay.GetComponent<CanvasGroup>().alpha = 1;
        showCurrentMapInfo();
        resetMapColor();
        panel_enabled = true;
        current_selected_x = 0;
        current_selected_y = 0;
    }
    public void preSetting()
    {
        showCurrentMapInfo();
        resetMapColor();
        refreshMapSquareDoors();
    }
    private void resetVariables()
    {
        panel_enabled = false;
        state_control = false;
        current_selected_x = -1;
        current_selected_y = -1;
    }
    public void resetMapColor()
    {
        refreshMapSquareDoors();
        for (int i = 0; i < map_square_collection.Count; i++)
        {
            for (int k = 0; k < map_square_collection[i].Count; k++)
            {
                var square = map_square_collection[i][k].GetComponent<mapSquare>();
                square.state = event_center.getFlagBool(ODMVariable.convert.getAreaFlag(square.selfRef.name));

                square.setColor();

                if (current_level_map != null)
                {
                    if (map_square_collection[i][k].name.Equals(current_level_map.name))
                    {
                        map_square_collection[i][k].GetComponent<mapSquare>().setCurrent();
                    }
                }
            }
        }
        if (current_selected_x != -1 && current_selected_y != -1)
            map_square_collection[current_selected_y][current_selected_x].GetComponent<mapSquare>().setSelected();
    }
    private void showSelectedMapInfo()
    {
        txtMapInfoCurrentSelected.GetComponent<Text>().text =
            map_square_collection[current_selected_y][current_selected_x].GetComponent<mapSquare>().showInfo();
    }
}
