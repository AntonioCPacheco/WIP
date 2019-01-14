using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueAfterTime : MonoBehaviour {

    public DialogueTrigger dt;
    public float timeToTrigger = 8f;
    float triggerTime = 0f;

    bool triggered = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (!triggered && triggerTime != 0 && Time.realtimeSinceStartup - triggerTime >= timeToTrigger) trigger();
	}

    public void arm()
    {
        triggerTime = Time.realtimeSinceStartup;
    }

    void trigger()
    {
        dt.TriggerDialogue();
        triggered = true;
    }

    public void disarm()
    {
        triggered = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<NPC_Movement>().followPlayer = false;
            disarm();
        }
    }
}