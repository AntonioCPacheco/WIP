using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{

    public float slowAmount;
    public float lerpTime;
    int inside = 0;
    IEnumerator coroutine;

    GameObject introScreen;

    // Use this for initialization
    void Awake()
    {
        introScreen = GameObject.Find("IntroScreen");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player") )//|| coll.CompareTag("NPC"))
        {
            if (inside == 0)
            {
                if (coroutine != null) StopCoroutine(coroutine);

                NPC_Movement nm = FindObjectOfType<NPC_Movement>();
                nm.followPlayer = true;

                coroutine = changeTimeScale(true, Time.realtimeSinceStartup);
                StartCoroutine(coroutine);
            }
            inside++;

            if (coll.CompareTag("Player")) coll.GetComponent<Player_Movement>().followNPC = true;
            if(coll.GetComponent<NPC_Movement>() != null)
                coll.GetComponent<NPC_Movement>().followPlayer = false;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player") || coll.CompareTag("NPC"))
        {
            inside--;
            if (inside == 0)
            {
                if (coroutine != null) StopCoroutine(coroutine);
                coroutine = changeTimeScale(false, Time.realtimeSinceStartup);
                StartCoroutine(coroutine);
            }
        }
    }

    IEnumerator changeTimeScale(bool slow, float startTime)
    {
        float alpha = (Time.realtimeSinceStartup - startTime) / lerpTime;
        bool triggered = false;
        bool end = GetComponent<EndGame>() != null;
        if (end) Camera.main.GetComponent<Camera_Movement>().zoomIn(lerpTime);
        while (alpha < 1)
        {
            if (!triggered && alpha > 0.5f && end)
            {
                GameObject.Find("DeathScreen").GetComponent<Animator>().SetTrigger("GameOver");
                triggered = true;
                StartCoroutine(backToMenu());
            }
            if (slow)
                Time.timeScale = Mathf.Lerp(1, slowAmount, alpha);
            else
                Time.timeScale = Mathf.Lerp(slowAmount, 1, alpha);
            alpha = (Time.realtimeSinceStartup - startTime) / lerpTime;
            yield return null;
        }
    }

    IEnumerator backToMenu()
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - start < 6f)
        {
            yield return null;
        }

        FindObjectOfType<Game_PauseManager>().Quit();
        /*introScreen.SetActive(true);
        GameObject button = GameObject.Find("Play");
        FindObjectOfType<GUI_FirstSelected>().setFirstSelected(button);*/
    }

    public bool isActive()
    {
        return inside > 0;
    }
}
