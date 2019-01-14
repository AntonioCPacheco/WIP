using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turtle_Movement : MonoBehaviour {

    public float maxSpeed = 4f;
    public bool facingRight = false;
    public float delay = 2f;

    Vector3 vecZero = Vector3.zero;
    Rigidbody2D rbody;

    bool grounded = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    bool playerOnTop = false;

    float lastX = 0f;

    List<Statue_Movement> statuesOnTop;

    bool v_isInLight = false;
    float timeInLight = 0f;

    // Use this for initialization
    void Start () {
        rbody = GetComponent<Rigidbody2D>();
        statuesOnTop = new List<Statue_Movement>();
    }
	
	// Update is called once per frame
	void Update () {
        if (v_isInLight)
        {
            if (timeInLight >= delay)
            {
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                rbody.velocity = new Vector2((facingRight ? 1 : -1) * 0.3f * maxSpeed, rbody.velocity.y);
                if (playerOnTop && lastX != transform.position.x)
                {
                    GameObject.Find("Player Prefab").GetComponent<Player_Movement>().somethingsVelocity = (rbody.velocity);
                }
                foreach(Statue_Movement st in statuesOnTop)
                {
                    if (lastX != transform.position.x)
                        st.somethingsVelocity = (transform.position.x - lastX);
                }
            }
            else
            {
                timeInLight += Time.deltaTime;
                if(timeInLight >= delay)
                {
                    Flip();
                }
            }
        }
        else
        {
            timeInLight = 0;
            rbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        lastX = transform.position.x;
    }

    void FixedUpdate()
    {
        if (v_isInLight)
        {
            grounded = Mathf.Round(rbody.velocity.y) == 0f;
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void InLight(bool l)
    {
        v_isInLight = l;
    }

    public bool IsInLight()
    {
        return v_isInLight;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Floor") || other.gameObject.layer == LayerMask.NameToLayer("Statues") || (other.gameObject.layer == LayerMask.NameToLayer("LightWalls") && !other.isTrigger))
            Flip();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && coll.collider.GetType() == typeof(BoxCollider2D))
        {
            GameObject.Find("Player Prefab").GetComponent<Player_Movement>().onTopOfSomething = true;
            playerOnTop = true;
        }
        if (coll.gameObject.CompareTag("Statue"))
        {
            statuesOnTop.Add(coll.gameObject.GetComponent<Statue_Movement>());
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.collider.GetType() == typeof(BoxCollider2D))
        {
            GameObject.Find("Player Prefab").GetComponent<Player_Movement>().onTopOfSomething = false;
            playerOnTop = false;
        }
        if (col.gameObject.CompareTag("Statue"))
        {
            statuesOnTop.Remove(col.gameObject.GetComponent<Statue_Movement>());
        }
    }
}
