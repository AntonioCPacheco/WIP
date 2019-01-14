using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin_Behavior : MonoBehaviour {

    public float force = 400f;
    public float jumpTime = 1f;

    AudioSource audioSource;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VibrateController.vibrateControllerForXSeconds(0.1f, 0.7f, 0.7f);
            print(audioSource.name);
            if (audioSource != null) audioSource.Play();
            other.GetComponent<Player_Movement>().addDirectionalForce(this.transform.up, force, jumpTime);
            GetComponent<Animator>().SetTrigger("Triggered");
        }

        else if (other.CompareTag("Box"))
        {
            print(audioSource.name);
            if (audioSource != null) audioSource.Play();
            other.GetComponent<BoxBounce>().addDirectionalForce(this.transform.up, force * 1.2f);
            GetComponent<Animator>().SetTrigger("Triggered");
        }
    }
}
