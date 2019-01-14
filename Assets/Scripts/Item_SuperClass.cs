using UnityEngine;
using System.Collections;

public abstract class Item_SuperClass : MonoBehaviour {

	int tFrames = 0;
	protected GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player Prefab");
	}
	
	// Update is called once per frame
	void Update () {
		if (tFrames > 0) {
			transform.Translate (0, 0.3f, 0);
			tFrames -= 1;
		} else if (tFrames < 0) {
			tFrames = 0;
		}
	}

	public abstract void Behaviour ();

	public abstract void OnPickUp ();

	public void OnChestOpen () {
		showSelf ();
		tFrames = 100;
		StartCoroutine (wait(3));
	}

	public void hideSelf(){
		if(gameObject.activeSelf)
			gameObject.SetActive (false);
	}

	public void showSelf(){
		if(!gameObject.activeSelf)
			gameObject.SetActive (true);
	}

	protected void myStart(){
		player = GameObject.Find ("Player Prefab");
	}

	IEnumerator wait(int t){
		yield return new WaitForSeconds (t);
		OnPickUp ();
	}
}
