using UnityEngine;
using System.Collections;

public class LightWall_Behaviour : MonoBehaviour {
    
    bool v_isInLight = false;
    bool lastFrame = false;

    public bool inverted = false;
    public float delay = 1.5f;

    float elapsedTime = 0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (v_isInLight && elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
        } else if (!v_isInLight && elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
        }
        
        if (elapsedTime >= delay)
        {
            GetComponent<BoxCollider2D>().isTrigger = !inverted;
        }
        else if (elapsedTime <= 0)
        {
            elapsedTime = 0f;
            GetComponent<BoxCollider2D>().isTrigger = inverted;
        }

        GetComponent<SpriteRenderer>().color = Color.Lerp(inverted ? new Color(0,0,0,0) : Color.white, inverted ? Color.white : new Color(0, 0, 0, 0), elapsedTime / delay);
        /*if(v_isInLight != lastFrame)
        {
            lastFrame = v_isInLight;

            GetComponent<BoxCollider2D>().isTrigger = !v_isInLight;
            if (inverted)
                GetComponent<BoxCollider2D>().isTrigger = !GetComponent<BoxCollider2D>().isTrigger;
        }*/
    }

    public void InLight(bool l)
    {
        v_isInLight = l;
    }

    public bool IsInLight()
    {
        return v_isInLight;
    }
}
