using UnityEngine;
using System.Collections;

public class Camera_Movement : MonoBehaviour
{

    public float lookRight = 3f;
    public float lookUp = 0f;
    public float vDampTime = 0.15f;
    public float hDampTime = 0.15f;

    public bool followPlayer = true;
    public bool followNPC = true;

    public bool followMouse = true;
    public float minMouseDistance = 30f;

    public Vector3 camPositionSnap = Vector3.zero;

    Vector3 velocity = Vector3.zero;

    Transform player;
    Vector3 mouse;
    public Vector3 topLeft = new Vector3(-3000f, 3000f, 0f);
    public Vector3 bottomRight = new Vector3(3000f, -3000f, 0f);
    Vector3 target;

    float zPos;

    bool isGrounded = true;

    Camera cam;

    bool inCutscene = false;

    //Zoom in Variables
    IEnumerator zoomInCoroutine;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player Prefab").transform;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        cam = GetComponent<Camera>();

        zPos = this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (inCutscene) return;

        isGrounded = player.gameObject.GetComponent<Player_Movement>().isGrounded();

        if (followPlayer)
        {
            if (followMouse)
                calculateMouse();
            else
                target = player.position + new Vector3(lookRight, lookUp, 0);
        }
        else
        {
            target = camPositionSnap;
        }

        Vector3 point = cam.WorldToViewportPoint(target);
        Vector3 delta = target - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        delta = new Vector3(delta.x, (isGrounded ? delta.y : 0f), delta.z);
        Vector3 destination = transform.position + delta;
        destination = correctBorders(destination);

        Vector3 vDamp = destination;
        vDamp.x = transform.position.x;
        vDamp = Vector3.SmoothDamp(transform.position, vDamp, ref velocity, vDampTime);
        Vector3 hDamp = vDamp;
        hDamp.x = destination.x;
        transform.position = Vector3.SmoothDamp(vDamp, hDamp, ref velocity, hDampTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
    }

    void calculateMouse()
    {
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 mouseRelative = mouse - player.position;

        if (mouseRelative.magnitude > minMouseDistance)
        {
            Vector2 compensation2D = new Vector2();
            if (Mathf.Abs(mouseRelative.x / mouseRelative.y) <= 0.5f)
                compensation2D = new Vector2(0, Mathf.Sign(mouseRelative.y));
            else if (Mathf.Abs(mouseRelative.x / mouseRelative.y) >= 0.5f && Mathf.Abs(mouseRelative.y / mouseRelative.x) >= 0.5f)
                compensation2D = new Vector2(Mathf.Sign(mouseRelative.x), Mathf.Sign(mouseRelative.y));
            else if (Mathf.Abs(mouseRelative.y / mouseRelative.x) <= 0.5f)
                compensation2D = new Vector2(Mathf.Sign(mouseRelative.x), 0);

            compensation2D = lookRight * compensation2D;
            float targetX = (followMouse ? compensation2D.x : 0);
            float targetY = (followMouse ? compensation2D.y : 0);
            target = new Vector3((player.position.x + targetX), (player.position.y + targetY), (player.position.z));
        }
    }

    Vector3 correctBorders(Vector3 destination)
    {
        //correcting X
        if (destination.x > (bottomRight.x - 200))
            destination = new Vector3(bottomRight.x - 200, destination.y, destination.z);
        else if (destination.x < (topLeft.x + 200))
            destination = new Vector3(topLeft.x + 200, destination.y, destination.z);

        //correcting Y
        if (destination.y < (bottomRight.y + 123.5f))
            destination = new Vector3(destination.x, bottomRight.y + 123.5f, destination.z);
        else if (destination.y > (topLeft.y - 123.5f))
            destination = new Vector3(destination.x, topLeft.y - 123.5f, destination.z);

        return destination;
    }

    public void newCutscene(Vector2 finalPosition, float pan_duration, float stay_duration, Player_EnterDoors linkedDoor)
    {
        VibrateController.vibrateControllerForXSeconds(pan_duration + stay_duration, 0, 0);
        Time.timeScale = 0;
        inCutscene = true;
        StartCoroutine(panCamera(this.transform.position, Time.realtimeSinceStartup, finalPosition, pan_duration, stay_duration, linkedDoor));
    }

    IEnumerator panCamera(Vector2 startPosition, float startTime, Vector2 finalPosition, float pan_duration, float stay_duration, Player_EnterDoors linkedDoor)
    {
        float alpha = (Time.realtimeSinceStartup - startTime) / pan_duration;
        float beta = (Time.realtimeSinceStartup - startTime - pan_duration) / stay_duration;
        while (alpha < 1)
        {
            alpha = (Time.realtimeSinceStartup - startTime) / pan_duration;
            this.transform.position = Vector2.Lerp(startPosition, finalPosition, alpha);
            yield return null;
        }
        bool updated = false;
        while (beta < 1)
        {
            if (!updated && beta > 0.2f)
            {
                linkedDoor.isInCutscene = false;
                linkedDoor.checkLocks();
                updated = true;
            }
            beta = (Time.realtimeSinceStartup - startTime - pan_duration) / stay_duration;
            yield return null;
        }
        Time.timeScale = 1;
        inCutscene = false;
    }

    public void zoomIn()
    {
        changeCameraSize(new Vector2(3, 30), 125);
    }

    public void zoomIn(float lerpTime)
    {
        changeCameraSize(lerpTime);
    }

    public void zoomOut(Vector2 focusPosition, float newSize)
    {
        changeCameraSize(focusPosition, newSize);
    }

    void changeCameraSize(Vector2 focusPosition, float newSize)
    {
        if (zoomInCoroutine != null) StopCoroutine(zoomInCoroutine);
        
        zoomInCoroutine = changeCameraSize(focusPosition, newSize, 2);
        StartCoroutine(zoomInCoroutine);
    }

    void changeCameraSize(float lerpTime)
    {
        if (zoomInCoroutine != null) StopCoroutine(zoomInCoroutine);
        zoomInCoroutine = changeCameraSize(Vector2.zero, 125, lerpTime*0.6f);
        StartCoroutine(zoomInCoroutine);
    }

    IEnumerator changeCameraSize(Vector2 focusPosition, float newSize, float lerpTime)
    {
        float original = this.GetComponent<Camera>().orthographicSize;
        Vector2 previousPosition = new Vector2(lookRight, lookUp);
        float startTime = Time.realtimeSinceStartup;
        float alpha = (Time.realtimeSinceStartup - startTime) / lerpTime;
        while (alpha < 1)
        {
            lookRight = Mathf.Lerp(previousPosition.x, focusPosition.x, alpha);
            lookUp = Mathf.Lerp(previousPosition.y, focusPosition.y, alpha);
            this.GetComponent<Camera>().orthographicSize = Mathf.Lerp(original, newSize, alpha);
            alpha = (Time.realtimeSinceStartup - startTime) / lerpTime;
            yield return null;
        }
    }

    public void EnterDoorCutscene()
    {
        inCutscene = true;
        GameObject.Find("DeathScreen").GetComponent<Animator>().SetTrigger("FadeOut");
        StartCoroutine(waitForFadeOut());
    }

    void continueEnterDoor()
    {
        inCutscene = false;
    }

    IEnumerator waitForFadeOut()
    {
        yield return new WaitForSeconds(1.8f);
        continueEnterDoor();
    }
}