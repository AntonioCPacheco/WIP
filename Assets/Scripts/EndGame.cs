using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    IEnumerator coroutine;

    public void endGame()
    {
        Application.Quit();
    }

    public void endGameNPC()
    {
        if(coroutine != null) StopCoroutine(coroutine);
        float startTime = Time.realtimeSinceStartup;
        coroutine = endGameCoroutine(startTime);
        StartCoroutine(coroutine);
    }

    IEnumerator endGameCoroutine(float startTime)
    {
        yield return new WaitForSeconds(2f);
        GameObject.Find("DeathScreen").GetComponent<Animator>().SetTrigger("GameOver");
        yield return new WaitForSeconds(6f);
        FindObjectOfType<Game_PauseManager>().Quit();
    }

}
