using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lairSetting : MonoBehaviour
{

    public float bug_birth_rate;
    public GameObject ref_location;
    public bool isGrowingLair = false;
    /// <summary>
    /// The following method will only be called when the lair is activate
    /// -warmbugLair +releaseWarmbugs will do the job
    /// </summary>
    public void initilizeGrowingLair()
    {
        int status = ODMObject.event_manager.GetComponent<sceneObjectManager>().getSceneObjectInfo(GetComponent<sceneObjectInfo>().getIdentifier());
        if (status == 0)
        {
            activateLair();
        }
        else if (status == 1)
        {
            //Lair is destroyed
            //Show lair close sprite
        }
    }

    //public void selfRegister(GameObject _warmbug_lair)
    //{
    //    if (!isGrowingLair)
    //        ODMObject.current_level_lair.GetComponent<warmbugLair>().warmbug_lair_collection.Add(transform.gameObject);
    //}

    public void activateLair()
    {
        if (bug_birth_rate >= 100)
            bug_birth_rate = 100;

        float rand = Random.Range(0, 100.0f);
        if (bug_birth_rate >= rand)
        {
            warmbugLair warmbug_lair = ODMObject.current_level_lair.GetComponent<warmbugLair>();

            GameObject attacker = ODMObject.event_manager.GetComponent<warmbugLairManager>().getRandomAttacker();
            float attacker_position = (float)ref_location.transform.position.x + Random.Range(0, 3f);
            Vector3 bug_position = new Vector3(attacker_position,
                attacker.transform.position.y,
                attacker.transform.position.z);

            //Deploy Warmbug
            GameObject bug = (GameObject)Instantiate(attacker, bug_position, Quaternion.identity);
            bug.GetComponent<warmbugAction>().initilization(warmbug_lair);

            //Register
            lairInfo lair_info = new lairInfo();
            lair_info.bug_name = bug.GetComponent<warmbugAction>().getName();
            lair_info.location_x = attacker_position;
            ODMObject.current_level_lair.GetComponent<warmbugLair>().addBugLairInfo(lair_info);
        }
    }


}
