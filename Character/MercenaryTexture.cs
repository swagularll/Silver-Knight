using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;
using System.Collections.Generic;

public class MercenaryTexture : MonoBehaviour
{
    //small fix
    private SkeletonAnimation s;

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
        s = transform.GetComponent<SkeletonAnimation>();
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
        s.skeleton.SetAttachment(headSlot, normalHeadYellow);
        s.skeleton.SetAttachment(leftHandSlot, LeftHandYellow);
        s.skeleton.SetAttachment(rightHandSlot, RightHandYellow);
        setEquipment();
    }

    public void setPinkShit()
    {
        s.skeleton.SetAttachment(headSlot, normalHeadPink);
        s.skeleton.SetAttachment(leftHandSlot, LeftHandPink);
        s.skeleton.SetAttachment(rightHandSlot, RightHandPink);
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
                s.skeleton.SetAttachment(armorSlot, armorA);
                break;
            case 1:
                s.skeleton.SetAttachment(armorSlot, armorB);
                break;
            case 2:
                s.skeleton.SetAttachment(armorSlot, armorC);
                break;
        }


        if (convertFloatToBool(haveWeapon))
        {
            s.skeleton.SetAttachment(weaponSlot, gun);
            s.skeleton.SetAttachment(sideKnifeSlot, knifeSide);
            weaponStyle = true;

        }
        else
        {
            s.skeleton.SetAttachment(weaponSlot, knife);
            s.skeleton.SetAttachment(sideKnifeSlot, null);

        }

        if (convertFloatToBool(haveRightPack))
        {
            s.skeleton.SetAttachment(packLeft, packLeft);
        }
        else
        {
            s.skeleton.SetAttachment(packLeft, null);

        }

        if (convertFloatToBool(haveLeftPack))
        {
            s.skeleton.SetAttachment(packRight, packRight);

        }
        else
        {
            s.skeleton.SetAttachment(packRight, null);
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
