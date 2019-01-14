using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : MonoBehaviour {

    public Vector2 focusPosition;
    public float newSize = 185;
    Camera_Movement mainCam;
    int inside = 0;
    public bool soundBool = false;
    // Use this for initialization
    void Awake () {
        mainCam = Camera.main.GetComponent<Camera_Movement>();
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (inside == 0)
            {
                mainCam.zoomOut(focusPosition, newSize);
            }
            inside++;
        }

        if (!soundBool)
        {
            foreach (AudioSource s in Camera.main.GetComponents<AudioSource>())
            {
                if (!s.isPlaying && s.clip.name == "FinalSceneMusic")
                    s.Play();
                else
                {
                    StartCoroutine(fadeOut(s));
                }
            }
            soundBool = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            inside--;
            if (inside == 0)
            {
                mainCam.zoomIn();
            }
        }
    }

    IEnumerator fadeOut(AudioSource s)
    {
        float totalTime = 4.0f;
        float startTime = Time.realtimeSinceStartup;
        float initialVolume = s.volume;
        while (Time.realtimeSinceStartup - startTime < totalTime)
        {
            s.volume = (1 - (Time.realtimeSinceStartup - startTime / totalTime)) * initialVolume;
            yield return null;
        }
    }
}
