using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_DisplayLockStates : MonoBehaviour {

    public Sprite activated;
    public Sprite deactivated;
    public float spaceBetweenIndicators = 50f;
    public float moveUp = 100f;
    
    int numLocks = 0;
    int numTriggered = 0;

    Transform[] indicators;
	// Use this for initialization
	void Start () {
        numLocks = this.GetComponentInParent<Player_EnterDoors>().locks.Length;
        indicators = new Transform[numLocks];

        GameObject newObject = new GameObject();
        newObject.transform.SetParent(this.transform);

        SpriteRenderer spr = newObject.AddComponent<SpriteRenderer>();
        spr.sprite = deactivated;

        for (int i=0; i<numLocks; i++)
        {
            indicators[i] = Instantiate(newObject).transform;
            indicators[i].SetParent(this.transform);
            Vector2 translate = new Vector2(((i - ((numLocks - 1)/2f)) * (spaceBetweenIndicators)), moveUp);
            indicators[i].localPosition = translate; 
        }
        GameObject.Destroy(newObject);
    }
	
	public void triggerLock () {
        if (numTriggered >= numLocks) return;
        indicators[numTriggered].GetComponent<SpriteRenderer>().sprite = activated;
        numTriggered++;
	}

    public void updateIndicators(int numLocksTriggered)
    {
        //if (numLocksTriggered == numTriggered) return;

        for (int i=0; i<indicators.Length; i++)
        {
            indicators[i].GetComponent<SpriteRenderer>().sprite = (numLocksTriggered > 0 ? activated : deactivated);
            numLocksTriggered--;
        }
    }
}
