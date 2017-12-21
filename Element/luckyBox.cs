using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class luckyBox : MonoBehaviour
{

    public List<GameObject> reward_collection;
    public List<int> amount_collection;

    private string identifier;
    private PlayMakerFSM fsm;

    private Animator ani;
    private itemManager item_manager;
    private sceneObjectManager scene_object_manager;
    private sceneObjectInfo info;
    private void Awake()
    {
        #region Configuration check
        if (reward_collection.Count != amount_collection.Count)
        {
            ODM.errorLog(transform.name, "Error: reward_collection count and amount_collection count are not matching.");
        }
        #endregion

        ani = GetComponent<Animator>();
        info = GetComponent<sceneObjectInfo>();

    }

    public void Start()
    {
        item_manager = ODMObject.event_manager.GetComponent<itemManager>();
        scene_object_manager = ODMObject.event_manager.GetComponent<sceneObjectManager>();
        fsm = fsmHelper.getFsm(transform.gameObject, ODMVariable.common.default_fsm);

        identifier = info.getIdentifier();
        int status = scene_object_manager.getSceneObjectInfo(identifier);
        switch (status)
        {
            case 0:
                fsm.enabled = true;//FSM will get disabled from FSM side.
                break;
            case 1://Open
                ani.SetBool(ODMVariable.animation.is_open_final, true);
                break;
            case 2://Crack
                ani.SetBool(ODMVariable.animation.is_crack_final, true);
                break;
        }

    }

    public void openBox()
    {
        ani.SetBool(ODMVariable.animation.is_open, true);
        generateReward();
        scene_object_manager.setSceneObjectInfo(identifier, 1);
    }
    public void crackBox()
    {
        ani.SetBool(ODMVariable.animation.is_crack, true);
        generateReward();
        scene_object_manager.setSceneObjectInfo(identifier, 2);
    }
    private void generateReward()
    {
        for (int i = 0; i < reward_collection.Count; i++)
        {
            item_manager.createItem(((int)reward_collection[i].GetComponent<itemSetting>().catalog_code).ToString(),
                amount_collection[i], ODM.getRandomPositionX(transform.position.x));
        }
    }
}
