using UnityEngine;
using System.Collections;

public class UISlider : MonoBehaviour {
    //this script is for conversation panel
    private Animator _animator;
    private CanvasGroup _canvas_group;

    public bool isOpen
    {
        get { return _animator.GetBool(ODMVariable.animation.is_open); }
        set { _animator.SetBool(ODMVariable.animation.is_open, value); }
    }

    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvas_group = GetComponent<CanvasGroup>();
    }

}
