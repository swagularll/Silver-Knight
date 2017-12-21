using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneObjectInfo : MonoBehaviour {

    public string identifier;

    private void Awake()
    {
        identifier = ODMVariable.convert.getSceneIdentifier(Application.loadedLevelName, identifier);
    }

    public string getIdentifier()
    {
        return identifier;
    }

}
