using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class modelHelper : MonoBehaviour {

    public void setBarLocation(GameObject mateBarLocation)
    {
        FsmVariables.GlobalVariables.GetFsmGameObject("obj_Matebar_Canvas").Value.transform.position = mateBarLocation.transform.position;
    }
}
