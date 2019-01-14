using UnityEngine;
using System.Collections;

public class Crawler_Movement : MonoBehaviour {
	
	public float maxSpeed = 4f;
	bool facingPlayer;
	public bool facingRight = false;
	public float aggroDistance = 10;
	
	bool grounded = false;
	public Transform groundCheck;
	//float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	
	float distance;
	GameObject player;
	Animator anim;
    Rigidbody2D rbody;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player Prefab");
		anim = GetComponent<Animator> ();
        rbody = GetComponent<Rigidbody2D>();
		distance = getDistance(player.transform.position.x, transform.position.x);
		anim.SetFloat("DistanceToPlayer", distance);
		facingPlayer = ((player.transform.position.x - transform.position.x) > 0 && facingRight) || ((player.transform.position.x - transform.position.x) < 0 && !facingRight);
	}
	
	// Update is called once per frame
	void Update () {

		if (!GameObject.Find ("Player Prefab").GetComponent<Player_Health> ().isDead ())
        {
			distance = getDistance (player.transform.position.x, transform.position.x);
			anim.SetFloat ("DistanceToPlayer", distance);

			if (distance < aggroDistance && grounded)
            {
				rbody.velocity = new Vector2 ((facingRight ? 1 : -1) * 0.3f * maxSpeed, rbody.velocity.y);
			}
            else
            {
				rbody.velocity = new Vector2 (0, rbody.velocity.y);
			}

			anim.SetFloat ("hSpeed", Mathf.Abs (rbody.velocity.x));
			facingPlayer = ((player.transform.position.x - transform.position.x) >= 0 && facingRight) || ((player.transform.position.x - transform.position.x) <= 0 && !facingRight);
		}
        else
        {
			rbody.velocity = Vector2.zero;
			anim.SetFloat ("hSpeed", 0);
		}
	}
	
	void FixedUpdate(){
		grounded = Mathf.Round(rbody.velocity.y) == 0f;/*Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);*/
		anim.SetBool ("Ground", grounded);
		
		if(!facingPlayer && (Mathf.Abs(player.transform.position.x - transform.position.x)>7))
			Flip();
	}

	float getDistance(float x1, float x2){
		return Mathf.Abs((x1 + Mathf.Abs (x1) + Mathf.Abs (x2))-(x2 + Mathf.Abs (x1) + Mathf.Abs (x2)));
	}

	void Flip(){
		facingRight = !facingRight;
		facingPlayer = !facingPlayer;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;	
	}
}