using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class mateBar : MonoBehaviour {
    //renew code
    public void setBarLocation(GameObject _obj_mate_bar_ref)
    {
        ODMObject.obj_matebar_canvas.transform.position = _obj_mate_bar_ref.transform.position;
    }
}
