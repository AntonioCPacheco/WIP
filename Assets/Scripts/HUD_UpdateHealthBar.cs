using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD_UpdateHealthBar : MonoBehaviour {
	
	public Transform HealthPrefab;
	public float distanceBetweenHearts = 20f;
	//Vector3 lastCameraPosition;
	int lastHealth;
	int maxHealth;
	List<int> winking;

	//int lastWinked = 0;
	public int timeBetweenWinks = 60;
	public int winkingDuration = 1;
	
	// Use this for initialization
	void Start () {
		//lastCameraPosition = GetComponentInParent<Transform> ().position;
		maxHealth = GameObject.Find ("Player Prefab").GetComponent<Player_Health> ().getMaxHealth ();

		winking = new List<int>();
		for (int i = 0; i < maxHealth; i++) {
			Object heartAux = Instantiate (HealthPrefab, new Vector2 (transform.position.x + distanceBetweenHearts * i, transform.position.y), transform.rotation);
			heartAux.name = "Heart " + (i + 1);
			GameObject.Find ("Heart " + (i + 1)).transform.SetParent (gameObject.transform);
            GameObject.Find("Heart " + (i + 1)).GetComponent<Animator>().SetBool("corrupted", false);
            winking.Add(0);
		}
		lastHealth = GameObject.Find("Player Prefab").GetComponent<Player_Health>().getHealth();

	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < maxHealth; i++){
			if(GameObject.Find ("Heart " + (i+1)).GetComponent<Animator> ().GetBool("corrupted") && Random.Range(0,101)>99){
				GameObject.Find ("Heart " + (i+1)).GetComponent<Animator> ().SetTrigger ("wink");
			}
		}
	}

	void FixedUpdate(){
		if (lastHealth != GameObject.Find ("Player Prefab").GetComponent<Player_Health>().getHealth()) {
			Draw(GameObject.Find ("Player Prefab").GetComponent<Player_Health>().getHealth());
			lastHealth = GameObject.Find ("Player Prefab").GetComponent<Player_Health> ().getHealth ();
		}
	}


	public void Draw(int health){
		if (lastHealth > health) {
			int toChange = lastHealth - health;
			int alreadyChanged = maxHealth - lastHealth;
			//DestroyHearts();
			for (int i = alreadyChanged; i < alreadyChanged + toChange; i++) {
				GameObject heartAux = GameObject.Find ("Heart " + (maxHealth - i));
				if (heartAux) {
                    Debug.Log("WHYYYY");
					heartAux.GetComponent<Animator> ().SetBool ("corrupted", true);
					heartAux.GetComponent<Animator> ().SetInteger ("randomHeart", RandomHeartAnimation ());
				}
			}
		} else if (lastHealth < health) {
			int toChange = health - lastHealth;
			int alreadyChanged = (maxHealth - lastHealth) - toChange;
			for (int i = alreadyChanged; i < alreadyChanged + toChange; i++) {
				GameObject heartAux = GameObject.Find ("Heart " + (maxHealth - i));
				if (heartAux) {
					heartAux.GetComponent<Animator> ().SetBool ("corrupted", false);
				}
			}
		}
	}

	/*
	public void DestroyHearts(){
		GameObject [] list = GameObject.FindGameObjectsWithTag("Heart");
		for (int i = 0; i < list.Length; i++) {
			Destroy(list[i]);
		}
	}*/

	public int RandomHeartAnimation(){
		return Random.Range(1, 4/*number of different heart animations + 1*/);
	}

	public int RandomHeart(){
		return Random.Range(1, maxHealth+1);
	}

	public void updatePosition(Vector3 newPosition){
		transform.Translate (newPosition);
	}
}
