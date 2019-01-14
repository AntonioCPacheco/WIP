using UnityEngine;
using System.Collections;
using System;

public class Item_Torch : Item_SuperClass
{
	public Item_Torch ()
	{
		
	}
		
	public override void Behaviour (){
        if (GameObject.Find("Player Prefab") == null)
            Debug.Log("asdasda");
		GameObject.Find ("Player Prefab").GetComponent<Player_Lighting> ().increaseLight (1);
	}

	public override void OnPickUp (){
		hideSelf ();
		Behaviour ();
	}
		
}

