using UnityEngine;
using System.Collections;

//Class used to aggregate all the information regarding the player instance
public class Player_Info : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isFacingRight(){
		return gameObject.GetComponent<Player_Movement> ().isFacingRight ();
	}

	public bool isGrounded(){
		return gameObject.GetComponent<Player_Movement> ().isGrounded ();
	}

	public int getHealth(){
		return gameObject.GetComponent<Player_Health> ().getHealth();
	}

	public int getMaxHealth(){
		return gameObject.GetComponent<Player_Health> ().getMaxHealth();
	}

	public bool isDead(){
		return gameObject.GetComponent<Player_Health> ().isDead();
	}

	public bool isInvincible(){
		return gameObject.GetComponent<Player_Health> ().isInvincible();
	}

	public float getStamina(){
		return gameObject.GetComponent<Player_Movement> ().getStamina();
	}









}
