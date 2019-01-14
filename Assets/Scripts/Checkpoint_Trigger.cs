using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint_Trigger : MonoBehaviour {

    private bool triggered = false;
    public Sprite checkpointOn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!triggered && coll.CompareTag("Player"))
        {
            triggered = true;
            SaveManager.saveGame();

            GetComponent<AudioSource>().Play();

            GetComponent<SpriteRenderer>().sprite = checkpointOn;
            /*Checkpoint_Handler ch = coll.gameObject.GetComponent<Checkpoint_Handler>();
            ch.checkpoint = transform.GetChild(0).transform;*/
        }
    }
}
