using UnityEngine;
using System.Collections;

public class Camera_Position_Snapper : MonoBehaviour {

    float lastTimeCalled;
    public float delay = 0.1f;
    bool lastFollowPlayer = false;
    Vector3 lastPosition;

	// Use this for initialization
	void Start () {
        lastTimeCalled = delay + 1;
        lastPosition = transform.GetChild(0).position;

    }
	
	// Update is called once per frame
	void Update () {
	    if(lastTimeCalled < delay)
        {
            lastTimeCalled += Time.deltaTime;
        }
	}

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.name.Equals("Player Prefab") && lastTimeCalled >= delay)
        {
            lastTimeCalled = 0f;
            Camera_Movement cm = Camera.main.GetComponent<Camera_Movement>();
            if (cm.followPlayer)
            {
                cm.camPositionSnap = (Vector2)transform.GetChild(0).position;
                lastFollowPlayer = true;
                cm.followPlayer = false;
            }
            else if(lastFollowPlayer)
            {
                lastPosition = cm.camPositionSnap;
                cm.camPositionSnap = Vector3.zero;
                lastFollowPlayer = false;
                cm.followPlayer = true;
            }
            else if((Vector2)cm.camPositionSnap == (Vector2)transform.GetChild(0).position)
            {
                cm.camPositionSnap = (Vector2)lastPosition;
                lastPosition = transform.GetChild(0).position;
                lastFollowPlayer = false;
                cm.followPlayer = false;
            } 
            else
            {
                lastPosition = cm.camPositionSnap;
                cm.camPositionSnap = (Vector2)transform.GetChild(0).position;
                lastFollowPlayer = false;
                cm.followPlayer = false;
            }
        }
    }
}
