using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneObjectManager : MonoBehaviour
{
    private ODM.ODMDictionary scene_object_collection;
    private void Awake()
    {
        scene_object_collection = new ODM.ODMDictionary();
    }

    private void Start()
    {
        scene_object_collection = saveRecord.getCurrentRecord().scene_info_colleciton;
    }
    public void registerSceneInfo(string _identifier)
    {
        scene_object_collection.add(_identifier, "0");//default = 0;
    }

    public void setSceneObjectInfo(string _identifier,int _value)
    {
        scene_object_collection.setValue(_identifier, _value.ToString());
    }

    public void setSceneObjectInfo(string _identifier, string _value)
    {
        scene_object_collection.setValue(_identifier, _value);
    }

    public ODM.ODMDictionary getSceneObjectInfoCollection()
    {
        return scene_object_collection;
    }

    public int getSceneObjectInfo(string _identifier)
    {
        return Int32.Parse(scene_object_collection.getValue(_identifier));
    }

}
