using UnityEngine;
using System.Collections;

public class Light_Settings : MonoBehaviour {

    private const float RANGE_TO_RADIUS_RATIO = 5.17241379f;

    Light aLight;
    Light_Movement lMovement;

    public Texture normalCookie;
    public Texture largeCookie;

    CircleCollider2D collider2d;

    float increments = 6;

    const float maxZ = 190;
    const float minZ = 63;

    const float maxRange = 880;
    const float minRange = 300;

    // Use this for initialization
    void Start () {
        aLight = GetComponent<Light>();
        lMovement = GetComponent<Light_Movement>();
        collider2d = GetComponent<CircleCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            transform.localScale = new Vector3(2, 2, 1);
            aLight.spotAngle = aLight.spotAngle * 1.41f;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            transform.localScale = new Vector3(1, 1, 1);
            aLight.spotAngle = aLight.spotAngle / 1.41f;
        }
    }

    public void increaseLight(int ammount)
    {
        lMovement.mouseZ = lMovement.mouseZ - (ammount * ((maxZ - minZ) / increments));
        lMovement.mouseZ = lMovement.mouseZ < -maxZ ? -maxZ : lMovement.mouseZ;

        aLight.range = aLight.range + (ammount * ((maxRange - minRange) / increments));
        aLight.range = aLight.range > maxRange ? maxRange : aLight.range;

        collider2d.radius = aLight.range / RANGE_TO_RADIUS_RATIO;
        /*if (aLight.spotAngle + (ammount * 20) > 179)
        {
            aLight.spotAngle = 179;
            return;
        }
        aLight.spotAngle += ammount * 10;*/
    }

    public void decreaseLight(int ammount)
    {
        lMovement.mouseZ = lMovement.mouseZ + (ammount * ((maxZ - minZ) / increments));
        lMovement.mouseZ = lMovement.mouseZ > -minZ ? -minZ : lMovement.mouseZ;

        aLight.range = aLight.range + (ammount * ((maxRange - minRange) / increments));
        aLight.range = aLight.range > maxRange ? maxRange : aLight.range;

        collider2d.radius = aLight.range / RANGE_TO_RADIUS_RATIO;
        /*if (aLight.spotAngle - (ammount * 20) < 70)
        {
            aLight.spotAngle = 70;
            return;
        }
        aLight.spotAngle -= ammount * 10;*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.StartsWith("Enemy_Statue") && other.GetType() == typeof(BoxCollider2D))
        {
            other.GetComponent<Statue_Movement>().InLight(true);
        }
        else if (other.gameObject.name.StartsWith("Turtle"))
        {
            other.GetComponent<Turtle_Movement>().InLight(true);
        }
        else if (other.gameObject.name.StartsWith("Blob"))
        {
            other.GetComponent<Blob_Movement>().InLight(true);
        }
        else if (other.gameObject.name.StartsWith("Lock_LightSwitch"))
        {
            other.GetComponent<Lock_LightSwitch>().InLight(true);
        }
        else if (other.gameObject.name.StartsWith("LightWall"))
        {
            other.GetComponent<LightWall_Behaviour>().InLight(true);
        }
        else if (other.gameObject.name.StartsWith("LightPlatform"))
        {
            other.GetComponent<LightWall_Behaviour>().InLight(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.StartsWith("Enemy_Statue") && other.GetType() == typeof(BoxCollider2D))
        {
            other.GetComponent<Statue_Movement>().InLight(false);
        }
        else if (other.gameObject.name.StartsWith("Turtle"))
        {
            other.GetComponent<Turtle_Movement>().InLight(false);
        }
        else if (other.gameObject.name.StartsWith("Blob"))
        {
            other.GetComponent<Blob_Movement>().InLight(false);
        }
        else if (other.gameObject.name.StartsWith("Lock_LightSwitch"))
        {
            other.GetComponent<Lock_LightSwitch>().InLight(false);
        }
        else if (other.gameObject.name.StartsWith("LightWall"))
        {
            other.GetComponent<LightWall_Behaviour>().InLight(false);
        }
        else if (other.gameObject.name.StartsWith("LightPlatform"))
        {
            other.GetComponent<LightWall_Behaviour>().InLight(false);
        }
    }
}
