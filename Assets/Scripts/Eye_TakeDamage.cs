using UnityEngine;
using System.Collections;

public class Eye_TakeDamage : MonoBehaviour {

	GameObject player;
	Collider2D playerCollider;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player Prefab");
		playerCollider = player.GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (playerCollider.IsTouching(GetComponent<CircleCollider2D>()) /*&& !player.GetComponent<Player_Info>().isInvincible()*/) {
			TakeDamageFunction();
		}
	}

	void TakeDamageFunction(){
		GameObject.Find ("Player Prefab").GetComponent<Player_Movement> ().jumpOnEnemy ();
		Destroy (this.gameObject);
		Debug.Log (this.GetInstanceID() + " died.");
	}
}
