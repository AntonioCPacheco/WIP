using UnityEngine;
using System.Collections;

public class Eye_DealDamage : MonoBehaviour {

	GameObject player;
	Collider2D playerPolygonCollider;
	Collider2D playerBoxCollider;

	bool playerTookDamage = false;

	float lastDealt = 0f;
	public float timeBetweenDealing = 1f;

	public int damage = 1;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player Prefab");
		playerPolygonCollider = player.GetComponent<PolygonCollider2D> ();
		playerBoxCollider = player.GetComponent<BoxCollider2D> ();
	}

	// Update is called once per frame
	void Update () {
		if (playerPolygonCollider.IsTouching(GetComponent<CircleCollider2D>()) && !playerBoxCollider.IsTouching(GetComponent<CircleCollider2D>()) && (Time.time > lastDealt + timeBetweenDealing)) {
			DoDamage();
		}
	}

	void DoDamage(){
		playerTookDamage = GameObject.Find ("Player Prefab").GetComponent<Player_Health> ().TakeDamage (damage);
		if (playerTookDamage) {
			lastDealt = Time.time;
			Debug.Log (this.GetInstanceID() + " dealt damage");
		}
	}
}
