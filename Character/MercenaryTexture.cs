using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;
using System.Collections.Generic;

public class MercenaryTexture : MonoBehaviour
{
    //small fix
    private SkeletonAnimation skeleton_animation;

    public bool isPink = true;
    public bool isYellow = true;
    public bool weaponStyle = false; //gun = 0; Knife = 1;
    private float spineLayerLocationZ = 0.02f;

    //Heads
    [SpineAttachment(true)]
    public string deadHeadPink;

    [SpineAttachment(true)]
    public string deadHeadYellow;

    [SpineAttachment(true)]
    public string normalHeadPink;

    [SpineAttachment(true)]
    public string normalHeadYellow;


    //Hands
    [SpineAttachment(true)]
    public string LeftHandYellow;

    [SpineAttachment(true)]
    public string LeftHandPink;

    [SpineAttachment(true)]
    public string RightHandYellow;

    [SpineAttachment(true)]
    public string RightHandPink;


    //Hood
    [SpineAttachment(true)]
    public string hood;

    [SpineAttachment(true)]
    public string hoodBlood;

    //Toung
    [SpineAttachment(true)]
    public string toungGreen;

    [SpineAttachment(true)]
    public string toungPurple;


    //Armor
    [SpineAttachment(true)]
    public string armorA;

    [SpineAttachment(true)]
    public string armorB;

    [SpineAttachment(true)]
    public string armorC;

    //Weapon
    [SpineAttachment(true)]
    public string gun;

    [SpineAttachment(true)]
    public string knife;

    [SpineAttachment(true)]
    public string knifeSide;



    //SLOTS
    [SpineSlot]
    public string headSlot;

    [SpineSlot]
    public string leftHandSlot;

    [SpineSlot]
    public string rightHandSlot;

    [SpineSlot]
    public string weaponSlot;

    [SpineSlot]
    public string sideKnifeSlot;

    [SpineSlot]
    public string packRight;

    [SpineSlot]
    public string packLeft;

    [SpineSlot]
    public string armorSlot;

    [SpineSlot]
    public List<string> slot_collection;

    [SpineAttachment(true)]
    public List<string> attachment_collection;
    //[SpineSlot]
    //public string leftHandSlot;

    //[SpineSlot]
    //public string rightHandSlot;

    // Use this for initialization
    void Start()
    {
        skeleton_animation = transform.GetComponent<SkeletonAnimation>();
        if (isPink && isYellow || !isPink && !isYellow)//random
        {
            bool b = convertFloatToBool(Random.value);
            if (b)
            {
                setYellowShit();
            }
            else
            {
                setPinkShit();
            }
        }
        else if (isPink)
        {
            setPinkShit();
        }
        else if (isYellow)
        {
            setYellowShit();
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, spineLayerLocationZ);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setYellowShit()
    {
        skeleton_animation.skeleton.SetAttachment(headSlot, normalHeadYellow);
        skeleton_animation.skeleton.SetAttachment(leftHandSlot, LeftHandYellow);
        skeleton_animation.skeleton.SetAttachment(rightHandSlot, RightHandYellow);
        setEquipment();
    }

    public void setPinkShit()
    {
        skeleton_animation.skeleton.SetAttachment(headSlot, normalHeadPink);
        skeleton_animation.skeleton.SetAttachment(leftHandSlot, LeftHandPink);
        skeleton_animation.skeleton.SetAttachment(rightHandSlot, RightHandPink);
        setEquipment();
    }
    private void setEquipment()
    {
        int armor = (int)(Random.value * 10) % 3;
        float haveWeapon = Random.value;
        float haveRightPack = Random.value;
        float haveLeftPack = Random.value;


        switch (armor)
        {
            case 0:
                skeleton_animation.skeleton.SetAttachment(armorSlot, armorA);
                break;
            case 1:
                skeleton_animation.skeleton.SetAttachment(armorSlot, armorB);
                break;
            case 2:
                skeleton_animation.skeleton.SetAttachment(armorSlot, armorC);
                break;
        }


        if (convertFloatToBool(haveWeapon))
        {
            skeleton_animation.skeleton.SetAttachment(weaponSlot, gun);
            skeleton_animation.skeleton.SetAttachment(sideKnifeSlot, knifeSide);
            weaponStyle = true;

        }
        else
        {
            skeleton_animation.skeleton.SetAttachment(weaponSlot, knife);
            skeleton_animation.skeleton.SetAttachment(sideKnifeSlot, null);

        }

        if (convertFloatToBool(haveRightPack))
        {
            skeleton_animation.skeleton.SetAttachment(packLeft, packLeft);
        }
        else
        {
            skeleton_animation.skeleton.SetAttachment(packLeft, null);

        }

        if (convertFloatToBool(haveLeftPack))
        {
            skeleton_animation.skeleton.SetAttachment(packRight, packRight);

        }
        else
        {
            skeleton_animation.skeleton.SetAttachment(packRight, null);
        }
    }

    private bool convertFloatToBool(float f)
    {
        //Debug.Log(f);
        if (f > 0.5f)
        {
            return true;
        }
        return false;
    }
}
