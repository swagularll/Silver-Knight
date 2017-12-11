using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSoundPlayer : MonoBehaviour
{
    public int current_index = -1;
    public int max_index = -1;
    private bool reach_broader = false;

    private AudioSource aud;
    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    public void playButtonSound(int _index)
    {
        if (current_index == -1)
        {
            current_index = 1;
        }
        else if (_index == 0)
        {
            current_index = 1;
            reach_broader = true;
        }
        else if (_index > max_index)
        {
            current_index = max_index;
            reach_broader = true;
        }
        else
        {
            if (reach_broader && (_index == 1 || _index == max_index))
            {
                reach_broader = false;
                aud.clip = Resources.Load<AudioClip>(audioResource.negative_small);
                aud.Play();
            }
            else
            {
                current_index = _index;
                aud.clip = Resources.Load<AudioClip>(audioResource.typer_switch);
                aud.Play();
            }
        }
    }
    public void resetButtonSound()
    {
        current_index =-1;
    }
}
