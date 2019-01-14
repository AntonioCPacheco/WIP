using UnityEngine;
using System.Collections;

public class Statue_Movement : MonoBehaviour
{

    public float maxSpeed = 4f;
    bool facingPlayer;
    public bool facingRight = false;
    public float aggroDistance = 10;
    Rigidbody2D rbody;

    int onEdge = 0;
    bool grounded = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    public bool onTopOfSomething = true;
    public float somethingsVelocity;
    bool isColliding = false;

    bool v_isInLight = false;

    float distance;
    GameObject player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player Prefab");
        distance = getDistance(player.transform.position.x, transform.position.x);
        facingPlayer = ((player.transform.position.x - transform.position.x) > 0 && facingRight) || ((player.transform.position.x - transform.position.x) < 0 && !facingRight);
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<Player_Health>().isDead() && !v_isInLight)
        {
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            distance = getDistance(player.transform.position.x, transform.position.x);
            if (distance < aggroDistance && grounded)
            {
                rbody.velocity = new Vector2((facingRight ? 1 : -1) * 0.3f * maxSpeed, rbody.velocity.y);
            }
            else
            {
                rbody.velocity = new Vector2(0, rbody.velocity.y);
            }
            facingPlayer = ((player.transform.position.x - transform.position.x) >= 0 && facingRight) || ((player.transform.position.x - transform.position.x) <= 0 && !facingRight);
        }
        else
        {
            rbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        if (onTopOfSomething && !isColliding)
        {
            transform.position = new Vector3(transform.position.x + somethingsVelocity, transform.position.y);
            somethingsVelocity = 0f;
        }
    }

    void FixedUpdate()
    {
        if (!v_isInLight)
        {
            grounded = Mathf.Round(rbody.velocity.y) == 0f && onEdge != 0;
           
            if (!facingPlayer && (Mathf.Abs(player.transform.position.x - transform.position.x) > 7))
                Flip();
        }
    }

    float getDistance(float x1, float x2)
    {
        return Mathf.Abs((x1 + Mathf.Abs(x1) + Mathf.Abs(x2)) - (x2 + Mathf.Abs(x1) + Mathf.Abs(x2)));
    }

    void Flip()
    {
        facingRight = !facingRight;
        facingPlayer = !facingPlayer;
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
    
    bool inBetween(Vector3 p1, Vector3 p2)
    {
        if((transform.position.x > p1.x && transform.position.x > p2.x) || (transform.position.x < p1.x && transform.position.x < p2.x))
        {
            return false;
        }
        return true;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("Floor") || coll.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            onEdge++;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Floor") || coll.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            onEdge--;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Floor") || (coll.gameObject.layer == LayerMask.NameToLayer("LightWalls") && !coll.collider.isTrigger))
        {
            isColliding = true;
        }
    }
}