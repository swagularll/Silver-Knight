using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class spineAudioManager : MonoBehaviour
{
    private GameObject previous_model;
    public void setSpineVoicePlayer(GameObject _newModel)
    {
        try
        {
            if (previous_model != null)
                previous_model.GetComponent<spineHelper>().isActivate = false;
            previous_model = _newModel;
            _newModel.GetComponent<spineHelper>().isActivate = true;
        }
        catch (Exception ex)
        {
            ODM.errorLog(this.transform.name,"", ex.ToString());
        }
    }
}
