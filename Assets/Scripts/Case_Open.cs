using UnityEngine;
using System.Collections;

public class Case_Open : MonoBehaviour {

	public Sprite openSprite;
	public Sprite closedSprite;

	Item_SuperClass item = null;
	Animator anim;
	SpriteRenderer spr;
	bool open=false;

	bool beenInitialized = false;

	int openedFrame = 0;

	// Use this for initialization
	void Start () {
		Debug.Log ("[CaseOpen]" + transform.gameObject.name + " case opened = " + open);
		spr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator>();

		anim.SetBool ("Open", open);

		//anim.SetBool ("Open", false); //not sure
	}
	
	// Update is called once per frame
	void Update () {
		if (openedFrame >= 30) {
			anim.enabled = false;
			spr.sprite = openSprite;
		}
		if (isOpen ()) {
			openedFrame++;
		}
	}

	bool isOpen(){
		return open;
	}

	public void Open(){
		if (!isOpen ()) {
			Debug.Log ("[CaseOpen] Supposedly opened the case");
			open = true;
			anim.SetBool ("Open", true);

			item.OnChestOpen ();
		}
	}


	public bool HasBeenInitialized(){
		return beenInitialized;
	}

	public void Initialized(){
		beenInitialized = true;
	}

	public void SetItem(Transform t){
		item = Instantiate (t).GetComponent<Item_SuperClass>();
		item.hideSelf ();
		item.transform.SetParent (transform);
		item.transform.position = transform.position;
	}
}
