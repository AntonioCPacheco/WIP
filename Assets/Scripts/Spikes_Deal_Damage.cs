using UnityEngine;
using System.Collections;

public class Spikes_Deal_Damage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name == "Player Prefab")
        {
            coll.GetComponent<Checkpoint_Handler>().returnToLastCheckpoint();
            GameObject.Find("DeathScreen").GetComponent<Animator>().SetTrigger("Die");
            VibrateController.vibrateControllerForXSeconds(0.5f, 0.3f, 0.3f);
        }
    }
}
