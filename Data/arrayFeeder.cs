using UnityEngine;
using System.Collections;

public class arrayFeeder : MonoBehaviour {

    public string collectionRow;
    public string linked_fsm = "FSM";

    private string[] collection;
    private int control_index = 0;
    private PlayMakerFSM fsm;

    void Awake()
    {
        collection = collectionRow.Split(',');
        fsm = fsmHelper.getFsm(transform.gameObject, linked_fsm);
    }

    //For beacon light
    public void getFloat(string fsmFloatName)
    {
        if (control_index == collection.Length - 1)
            control_index = 0;
        else
            control_index++;
        fsm.FsmVariables.GetFsmFloat(fsmFloatName).Value = float.Parse(collection[control_index]);
    }
}
