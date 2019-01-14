using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float maxSpeed = 4f;

    Vector3 vecZero = Vector3.zero;
    Rigidbody2D rbody;

    bool playerOnTop = false;

    float lastX = 0f;

    public bool enabled = true;
    public Sprite on;
    public Sprite off;
    SpriteRenderer onOff;
    int inside = 0;

    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        onOff = transform.Find("OnOff").GetComponentInChildren<SpriteRenderer>();
        onOff.sprite = enabled ? on : off;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            rbody.velocity = new Vector2(0.3f * maxSpeed, 0);
        }
        else
        {
            rbody.velocity = new Vector2(0, 0);
        }
        if (playerOnTop && lastX != transform.position.x)
        {
            GameObject.Find("Player Prefab").GetComponent<Player_Movement>().somethingsVelocity = (rbody.velocity);
        }
        lastX = transform.position.x;
    }

    public void Flip()
    {
        maxSpeed *= -1;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && coll.collider.GetType() == typeof(BoxCollider2D))
        {
            print("in");
            //if (inside++ == 0)
            //{
                GameObject.Find("Player Prefab").GetComponent<Player_Movement>().onTopOfSomething = true;
                playerOnTop = true;
            //}
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.collider.GetType() == typeof(BoxCollider2D))
        {
            print("out");
            //if (--inside <= 0)
            //{
                GameObject.Find("Player Prefab").GetComponent<Player_Movement>().onTopOfSomething = false;
                playerOnTop = false;
            //}
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor") || playerFlipCondition(other))
            Flip();
        else if (other.CompareTag("Box"))
        {
            foreach(BoxCollider2D box in this.GetComponents<BoxCollider2D>()){
                if (!box.isTrigger) Physics2D.IgnoreCollision(other, box, true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            foreach (BoxCollider2D box in this.GetComponents<BoxCollider2D>())
            {
                if(!box.isTrigger) Physics2D.IgnoreCollision(other, box, false);
            }
        }
    }

    public void enable()
    {
        enabled = true;
        onOff.sprite = on;
    }

    public void disable()
    {
        enabled = false;
        onOff.sprite = off;
    }

    bool playerFlipCondition(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return false;
        if (other.GetType() != typeof(BoxCollider2D)) return false;
        if (maxSpeed < 0 && other.transform.position.x < transform.position.x) return true;
        if (maxSpeed > 0 && other.transform.position.x > transform.position.x) return true;
        return false;
    }
}
