using UnityEngine;
using System.Collections;

public class spriteHelper : MonoBehaviour
{
    public bool isInvisible = false;

    void Start()
    {
        if (isInvisible)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
    }
}