using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using Assets.Script.ODM_Widget;

public class cameraControl : MonoBehaviour
{
    public GameObject defaultLocation;
    public bool isTracing = true;

    private Animator animator;
    private Vector3 originalLocation;
    private Vector3 finalLocation;
    private float fixedCameraY = 2;
    private float originalCameraY = 4.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void setFocus()
    {
        if (animator.GetBool("isConversation"))
            endConversation();
        animator.SetBool("isFocus", true);
    }
    public void setFocusBack()
    {
        animator.SetBool("isFocus", false);
    }

    public void setConversation()
    {
        isTracing = false;
        if (animator.GetBool("isFocus"))
            setFocusBack();
        animator.SetBool("isConversation", true);
        finalLocation = new Vector3(transform.position.x, fixedCameraY, transform.position.z);
        iTween.MoveTo(transform.gameObject, iTween.Hash("position", finalLocation, "easetype", iTween.EaseType.easeInOutSine, "time", 1f));
    }
    public void endConversation()
    {
        animator.SetBool("isConversation", false);
        finalLocation = new Vector3(transform.position.x, originalCameraY, transform.position.z);
        iTween.MoveTo(transform.gameObject, iTween.Hash("position", finalLocation, "easetype", iTween.EaseType.easeInOutSine, "time", 1f));
    }
    public void setCameraTarget(GameObject _obj)
    {
        isTracing = false;
        //finalLocation = new Vector3(_obj.transform.position.x, _obj.transform.position.y, transform.position.z);
        if (animator.GetBool("isConversation"))
            finalLocation = new Vector3(_obj.transform.position.x, fixedCameraY, transform.position.z);
        else
            finalLocation = new Vector3(_obj.transform.position.x, transform.position.y, transform.position.z);

        iTween.MoveTo(transform.gameObject, iTween.Hash("position", finalLocation, "easetype", iTween.EaseType.easeInOutSine, "time", 1f));
    }
    public void setCameraTargetPrecise(GameObject _obj)
    {
        isTracing = false;
        finalLocation = new Vector3(_obj.transform.position.x, _obj.transform.position.y, transform.position.z);
        iTween.MoveTo(transform.gameObject, iTween.Hash("position", finalLocation, "easetype", iTween.EaseType.easeInOutSine, "time", 1f));
    }
    public void setCameraAva()
    {
        isTracing = false;
        float avaPositionX = ODMObject.character_ava.transform.position.x;
        finalLocation = new Vector3(avaPositionX, transform.position.y, transform.position.z);
        iTween.MoveTo(transform.gameObject, iTween.Hash("position", finalLocation, "easetype", iTween.EaseType.easeInOutSine, "time", 1f));
    }
    public void moveCameraOriginal()
    {
        animator.SetBool("isFocus", false);
        animator.SetBool("isConversation", false);
        iTween.MoveTo(transform.gameObject, iTween.Hash("position", defaultLocation.transform.position, "easetype", iTween.EaseType.easeInOutSine, "time", 1f));
    }
    public void setCameraDefault()
    {
        //target = null;
        transform.SetParent(ODMObject.character_ava.transform);
        isTracing = true;
    }

    public void followTarget(GameObject _target)
    {
        transform.SetParent(_target.transform);
        isTracing = false;
    }

    void FixedUpdate()
    {
        if (isTracing)
            iTween.MoveUpdate(transform.gameObject, defaultLocation.transform.position, 0.1f);
    }
}
