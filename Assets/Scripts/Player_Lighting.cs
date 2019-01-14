using UnityEngine;
using System.Collections;

public class Player_Lighting : MonoBehaviour {

	Light_Settings spotlight;

	// Use this for initialization
	void Start () {
        if(GameObject.Find("Spotlight") != null)
		    spotlight = GameObject.Find("Spotlight").GetComponent<Light_Settings>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void increaseLight(int ammount)
    {
        if(spotlight != null)
            spotlight.increaseLight(ammount);
    }

    public void decreaseLight(int ammount)
    {
        if (spotlight != null)
            spotlight.decreaseLight(ammount);
    }
}
