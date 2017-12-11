using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class storyboard : MonoBehaviour
{
    public string next_level_name = "NEXT LEVEL NAME";
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
        aud = GetComponent<AudioSource>();
        aud.clip = buttonSound;
        txtStory = Resources.Load<TextAsset>(ODMVariable.path.storyboard_file_resource);
        txtContent = txtStory.text.Split('\n');

        fsm = fsmHelper.getFsm(transform.name, ODMVariable.common.default_fsm);
        ODMVariable.level_to_load = next_level_name;
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
            loadLevel();
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
        if (showingString.IndexOf(ODMVariable.tag.end) != 0)
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
        fsm.SendEvent(eventName.load_level);
    }

    private void loadLevel()
    {
        endLineControl = true;
        Application.LoadLevel(ODMVariable.common.initilization_level_name);
        //In initilization, save builder will automatically load level by "level to load" value.
    }
}
