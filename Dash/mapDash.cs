using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using LitJson;
using HutongGames.PlayMaker;
using Assets.Script.ODM_Widget;

public class mapDash : MonoBehaviour
{
    public GameObject mapSquare;
    public GameObject mapSquareContainer;
    public GameObject holder;//for setting grid size


    private bool mapPanelEnabled = false;
    private bool stateControl = false;

    private int currentSelectedX = -1;
    private int currentSelectedY = -1;

    private audioManager aud_manager;
    private AudioSource aud;

    public int X_limit = 9;
    public int Y_limit = 5;

    public List<List<GameObject>> mapSquareCollection;
    public MapDatabase db;


    private CMap currentSelectedMap;
    private CMap currentLevelMap;

    private eventCenter evnetCenter;
    private int exploredMapNumber = 0;

    string[] mapNames;
    private string txt_currentLocation;
    private string txt_explored;
    private string txt_unexplored;
    private string txt_exploredRate;

    public GameObject txtMapInfoGeneralInformation;
    public GameObject txtMapInfoCurrentSelected;
    public GameObject panelSelectedMapInformationDisplay;

    void Awake()
    {
        aud_manager = new audioManager();
        mapSquareCollection = new List<List<GameObject>>();

        db = GetComponent<MapDatabase>();
        aud = GetComponent<AudioSource>();
        evnetCenter = GetComponent<eventCenter>();
    }

    void Start()
    {
        mapNames = dataWidget.getMapName();
        initializeMap();
        txt_currentLocation = dataWidget.getTranslaton("Map Info currentLocation");//  GameObject.Find("Map Info currentLocation").GetComponent<Text>().text;
        txt_explored = dataWidget.getTranslaton("Map Info explored");//GameObject.Find("Map Info explored rate").GetComponent<Text>().text;
        txt_unexplored = dataWidget.getTranslaton("Map Info unexplored");//GameObject.Find("Map Info unexplored rate").GetComponent<Text>().text;
        txt_exploredRate = dataWidget.getTranslaton("Map Info explored rate");//GameObject.Find("Map Info exploredRate").GetComponent<Text>().text;
    }

    void Update()
    {
        if (mapPanelEnabled && !stateControl)
        {
            aud.clip = Resources.Load<AudioClip>(aud_manager.electrical);
            aud.Play();
            stateControl = true;//for the first time loading
            selectCurrentMap();//(-1, -1);
        }
        else if (mapPanelEnabled && stateControl)//When the select function is enabled...
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                closePanel();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentSelectedY == Y_limit)
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionNegative);
                }
                else//select next item
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                    currentSelectedY++;
                    selectCurrentMap();//(0, -1);
                }
                aud.Play();
            }


            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentSelectedY == 0)
                {
                    closePanel();
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                    aud.Play();
                    currentSelectedY--;
                    selectCurrentMap();//(0, +1);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentSelectedX == X_limit)
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionNegative);
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                    currentSelectedX++;
                    selectCurrentMap();//(-1, 0);
                }
                aud.Play();
            }


            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentSelectedX == 0)
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionNegative);
                }
                else
                {
                    aud.clip = Resources.Load<AudioClip>(aud_manager.selectionSwitch);
                    currentSelectedX--;
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
        exploredMapNumber = 0;
        for (int i = 0; i < mapNames.Length; i++)
        {
            if (evnetCenter.getFlagBool("Area " + mapNames[i]))
                exploredMapNumber++;
        }

    }

    public string getExlorationRateString()
    {
        getExploreRate();
        double exloreRate = Convert.ToDouble(exploredMapNumber) / 60;
        return (exloreRate * 100).ToString("F2") + "%";
    }

    private void showCurrentMapInfo()
    {
        currentLevelMap = db.getMap(Application.loadedLevelName);
        getExploreRate();
        double exloreRate = Convert.ToDouble(exploredMapNumber) / 60;
        if (currentLevelMap != null)
        {
            string generalInfo =
            txt_currentLocation + currentLevelMap.name + " - " + currentLevelMap.title + Environment.NewLine;
            generalInfo += txt_explored + exploredMapNumber.ToString() + Environment.NewLine;
            generalInfo += txt_unexplored + (60 - exploredMapNumber).ToString() + Environment.NewLine;
            generalInfo += txt_exploredRate + (exloreRate * 100).ToString("F2") + "%" + Environment.NewLine;

            txtMapInfoGeneralInformation.GetComponent<Text>().text = generalInfo;
        }
    }

    private void initializeMap()
    {
        try
        {
            //float sizeBase = (float)((mapSquareContainer.GetComponent<RectTransform>().rect.width * 0.1f) * 0.5);

            string[] s = { "A", "B", "C", "D", "E", "F", };
            for (int k = 0; k < 6; k++)
            {
                GameObject hold = Instantiate(holder);
                //hold.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(sizeBase, sizeBase);
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
                mapSquareCollection.Add(mapLine);
                hold.transform.SetParent(mapSquareContainer.transform);
            }
        }
        catch (Exception ex)
        {
            ODM.errorLog(transform.name, "Failed to initilize map.", ex.ToString());
        }
    }

    public void refreshMapSquareDoors()
    {
        for (int i = 0; i < mapSquareCollection.Count; i++)
        {
            for (int j = 0; j < mapSquareCollection[i].Count; j++)
            {
                mapSquareCollection[i][j].GetComponent<mapSquare>().renewDoorStatus();
            }
        }
    }

    public void closePanel()
    {
        panelSelectedMapInformationDisplay.GetComponent<CanvasGroup>().alpha = 0;
        aud.clip = Resources.Load<AudioClip>(aud_manager.electricalExit);
        aud.Play();
        resetVariables();
        GetComponent<menuManager>().tabSwitch = true; //make the top tab goes back to previous state...
        resetMapColor();
        getExploreRate();
    }

    public void openPanel()
    {
        panelSelectedMapInformationDisplay.GetComponent<CanvasGroup>().alpha = 1;
        showCurrentMapInfo();
        resetMapColor();
        mapPanelEnabled = true;
        currentSelectedX = 0;
        currentSelectedY = 0;
    }



    public void preSetting()
    {
        showCurrentMapInfo();
        resetMapColor();
        refreshMapSquareDoors();
    }
    private void resetVariables()
    {
        mapPanelEnabled = false;
        stateControl = false;
        currentSelectedX = -1;
        currentSelectedY = -1;
    }
    public void resetMapColor()
    {
        refreshMapSquareDoors();
        for (int i = 0; i < mapSquareCollection.Count; i++)
        {
            for (int k = 0; k < mapSquareCollection[i].Count; k++)
            {
                var square = mapSquareCollection[i][k].GetComponent<mapSquare>();
                square.state = evnetCenter.getFlagBool("Area " + square.selfRef.name);

                square.setColor();

                if (currentLevelMap != null)
                {
                    if (mapSquareCollection[i][k].name.Equals(currentLevelMap.name))
                    {
                        mapSquareCollection[i][k].GetComponent<mapSquare>().setCurrent();
                    }
                }
            }
        }
        if (currentSelectedX != -1 && currentSelectedY != -1)
            mapSquareCollection[currentSelectedY][currentSelectedX].GetComponent<mapSquare>().setSelected();
    }
    private void showSelectedMapInfo()
    {
        txtMapInfoCurrentSelected.GetComponent<Text>().text =
            mapSquareCollection[currentSelectedY][currentSelectedX].GetComponent<mapSquare>().showInfo();
    }
}
