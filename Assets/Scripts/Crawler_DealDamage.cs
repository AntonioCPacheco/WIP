using UnityEngine;
using System.Collections;

public class Crawler_DealDamage : MonoBehaviour {
	
	GameObject player;
	Collider2D[] playerColliders;
    Collider2D[] thisColliders;

    bool playerTookDamage = false;

	float lastDealt = 0f;
	public float timeBetweenDealing = 1f;
	
	public int damage = 1;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player Prefab");
        playerColliders = player.GetComponents<Collider2D> ();
        thisColliders = GetComponents<Collider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        foreach (Collider2D c in playerColliders)
        {
            if (!c.isTrigger)
            {
                foreach (Collider2D thisC in thisColliders)
                {
                    if (c.IsTouching(thisC)) {
                        DoDamage();
                        return;
                    }
                }
            }
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
