using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour {

    AudioSource[] sources;
    Transform rockObject;
    public Dialogue dialogue;
    GUI_FirstSelected es;

    void Start()
    {
        rockObject = this.transform.Find("RockPaperScissors");
        for (int i = 0; i < rockObject.childCount; i++)
        {
            rockObject.GetChild(i).gameObject.SetActive(false);
        }
        es = GameObject.FindObjectOfType<GUI_FirstSelected>();
        sources = GetComponents<AudioSource>();
    }

    public void StartRPS(bool first)
    {
        string sentence = dialogue.sentences[(first ? 0 : 1)];
        for (int i = 0; i < rockObject.childCount; i++)
        {
            rockObject.GetChild(i).gameObject.SetActive(true);
        }
        HandleFirstSelected(1);
        StartCoroutine(TypeSingleSentence(sentence, rockObject.GetChild(0).GetComponentInChildren<Text>()));
    }

    IEnumerator TypeSingleSentence(string sentence, Text toChange)
    {
        int s = 0;
        toChange.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            playSound(this, letter, s);
            s = (s + 1) % 4;
            toChange.text += letter;
            yield return null;
        }
        sources[1].Stop();
        sources[2].Stop();
        sources[3].Stop();
        sources[4].Stop();
    }

    static void playSound(RockPaperScissors rps, char c, int s)
    {
        if (c != ' ')
        {
            switch (s)
            {
                case 0:
                    rps.sources[1].Play();
                    break;
                case 1:
                    rps.sources[2].Play();
                    break;
                case 2:
                    rps.sources[3].Play();
                    break;
                case 3:
                    rps.sources[4].Play();
                    break;
            }
        }
    }

    public void pickChoice(int choice) //1 - 3
    {
        for (int i = 0; i < rockObject.childCount; i++)
        {
            rockObject.GetChild(i).gameObject.SetActive(false);
        }
        GetComponent<DialogueManager>().SetRockPaperScissorsResult(choice);
    }

    void HandleFirstSelected(int index)
    {
        es.setFirstSelected(rockObject.GetChild(index).gameObject);
    }
}
