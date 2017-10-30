using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class audioManager : MonoBehaviour {

    public string selectionSwitch = "Sound/[00]Components/Buttons/[Button Possitive][bubaproducer]button-5";
    public string selectionNegative = "Sound/[00]Components/Buttons/[Button Negative][bubaproducer]button-4";
    public string selectSmall = "Sound/[00]Components/Buttons/[Typer Negative][potentjello]buttons-and-knobs-10";
    public string pageTurn = "Sound/[00]Components/Buttons/[Page][mydo1]page-turn";
    public string electrical = "Sound/[00]Components/Buttons/[Electric][littlerobotsoundfactory]click-electronic-14";
    public string electricalExit = "Sound/[00]Components/Buttons/[Electric Out][kickhat]button-sound-closed-1";
    public string negativeSmall = "Sound/[00]Components/Buttons/[Click switch][bubaproducer]button-1";
    public string typer = "Sound/[00]Components/Buttons/[Typer][7778]button-2";
    public string woodFish = "Sound/[00]Components/Buttons/[Wood Fish][bubaproducer]button-27";

    private List<string> listTest = new List<string>();

    void Start()
    {
        listTest.Add(selectionSwitch);
        listTest.Add(selectionNegative);
        listTest.Add(selectSmall);
        listTest.Add(pageTurn);
        listTest.Add(electrical);
        listTest.Add(electricalExit);
        listTest.Add(negativeSmall);
        listTest.Add(typer);
        listTest.Add(woodFish);
        

        for (int i = 0; i < listTest.Count; i++)
        {
            AudioClip a = Resources.Load<AudioClip>(listTest[i]);
            if (a == null)
            {
                Debug.LogError("soundAudio Missing target: " + listTest[i]);
            }
        }

    }

}
