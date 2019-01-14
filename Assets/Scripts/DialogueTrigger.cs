using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public bool onlyPlayerTrigger = true;
    public int numOfSingleLines = 0;

    public Dialogue dialogue;
    public float[] npcSteps;

    public MovingPlatform platform;

    bool triggeredPlayer = false;
    bool triggeredNPC = false;

    public NPC_Answer answer;

    //public float timeToWait = 0f;

    public void initialize(string dialogueLines)
    {
        string[] lines = dialogueLines.Split('-');
        dialogue = new Dialogue(lines);
        triggeredPlayer = false;
        triggeredNPC = false;
    }

    public void TriggerDialogue()
    {
        StartCoroutine(waitForTime(0));
    }

    IEnumerator waitForTime(float time)
    {        
        yield return new WaitForSeconds(time/2);
        if (platform != null)
        {
            platform.enable();
        }
        yield return new WaitForSeconds(time/2);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, npcSteps, numOfSingleLines);
        if(answer != null)
            FindObjectOfType<DialogueManager>().setAnswer(answer);
        if (GetComponent<RockPaperScissorsTrigger>() != null)
        {
            int triggerAfterXChoices = GetComponent<RockPaperScissorsTrigger>().triggerAfterXChoices;
            FindObjectOfType<DialogueManager>().SetRockPaperScissors(triggerAfterXChoices);
        }
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (onlyPlayerTrigger || triggeredNPC)
                TriggerDialogue();
            else
                triggeredPlayer = true;
        }
        else if (other.CompareTag("NPC"))
        {
            if (triggeredPlayer)
                TriggerDialogue();
            else
                triggeredNPC = true;
        }
    }
}
