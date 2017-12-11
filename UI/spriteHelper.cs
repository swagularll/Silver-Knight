using UnityEngine;
using System.Collections;

public class spriteHelper : MonoBehaviour
{
    public bool is_invisible = false;

    void Start()
    {
        if (is_invisible)
        {
            GetComponent<SpriteRenderer>().color = ODMVariable.color.invisible;
        }
    }
}