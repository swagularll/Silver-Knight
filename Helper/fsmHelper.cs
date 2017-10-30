using UnityEngine;
using System.Collections;
using System;

public class fsmHelper
{

    public static PlayMakerFSM getFsm(string objName, string fsmName)
    {
        PlayMakerFSM fsm = null;
        PlayMakerFSM[] temp = GameObject.Find(objName).GetComponents<PlayMakerFSM>();
        foreach (PlayMakerFSM f in temp)
        {
            if (f.FsmName.Equals(fsmName))
            {
                fsm = f;
                break;
            }
        }
        return fsm;
    }

    public static PlayMakerFSM getFsm(GameObject target, string fsmName)
    {
        try
        {
            PlayMakerFSM fsm = null;
            PlayMakerFSM[] temp = target.GetComponents<PlayMakerFSM>();
            foreach (PlayMakerFSM f in temp)
            {
                if (f.FsmName.Equals(fsmName))
                {
                    fsm = f;
                    break;
                }
            }
            return fsm;
        }
        catch (Exception ex)
        {
            ODM.errorLog("Static", "Holder:" + target + ", fsmName: " + fsmName, ex.ToString());
        }
        return null;

    }
}
