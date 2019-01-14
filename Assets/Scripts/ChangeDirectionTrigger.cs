using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirectionTrigger : MonoBehaviour {
    public float nextStep = 0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            other.GetComponent<NPC_Movement>().setNextStep(nextStep);
            this.gameObject.SetActive(false);
        }
    }
}
