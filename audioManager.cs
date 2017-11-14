using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class audioManager
{
    public static string selectionSwitch = "Sound/[00]Components/Buttons/[Button Possitive][bubaproducer]button-5";
    public static string selectionNegative = "Sound/[00]Components/Buttons/[Button Negative][bubaproducer]button-4";
    public static string selectSmall = "Sound/[00]Components/Buttons/[Typer Negative][potentjello]buttons-and-knobs-10";
    public static string pageTurn = "Sound/[00]Components/Buttons/[Page][mydo1]page-turn";
    public static string electrical = "Sound/[00]Components/Buttons/[Electric][littlerobotsoundfactory]click-electronic-14";
    public static string electricalExit = "Sound/[00]Components/Buttons/[Electric Out][kickhat]button-sound-closed-1";
    public static string negativeSmall = "Sound/[00]Components/Buttons/[Click switch][bubaproducer]button-1";
    public static string typer = "Sound/[00]Components/Buttons/[Typer][7778]button-2";
    public static string woodFish = "Sound/[00]Components/Buttons/[Wood Fish][bubaproducer]button-27";

    //public void audioCheck()
    //{
    //    List<string> listTest = new List<string>();

    //    listTest.Add(selectionSwitch);
    //    listTest.Add(selectionNegative);
    //    listTest.Add(selectSmall);
    //    listTest.Add(pageTurn);
    //    listTest.Add(electrical);
    //    listTest.Add(electricalExit);
    //    listTest.Add(negativeSmall);
    //    listTest.Add(typer);
    //    listTest.Add(woodFish);

    //    for (int i = 0; i < listTest.Count; i++)
    //    {
    //        AudioClip a = Resources.Load<AudioClip>(listTest[i]);
    //        if (a == null)
    //        {
    //            ODM.errorLog("Static", "", "soundAudio Missing target: " + listTest[i]);
    //        }
    //    }

    //}

}
