using UnityEngine;
using System.Collections;

public class UISlider : MonoBehaviour {
    //this script is for conversation panel
    private Animator _animator;
    private CanvasGroup _canvasGroup;

    public bool isOpen
    {
        get { return _animator.GetBool("isOpen"); }
        set { _animator.SetBool("isOpen", value); }
    }

    public void Awake()
    {
        //Debug.Log("UISlider = " + transform.name); // currently only conversation panel is using this script...
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

}
