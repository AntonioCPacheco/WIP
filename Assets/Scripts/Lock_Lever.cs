using UnityEngine;
using System.Collections;

public class Lock_Lever : Lock_SuperClass {

    public Sprite sprTriggered;
    public Sprite sprUnTriggered;

    Transform player;
    
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player Prefab").transform;
        }
        SuperStart();
    }

    protected override bool isBeingTriggered()
    {
        if((Input.GetKeyDown(KeyCode.S) && player.GetComponent<PolygonCollider2D>().OverlapPoint(transform.Find("Open Point").GetComponent<Transform>().position)))
        {
            transform.GetComponent<SpriteRenderer>().sprite = sprTriggered;
            return true;
        }
        return false;
    }

    protected override void childUnTrigger()
    {
        return;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        print("Lever - Trigger Enter 2D Not Implemented");
    }
}
