using UnityEngine;
using System.Collections;

public class Crawler_TakeDamage : MonoBehaviour {

	GameObject player;
	Collider2D playerEdgeCollider;
    Collider2D[] thisColliders;

    // Use this for initialization
    void Start () {
		player = GameObject.Find ("Player Prefab");
        playerEdgeCollider = player.GetComponent<EdgeCollider2D>();
        thisColliders = GetComponentsInChildren<Collider2D>();
    }

	// Update is called once per frame
	void Update ()
    {
        thisColliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D c in thisColliders)
        {
            if (!c.isTrigger && playerEdgeCollider.IsTouching(c)/* && !playerPolygonCollider.IsTouching(GetComponent<PolygonCollider2D>())*/ && !player.GetComponent<Player_Info>().isInvincible())
            {
                TakeDamageFunction();
                break;
            }
        }
	}

	void TakeDamageFunction(){
        if(this.GetComponent<Blob_SpawnerBehaviour>() != null) this.GetComponent<Blob_SpawnerBehaviour>().triggerSpawner();
        GameObject.Find ("Player Prefab").GetComponent<Player_Movement> ().jumpOnEnemy ();
		Destroy (this.gameObject);
		Debug.Log (this.GetInstanceID() + " died.");
	}
}
