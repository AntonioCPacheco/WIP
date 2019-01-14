using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBounce : MonoBehaviour {

    Rigidbody2D rBody;
	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void addDirectionalForce(Vector2 direction, float force)
    {
        rBody.velocity = Vector2.zero;
        Vector2 f = new Vector2();
        f.x = direction.x * force * 0.7f;
        f.y = direction.y * force * (f.x == 0 ? 1.6f : 1f);
        rBody.AddForce(f, ForceMode2D.Impulse);
    }
}