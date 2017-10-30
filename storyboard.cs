using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class storyboard : MonoBehaviour
{
    public string nextLevelName = "NEXT LEVEL NAME";
    public TextAsset txtStory;
    public Text txtLine;
    public AudioClip buttonSound;

    public string storyboardTitle = "";
    private string[] txtContent;
    private int currentIndex = -1;
    private AudioSource aud;
    private bool endLineControl = false;
    private PlayMakerFSM fsm;
    void Awake()
    {
        string fileLocaion = @"Data Collection\" + PlayerPrefs.GetString("lang") + @"\Storyboard";
        aud = GetComponent<AudioSource>();
        aud.clip = buttonSound;
        txtStory = Resources.Load<TextAsset>(fileLocaion);
        txtContent = txtStory.text.Split('\n');

        fsm = fsmHelper.getFsm(transform.name, "Exit");
        fsm.FsmVariables.GetFsmString("next level name").Value = nextLevelName;

        //GetComponent<AudioSource>().clip = PlayerPrefs.GetString("stroyboard theme"));
    }
    void Start()
    {
        startShowLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!endLineControl)
            {
                aud.Play();
                showNextLine();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            fastLoad();
        }
    }

    public void startShowLine()
    {
        currentIndex = 0;
        string storyTitle = "[" + storyboardTitle + "]";
        for (int i = 0; i < txtContent.Length; i++)
        {
            if (txtContent[i].IndexOf(storyTitle) != -1)
            {
                txtContent[i].Replace(storyTitle, "");
                currentIndex = i;
                showNextLine();
                break;
            }
        }
    }

    public void showNextLine()
    {

        currentIndex++;
        string showingString = txtContent[currentIndex];
        if (showingString.IndexOf("[END]") != 0)
        {
            txtLine.GetComponent<Text>().text = showingString;
        }
        else
        {
            endPlot();
        }
    }

    private void endPlot()
    {
        endLineControl = true;
        fsm.SendEvent("load level");
    }

    private void fastLoad()
    {
        endLineControl = true;
        fsm.SendEvent("fast load");
    }
}
