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
    private SkeletonAnimation skeletion_animation;
    private AudioSource aud;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        skeletion_animation = GetComponent<SkeletonAnimation>();
        if (skeletion_animation == null) return;
        skeletion_animation.state.Event += soundEvent;
    }

    #region ANIMATION
    public void playAnimation(string _animation_name)
    {
        skeletion_animation.state.SetAnimation(0, _animation_name, false);
    }

    public void playAnimationLoop(string _animation_name)
    {
        skeletion_animation.state.SetAnimation(0, _animation_name, true);
    }
    #endregion

    #region SLOTS
    public void setSkin(string _skin_name)
    {
        skeletion_animation.skeleton.SetSkin(_skin_name);
    }
    #endregion

    #region SE CONTROL
    void soundEvent(Spine.TrackEntry entry, Spine.Event e)
    {
        if (isActivate)
        {
            aud.Stop();
            aud.clip = audio_collection.Where(s => s.name.Contains("[" + e.Data.Name + "]"))
                .OrderBy(rand => Guid.NewGuid()).FirstOrDefault();

            if (aud.clip != null)
                aud.Play();
        }
    }
    #endregion
}
