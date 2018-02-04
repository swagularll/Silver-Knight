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

    #region For Call
    public void registerSceneObject(GameObject _level_loader)
    {
        _level_loader.GetComponent<afterLoad>().addToSceneObjectCollection(transform.gameObject);
    }
    #endregion

}
