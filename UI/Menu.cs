using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{

    private float width_percentage = -1;
    private float height_percentage = -1;
    private CanvasGroup _canvasGroup;
    private Animator _animator;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _animator = GetComponent<Animator>();
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);//set to the middle while start the game

        if (width_percentage != -1)
        {
            GetComponent<RectTransform>().sizeDelta =
                new Vector2(Screen.width / 100 * width_percentage, transform.GetComponent<RectTransform>().sizeDelta.y);
        }

        if (height_percentage != -1)
        {
            GetComponent<RectTransform>().sizeDelta =
                new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, Screen.width / 100 * height_percentage);
        }
    }
    public bool isOpen
    {
        get { return _animator.GetBool(ODMVariable.animation.is_open); }
        set { _animator.SetBool(ODMVariable.animation.is_open, value); }
    }

}
