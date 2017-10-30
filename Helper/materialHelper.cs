using UnityEngine;
using System.Collections;

public class materialHelper : MonoBehaviour
{
    public bool isSkeleton = false;
    private Shader litShader;
    void Start()
    {
        if (isSkeleton)
            GetComponent<Renderer>().material.shader = Shader.Find("Spine/Skeleton Lit");
        else
            GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Diffuse");
    }

}
