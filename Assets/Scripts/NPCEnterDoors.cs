using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEnterDoors : MonoBehaviour {
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            Player_EnterDoors ped = this.GetComponentInParent<Player_EnterDoors>();
            if(other.GetComponent<NPC_Movement>().tryEnterDoor(ped))
                this.gameObject.SetActive(false);
        }
    }
}
