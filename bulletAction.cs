using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletAction : MonoBehaviour {

    //Shout add bullet hit event*
    public GameObject se_gun_sound;

    public float bullet_speed;
    public float angle;
    public float bullet_virtical_speed;
    public float power;
    private PlayMakerFSM fsm;

    private void Awake()
    {
        fsm = fsmHelper.getFsm(transform.gameObject, ODMVariable.common.default_fsm);
    }

    void Start () {
        transform.Rotate(0, 0, angle * ODMVariable.ava_direction);
        transform.localScale = new Vector3(transform.localScale.x * ODMVariable.ava_direction, transform.localScale.y, transform.localScale.z);
        fsm.FsmVariables.GetFsmFloat(ODMVariable.local.bullet_speed).Value = ODMVariable.ava_direction * bullet_speed;
        fsm.FsmVariables.GetFsmFloat(ODMVariable.local.bullet_virtical_speed).Value = bullet_virtical_speed;
        fsm.enabled = true;
        Instantiate(se_gun_sound);
    }
}
