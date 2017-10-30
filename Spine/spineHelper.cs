using UnityEngine;
using System.Collections;
using Spine.Unity;
using System.Collections.Generic;
using Spine;
using System;
using System.Linq;

public class spineHelper : MonoBehaviour
{
    public bool isActivate = false;//For spine audio manager to call
    public List<AudioClip> audio_collection = new List<AudioClip>();
    private SkeletonAnimator skeleton;
    //private SkeletonAnimation skeletion_animation;

    void Awake()
    {
        skeleton = GetComponent<SkeletonAnimator>();
    }

    #region ANIMATION
    //public void playAnimation(string _animation_name)
    //{
    //    skeletion_animation.state.SetAnimation(0, _animation_name, false);
    //}

    //public void playAnimationLoop(string _animation_name)
    //{
    //    skeletion_animation.state.SetAnimation(0, _animation_name, true);
    //}
    #endregion

    #region SLOTS
    public void setSkin(string _skin_name)
    {
        skeleton.skeleton.SetSkin(_skin_name);
    }
    #endregion

    #region SE CONTROL
    private void soundEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (isActivate)
        {
            try
            {
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().clip =
                   audio_collection.Where(s => s.name.Contains("[" + e.Data.Name + "]"))
                   .OrderBy(rand => Guid.NewGuid()).FirstOrDefault();
                GetComponent<AudioSource>().Play();
            }
            catch (Exception ex)
            {
                //XXX to remove Keys
                //ODM.log("SpineSoundPlayer Empty Event: " + e.Data.Name);
            }
        }
    }
    #endregion
}
