using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class collisionControl : MonoBehaviour {

    // default position
    private float standSizeY = 6.18f;
    private float standOffsetY = 3.17f;

    // squat collision space;
    private float squatSizeY = 3.75f;
    private float squatOffsetY = 1.75f;

    // crawl collision space;
    private float crawlSizeY = 2f;
    private float crawlOffsetY = 1f;

    public void squatSpace()
    {
        BoxCollider2D area = FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value.GetComponent<BoxCollider2D>();
        area.size = new Vector2(area.size.x, squatSizeY);
        area.offset = new Vector2(area.offset.x, squatOffsetY);
    }

    public void standSpace()
    {
        BoxCollider2D area = FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value.GetComponent<BoxCollider2D>();
        area.size = new Vector2(area.size.x, standSizeY);
        area.offset = new Vector2(area.offset.x, standOffsetY);
    }

    public void crawlSpace()
    {
        BoxCollider2D area = FsmVariables.GlobalVariables.GetFsmGameObject("Ava").Value.GetComponent<BoxCollider2D>();
        area.size = new Vector2(area.size.x, crawlSizeY);
        area.offset = new Vector2(area.offset.x, crawlOffsetY);
    }
}
