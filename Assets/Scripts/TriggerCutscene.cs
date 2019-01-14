using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCutscene : MonoBehaviour {

    Lock_SuperClass linkedLock;
    public Vector2 positionToSnapTo;
    public float panDuration = 2;
    public float stayDuration = 2;

    private bool triggered = false;

	// Use this for initialization
	void Start () {
        linkedLock = GetComponent<Lock_SuperClass>();
        positionToSnapTo = linkedLock.door.position;
	}

    public void trigger()
    {
        if (!triggered)
        {
            triggered = true;
            Camera.main.GetComponent<Camera_Movement>().newCutscene(positionToSnapTo, panDuration, stayDuration, linkedLock.door.GetComponent<Player_EnterDoors>());
        }
    }
}
