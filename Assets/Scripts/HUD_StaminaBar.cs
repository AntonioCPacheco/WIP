using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD_StaminaBar : MonoBehaviour {

	public List<Sprite> staminaSprites;
	public Transform staminaBar;
	float stamina;

	// Use this for initialization
	void Start () {
		stamina = GameObject.Find ("Player Prefab").GetComponent<Player_Info> ().getStamina ();
		staminaBar = (Transform)Instantiate (staminaBar, transform.position, transform.rotation);
		if(GameObject.Find("StaminaBar")!=null)
			staminaBar.SetParent (GameObject.Find("StaminaBar").transform);
	}
	
	// Update is called once per frame
	void Update () {
		stamina = GameObject.Find ("Player Prefab").GetComponent<Player_Info> ().getStamina ();
		int i = Mathf.RoundToInt(stamina * 14);
		if (i < 0)
			i = 0;
		if (i > 13)
			i = 13;
		staminaBar.GetComponent<SpriteRenderer>().sprite = staminaSprites [i];
	}

	public float getStamina (){
		return stamina;
	}
}
