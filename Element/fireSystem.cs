using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireSystem : MonoBehaviour
{

    public GameObject bullet_generate_locaiton;
    public GameObject bullet_generate_locaiton_up;
    public GameObject bullet_generate_locaiton_down;

    public List<GameObject> bullet_collection;
    public List<GameObject> bullet_collection_up;
    public List<GameObject> bullet_collection_down;

    //private inventoryDash inventory_dash;

    private void Start()
    {
        //Set bullet power

    }

    public void fire()
    {
        if (ODMVariable.ava_current_weapon != -1)
        {
            GameObject bullet = (GameObject)Instantiate(bullet_collection[ODMVariable.ava_current_weapon - 1], bullet_generate_locaiton.transform.position, Quaternion.identity);
            takeAwayBullet();
        }
    }


    public void fireUp()
    {
        if (ODMVariable.ava_current_weapon != -1)
        {
            GameObject bullet = (GameObject)Instantiate(bullet_collection_up[ODMVariable.ava_current_weapon - 1], bullet_generate_locaiton_up.transform.position, Quaternion.identity);
            takeAwayBullet();
        }
    }

    public void fireDown()
    {
        if (ODMVariable.ava_current_weapon != -1)
        {
            GameObject bullet = (GameObject)Instantiate(bullet_collection_down[ODMVariable.ava_current_weapon - 1], bullet_generate_locaiton_down.transform.position, Quaternion.identity);
            takeAwayBullet();
        }
    }

    private void takeAwayBullet()
    {
        inventoryDash inventory_dash = ODMObject.event_manager.GetComponent<inventoryDash>();

        if (inventory_dash.hasEnhanced())
            inventory_dash.checkAndTakeAway((int)ODMVariable.itemCatalogue.rct_pistol_enhanced);
        else
            inventory_dash.checkAndTakeAway((int)ODMVariable.itemCatalogue.rct_pistol);
    }
}
