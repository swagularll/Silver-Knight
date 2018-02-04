using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageAction : MonoBehaviour {
    public List<float> damage_collection;
    public string fsm_name = "Health System";
    private PlayMakerFSM fsm;
    private void Start()
    {
       fsm = fsmHelper.getFsm(transform.gameObject, fsm_name);
    }

    public void applyDamage(int _bullet_type)
    {
        float damage = damage_collection[_bullet_type - 1];
        fsm.FsmVariables.GetFsmFloat(ODMVariable.local.bullet_power).Value = damage;
        fsm.SendEvent(eventName.get_hit);
    }
}
