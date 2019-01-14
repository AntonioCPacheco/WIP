using UnityEngine;
using System.Collections;

public class Eye_Movement : MonoBehaviour {

	public float minSpeed = 40f;
	//bool facingRight = false;
	public float aggroDistance = 20;
	//public LayerMask whatIsGround;
	public float force = 10f;

	GameObject player;
	Animator anim;
	Rigidbody2D rbody;
	float distance;
	SpriteRenderer sprR;


	Vector3 lastDir;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player Prefab");
		distance = (player.transform.position - transform.position).magnitude;
		anim = GetComponent<Animator> ();
		rbody = GetComponent<Rigidbody2D> ();
		sprR = GetComponent<SpriteRenderer> ();
		lastDir = Vector3.zero;

	}

    // Update is called once per frame
    void Update() {
        if (!GameObject.Find("Player Prefab").GetComponent<Player_Health>().isDead()) {
            distance = (player.transform.position - transform.position).magnitude;

            if (distance < aggroDistance && rbody.velocity.magnitude < minSpeed) {
                rbody.velocity = GetVelocityVector();
                Flip();
            }
        } else {
            rbody.velocity = Vector2.zero;
        }
        if (Random.Range(0, 100) > 97)
            anim.SetTrigger("wink");        
    }

    void FixedUpdate()
    {
        Vector3 vNorm = rbody.velocity.normalized;
        if (lastDir != new Vector3(vNorm.x, vNorm.y))
        {
            lastDir = new Vector3(vNorm.x, vNorm.y);
            updateRotation();
        }
    }

	void Flip(){
        updateRotation();
        lastDir = new Vector3(rbody.velocity.normalized.x, rbody.velocity.normalized.y);
	}

	Vector3 GetVelocityVector() {
		Vector3 aux = (player.transform.position - transform.position).normalized;
		aux.x = aux.x * force;
		aux.y = aux.y * force;
		return aux;
	}

    void updateRotation()
    {
        sprR.transform.rotation = Quaternion.Euler(0, 0, (lastDir.y < 0 ? -1 : 1) * Vector3.Angle(new Vector3(1, 0, 0), lastDir));
    }
}
