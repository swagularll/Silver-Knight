using UnityEngine;
using System.Collections;

public class lightHelper : MonoBehaviour
{
    public bool defaultOpen = false;

    void Start()
    {
        if (!defaultOpen)
            closeLight();
    }

    public void closeLight()
    {
        transform.GetComponent<Light>().enabled = false;
    }

    public void openLight()
    {
        transform.GetComponent<Light>().enabled = true;
    }
}
