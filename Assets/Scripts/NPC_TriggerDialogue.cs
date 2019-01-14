using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_TriggerDialogue : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("NPC"))
        {
            if(coll.transform.position.x - GameObject.Find("Player Prefab").transform.position.x > 110)
            {
                this.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
            else
            {
                this.gameObject.SetActive(false);
                if(FindObjectOfType<SlowMotion>() != null && !FindObjectOfType<SlowMotion>().isActive()) FindObjectOfType<SlowMotion>().gameObject.SetActive(false);
            }
        }
    }
}
