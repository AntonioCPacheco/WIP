using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock_BoxTrigger : Lock_SuperClass {

    public Sprite sprTriggered;
    public Sprite sprUnTriggered;

    public GameObject triggeredCollider;

    public bool isTriggered;
    
    Transform player;
    // Use this for initialization
    void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
        triggeredCollider.SetActive(false);
        if (player == null)
        {
            player = GameObject.Find("Player Prefab").transform;
        }
        untriggerable = false;
        SuperStart();
    }

    protected override bool isBeingTriggered()
    {
        return isTriggered;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            VibrateController.vibrateControllerForXSeconds(0.2f, 0.6f, 0.6f);
            other.gameObject.SetActive(false);
            childTrigger();
            if (this.GetComponent<TriggerCutscene>() != null)
                this.GetComponent<TriggerCutscene>().trigger();
        }
    }

    protected override void childUnTrigger()
    {
        triggeredCollider.SetActive(false);
        transform.GetComponent<SpriteRenderer>().sprite = sprUnTriggered;
        isTriggered = false;
        triggered = false;
        return;
    }

    private void childTrigger()
    {
        trigger();
        isTriggered = true;
        transform.GetComponent<SpriteRenderer>().sprite = sprTriggered;
        triggeredCollider.SetActive(true);
    }

    public void load(bool triggered)
    {
        if (!triggered) childUnTrigger();
        else childTrigger();
    }
}
