using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lock_LightSwitch : Lock_SuperClass {
    
    public bool isInLight = false;
    public List<Sprite> sprites;
    public float completed = 0.0f;
    
    protected override bool isBeingTriggered()
    {
        if (completed >= 1.0f)
        {
            transform.GetComponent<SpriteRenderer>().sprite = sprites[0];
            completed = 1.0f;
            return true;
        }
        if (isInLight)
            completed += 0.0025f;
        else
            completed = 0.0f;

        int i = Mathf.RoundToInt(completed * sprites.Count);
        if (i < 0)
            i = 0;
        if (i > (sprites.Count-1))
            i = (sprites.Count - 1);
        transform.GetComponent<SpriteRenderer>().sprite = sprites[i];

        return false;
    }

    public void InLight(bool l)
    {
        isInLight = l;
    }

    protected override void childUnTrigger()
    {
        return;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        print("Light Switch - Trigger Enter 2D Not Implemented");
    }
}