using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DialogueManager : MonoBehaviour {
    
    public bool assertive = false;
    [HideInInspector]
    public Text nameTag;

    private Queue<string> sentences;
    private int sentencesDisplayed = 0;
    private float[] npcSteps;

    int numOfSingleLines = 0;
    Transform childObject;
    Lock_SuperClass linkedLock;
    GUI_FirstSelected es;
    bool waitForContinue = false;

    int numberOfPicks = 0;
    int rockPaperScissors = -1;

    AudioSource[] sources;

    IEnumerator lastCoroutine;

    //NPC Answers variables
    NPC_Answer npcAnswer;
    bool lastPlayerAnswerWasRight;
    [HideInInspector]
    public int opposing = 0;
    float rightDoorX = 0;

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 30;
        //initializeTriggers();

        childObject = this.transform.Find("ChildObject");
        for(int i=0; i<childObject.childCount; i++)
        {
            childObject.GetChild(i).gameObject.SetActive(false);
        }
        sentences = new Queue<string>();
        es = GameObject.FindObjectOfType<GUI_FirstSelected>();
        sources = GetComponents<AudioSource>();
    }

    //Functions to set everything when Dialogue Trigger is triggered
    public void StartDialogue(Dialogue dialogue, float[] npcSteps, int numOfSingleLines, Lock_SuperClass linkedLock)
    {
        numberOfPicks = 0;
        sentencesDisplayed = 0;
        sentences.Clear();

        if (npcSteps != null)
            this.npcSteps = npcSteps;

        this.linkedLock = linkedLock;
        this.numOfSingleLines = numOfSingleLines;
        GameObject.Find("Player Prefab").GetComponent<Player_Movement>().startDialogue();
        GameObject.Find("NPC").GetComponent<NPC_Movement>().startDialogue();

        foreach (string sentence in dialogue.sentences){ sentences.Enqueue(sentence); }
        DisplayNextSentence();
    }

    public void setAnswer(NPC_Answer answer)
    {
        npcAnswer = answer;
    }

    public void StartDialogue(Dialogue dialogue, float[] npcSteps, int numOfSingleLines)
    {
        StartDialogue(dialogue, npcSteps, numOfSingleLines, null);
    }

    //Setting if rockpaper or not
    public void SetRockPaperScissors(int triggerAfterXChoices)
    {
        this.rockPaperScissors = triggerAfterXChoices;
    }

    public void SetRockPaperScissors()
    {
        this.rockPaperScissors = -1;
    }

    public void SetRockPaperScissorsResult(int choice)
    {
        string sentence = "";
        switch (choice)
        {
            case 1:
                sentence = "Rock vs. Scissors!";
                break;
            case 2:
                sentence = "Paper vs. Rock!";
                break;
            case 3:
                sentence = "Scissors vs. Paper!";
                break;
        }
        sentence += "\n You win. " + (assertive ? "I'm going left." : "I guess I'll go left.");

        this.rockPaperScissors = -2;
        Text toChange = getIndex(0);
        getIndex(5);
        HandleFirstSelected(5);

        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        lastCoroutine = TypeSingleSentence(sentence, toChange);
        StartCoroutine(lastCoroutine);
    }

    //Displaying Sentences
    public void DisplayNextSentence()
    {
        if(sentences.Count == 0 || waitForContinue)
        {
            return;
        }

        if (numOfSingleLines > 0)
        {
            numOfSingleLines--;
            DisplaySingleDialogue();
            return;
        }
        Text toChange = getIndex(sentencesDisplayed);
        string sentence = sentences.Dequeue();
        if(sentences.Count == 0) HandleFirstSelected(1);
        if (sentencesDisplayed++ == 0)
        {
            if (lastCoroutine != null)
                StopCoroutine(lastCoroutine);

            lastCoroutine = TypeSentence(sentence, toChange);
            StartCoroutine(lastCoroutine);
        }
        else
        {
            DisplaySentence(sentence, toChange);
        }
        
    }

    void DisplaySentence(string sentence, Text toChange)
    {
        toChange.text = sentence;
        DisplayNextSentence();
    }

    void DisplaySingleDialogue()
    {
        Text toChange = getIndex(0);
        getIndex(5);
        HandleFirstSelected(5);

        string sentence = sentences.Dequeue();
        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        lastCoroutine = TypeSingleSentence(sentence, toChange);
        StartCoroutine(lastCoroutine);
    }

    //Typing one letter at a time
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
        waitForContinue = true;
    }

    IEnumerator TypeSentence (string sentence, Text toChange)
    {
        int s = 0;
        toChange.text = "";
        foreach(char letter in sentence.ToCharArray())
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
        DisplayNextSentence();
    }

    //Playing sound for each letter
    static void playSound(DialogueManager dm, char c, int s)
    {
        if (c != ' ')
        {
            switch (s)
            {
                case 0:
                    dm.sources[1].Play();
                    break;
                case 1:
                    dm.sources[2].Play();
                    break;
                case 2:
                    dm.sources[3].Play();
                    break;
                case 3:
                    dm.sources[4].Play();
                    break;
            }
        }
    }

    void EndDialogue()
    {
        for (int i = 0; i < childObject.childCount; i++)
        {
            childObject.GetChild(i).gameObject.SetActive(false);
        }

        bool doesNPCAnswer = false;
        if (npcAnswer != null)
        {
            doesNPCAnswer = npcAnswer.triggerPlayerAnswer(lastPlayerAnswerWasRight);
            npcAnswer = null;
        }
        
        if(!doesNPCAnswer)
        {
            GameObject.Find("Player Prefab").GetComponent<Player_Movement>().stopDialogue();
            GameObject.Find("NPC").GetComponent<NPC_Movement>().stopDialogue();
        }
    }

    //Function called when a button is pressed
    public void pickChoice(int choice) //from 1 - 4
    {
        if (choice < 1 || choice > 4) throw new System.Exception("Choice out of range.");

        sources[0].Play();
        numberOfPicks++;
        if(rockPaperScissors > -1 && numberOfPicks == rockPaperScissors)
        {
            this.GetComponent<RockPaperScissors>().StartRPS(choice == 1);
            for (int i = 0; i < childObject.childCount; i++)
            {
                childObject.GetChild(i).gameObject.SetActive(false);
            }
            if(lastCoroutine!=null) StopCoroutine(lastCoroutine);
            return;
        }

        if (npcSteps == null || npcSteps.Length == 0)
        {
            EndDialogue();
            if (lastCoroutine != null) StopCoroutine(lastCoroutine);
            print("No steps for the NPC to take.");
            return;
        }
        //Log choice
        GameObject.Find("NPC").GetComponent<NPC_Movement>().setNextStep(npcSteps[choice - 1]);
        float rightX = float.MinValue;
        for (int i=0; i<npcSteps.Length; i++)
        {
            if (rightX < npcSteps[i]) rightX = npcSteps[i];
        }
        rightDoorX = rightX;
        lastPlayerAnswerWasRight = (npcSteps[choice - 1] < rightDoorX);
        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        EndDialogue();
    }

    public float getRightDoorX()
    {
        return rightDoorX;
    }

    public float getLeftDoorX()
    {
        for(int i=0; i < npcSteps.Length; i++)
        {
            if (npcSteps[i] < rightDoorX - 1) return npcSteps[i];
        }
        throw new Exception("Error in calculating left door.");
    }

    //Returns child of "ChildObject" correspondent to index
    private Text getIndex(int index)
    {
        childObject.GetChild(index).gameObject.SetActive(true);
        return childObject.GetChild(index).GetComponentInChildren<Text>();
    }

    public void continueButton(){
        numberOfPicks++;

        getIndex(5).transform.parent.gameObject.SetActive(false);
        getIndex(0).transform.parent.gameObject.SetActive(false);

        sources[0].Play();
        waitForContinue = false;
        if (lastCoroutine != null) StopCoroutine(lastCoroutine);

        if (rockPaperScissors != -2)
        {
            if (numOfSingleLines == 0) HandleFirstSelected(1);

            if (sentences.Count == 0) EndDialogue();
            else DisplayNextSentence();
        }
        else
        {
            rockPaperScissors = -1;
            GameObject.Find("NPC").GetComponent<NPC_Movement>().setNextStep(npcSteps[0]);
            EndDialogue();
        }
    }

    //Manage which option is selected
    void HandleFirstSelected(int index)
    {
        es.setFirstSelected(childObject.GetChild(index).gameObject);
    }

    //Initialize Dialogue triggers with text from txt file
    public void initializeTriggers()
    {
        string path = Application.dataPath + "/Dialogue/dialogue" + (assertive ? "Assertive" : "Submissive") + ".txt";
        print(path);
        if (!File.Exists(path)) return;

        StreamReader reader = new StreamReader(path);
        List<string> dialogueList = parseTriggerText(reader.ReadToEnd());
        reader.Close();

        for (int i = 1; i <= dialogueList.Count && GameObject.Find("DialogueTrigger " + i) != null; i++)
        {
            GameObject.Find("DialogueTrigger " + i).GetComponent<DialogueTrigger>().initialize(dialogueList[i-1]);
        }

        //CHANGE NAME TAG BASED ON assertive VARIABLE
        nameTag.text = (assertive ? "MAX:" : "LINUS:");
    }

    List<string> parseTriggerText(string file)
    {
        List<string> res = new List<string>();
        string[] triggers = file.Split('|');
        res.AddRange(triggers);
        return res;
    }

    public void toggleAssertive()
    {
        print("toggled ass");
        assertive = !assertive;
    }
}
