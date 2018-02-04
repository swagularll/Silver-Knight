using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class weaponDisplay : MonoBehaviour
{
    [SpineAttachment(true)]
    public string pistol;

    [SpineAttachment(true)]
    public string pistol_enhanced;

    //SLOTS
    [SpineSlot]
    public string weapon_slot;

    private SkeletonAnimation skeleton_animation;

    private void Start()
    {
        skeleton_animation = GetComponent<SkeletonAnimation>();
    }

    public void checkWeapon()
    {
        if (ODMObject.event_manager.GetComponent<inventoryDash>().checkItemExist((int)ODMVariable.itemCatalogue.rct_pistol_enhanced))
        {
            skeleton_animation.skeleton.SetAttachment(weapon_slot, pistol_enhanced);
        }
        else
        {
            skeleton_animation.skeleton.SetAttachment(weapon_slot, pistol);
        }
    }

}
