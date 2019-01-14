using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint_Handler : MonoBehaviour {

    public Transform checkpoint;
    List<GameObject> checkpoints;
    public Sprite checkpointOn;

	// Use this for initialization
	void Start ()
    {
        checkpoints = new List<GameObject>();
	}

    public Vector3 getCheckpointPosition()
    {
        return checkpoint.position;
    }

    public void returnToLastCheckpoint()
    {
        StartCoroutine(muteAndWait());
        SaveManager.loadGame();
        /*if (checkpoint != null)
            transform.position = checkpoint.position;
        else
            GameObject.Find("SceneManager").GetComponent<Game_SceneManager>().NewGame();*/
    }


    IEnumerator muteAndWait()
    {
        AudioListener.volume = 0;
        yield return new WaitForSeconds(1.7f);
        float alpha = 0;
        float start = Time.realtimeSinceStartup;
        while(alpha < 1)
        {
            AudioListener.volume = Mathf.Lerp(0, 1, alpha);
            alpha = (Time.realtimeSinceStartup - start) / 3;
            yield return null;
        }

    }
}
