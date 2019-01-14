using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            other.GetComponent<NPC_Movement>().jump();
            this.gameObject.SetActive(false);
        }
    }
}
