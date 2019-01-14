using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressiveLight : MonoBehaviour {

    public GameObject player;
    Light settings;
    float maxX = 200;
    public float maxIntensity;
    public float minIntensity;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player Prefab");
        settings = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
		if(0 < player.transform.position.x && player.transform.position.x < maxX)
        {
            float t = player.transform.position.x / maxX;
            settings.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        }
        else if(player.transform.position.x <= 0)
        {
            settings.intensity = maxIntensity;
        }
        else
        {
            settings.intensity = minIntensity;
        }
	}
}
