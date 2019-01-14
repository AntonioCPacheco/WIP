using UnityEngine;
using System.Collections;
using System;

public class Item_HealthPack : Item_SuperClass
{
	public Item_HealthPack ()
	{
		
	}

	public override void Behaviour (){
		GameObject.Find ("Player Prefab").GetComponent<Player_Health> ().GainHealth (1);
	}

	public override void OnPickUp (){
		hideSelf ();
		Behaviour ();
	}

}

