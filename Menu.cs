using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Animator _animator;
    public float widthPercent = -1;
    public float heightPercent = -1;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _animator = GetComponent<Animator>();
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);//set to the middle while start the game

        if (widthPercent != -1)
        {
            GetComponent<RectTransform>().sizeDelta =
                new Vector2(Screen.width / 100 * widthPercent, transform.GetComponent<RectTransform>().sizeDelta.y);
        }

        if (heightPercent != -1)
        {
            GetComponent<RectTransform>().sizeDelta =
                new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, Screen.width / 100 * heightPercent);
        }
    }
    public bool isOpen
    {
        get { return _animator.GetBool("isOpen"); }
        set { _animator.SetBool("isOpen", value); }
    }

}
