using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob_Movement : MonoBehaviour {

    public float maxSpeed = 2f;
    public bool facingRight = false;
    public float delay = 2f;
    public bool needsLight = true;
    public float minDistanceToPlayer = 200;

    //Vector3 vecZero = Vector3.zero;
    Rigidbody2D rbody;

    bool grounded = false;

    bool v_isInLight = false;
    float distance_to_player = 0f;
    float timeInLight = 0f;
    bool triggered = false;

    public GameObject idleColliders;
    public GameObject movingColliders;

    Animator anim;

    // Use this for initialization
    void Start()
    {
        distance_to_player = (this.transform.position - GameObject.Find("Player Prefab").transform.position).magnitude;
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distance_to_player = (this.transform.position - GameObject.Find("Player Prefab").transform.position).magnitude;
        if (!triggered)
        {
            if (v_isInLight || !needsLight)
            {
                if ((timeInLight >= delay || !needsLight) && distance_to_player <= minDistanceToPlayer)
                {
                    triggered = true;
                    rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rbody.velocity = new Vector2((facingRight ? 1 : -1) * maxSpeed, rbody.velocity.y);
                    anim.SetBool("Moving", true);
                    switchColliders();
                }
                else if(v_isInLight && distance_to_player <= minDistanceToPlayer)
                {
                    anim.SetBool("WakingUp", true);
                    timeInLight += Time.deltaTime;
                }
            }
            else
            {
                anim.SetBool("WakingUp", false);
                timeInLight = 0;
                rbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else
        {
            anim.SetBool("Moving", true);
            rbody.velocity = new Vector2((facingRight ? 1 : -1) * maxSpeed, rbody.velocity.y);
        }
    }

    void FixedUpdate()
    {
        grounded = Mathf.Round(rbody.velocity.y) == 0f;
    }

    void switchColliders()
    {
        idleColliders.SetActive(!idleColliders.activeSelf);
        movingColliders.SetActive(!movingColliders.activeSelf);
    }

    public void Flip()
    {
        Debug.Log("***TRIGGERED***");
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor") || other.gameObject.layer == LayerMask.NameToLayer("Statues") || (other.gameObject.layer == LayerMask.NameToLayer("LightWalls") && !other.isTrigger))
            Flip();
    }
}