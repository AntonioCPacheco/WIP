using UnityEngine;
using System.Collections;

public abstract class Lock_SuperClass : MonoBehaviour {

    public bool triggered;
    public Transform door;

    protected AudioSource audioSource;

    protected bool untriggerable = false;

	// Use this for initialization
	void Start () {
        SuperStart();
    }

    protected void SuperStart()
    {
        triggered = false;
    }

    void Update()
    {
        if (isBeingTriggered())
        {
            trigger();
        }
       else
        {
            untrigger();
        }
    }

    protected void trigger()
    {
        if (!triggered)
        {
            triggerAux(true);
            if (audioSource != null) audioSource.Play();
        }
    }

    protected void untrigger()
    {
        if(triggered && !untriggerable)
            triggerAux(false);
    }

    private void triggerAux(bool t)
    {
        if(GetComponent<Animator>() != null)
            GetComponent<Animator>().SetBool("triggered", t);
        triggered = t;
        if(!t) childUnTrigger();
        door.GetComponent<Player_EnterDoors>().checkLocks();
    }

    protected abstract void OnTriggerEnter2D(Collider2D other);

    protected abstract bool isBeingTriggered();
    protected abstract void childUnTrigger();
}
