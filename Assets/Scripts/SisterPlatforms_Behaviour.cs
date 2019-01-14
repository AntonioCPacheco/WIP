using UnityEngine;
using System.Collections;

public class SisterPlatforms_Behaviour : MonoBehaviour {

    public SisterPlatforms_Behaviour sisterPlatform;

	// Use this for initialization
	void Start () {
        if (sisterPlatform == null)
        {
            sisterPlatform = this;
        }
        GetComponent<SpriteRenderer>().color = GetComponent<Collider2D>().isTrigger ? Color.cyan : Color.blue;
        /*else if(sisterPlatform.isTriggered() && isTriggered())
        {
            sisterPlatform.getTriggered();
        }*/
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void getTriggered(bool trigger)
    {
        GetComponent<Collider2D>().isTrigger = trigger;
        GetComponent<SpriteRenderer>().color = trigger ? Color.cyan : Color.blue;
    }

    bool isTriggered()
    {
        return GetComponent<Collider2D>().isTrigger;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            this.getTriggered(false);
            sisterPlatform.getTriggered(true);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            this.getTriggered(false);
            sisterPlatform.getTriggered(true);
        }
    }
}
