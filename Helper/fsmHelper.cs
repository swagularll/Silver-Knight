using UnityEngine;
using System.Collections;
using System;

public class fsmHelper
{

    public static PlayMakerFSM getFsm(string _obj_name, string _fsm_name)
    {
        PlayMakerFSM fsm = null;
        PlayMakerFSM[] temp = GameObject.Find(_obj_name).GetComponents<PlayMakerFSM>();
        foreach (PlayMakerFSM f in temp)
        {
            if (f.FsmName.Equals(_fsm_name))
            {
                fsm = f;
                break;
            }
        }
        return fsm;
    }

    public static PlayMakerFSM getFsm(GameObject _target, string _fsm_name)
    {
        try
        {
            PlayMakerFSM fsm = null;
            PlayMakerFSM[] temp = _target.GetComponents<PlayMakerFSM>();
            foreach (PlayMakerFSM f in temp)
            {
                if (f.FsmName.Equals(_fsm_name))
                {
                    fsm = f;
                    break;
                }
            }
            return fsm;
        }
        catch (Exception ex)
        {
            ODM.errorLog("fsmHelper", "Target:" + _target + ", Fsm:" + _fsm_name + ", Error:" + ex.ToString());
        }
        return null;
    }
}
