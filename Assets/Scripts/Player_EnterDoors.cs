using UnityEngine;
using System.Collections;

public class Player_EnterDoors : MonoBehaviour {

	public Player_EnterDoors linkedDoor;
	Transform nextRoom;
	Transform currentRoom;

    public Player_EnterDoors npcDoor;
    public DialogueTrigger dialogueTrigger;
    public Lock_SuperClass[] locks;

    public bool isOpen = false;
	public bool isLadder = false;

    public bool sameRoomAsNPC = false;
    public bool isInCutscene = false;
    public bool saveGameOnEnter = false;

    public bool playSoundOnOpen = true;

    AudioSource dingSource;
    AudioSource slideSource;

    bool beenInitialized = false;
    EndGame end;
    bool dialogueTriggerDone = false;
    int lastNumTriggers = 0;

	// Use this for initialization
	void Start () {
        dingSource = this.GetComponents<AudioSource>()[0];
        slideSource = this.GetComponents<AudioSource>()[1];

        GetComponent<Animator>().SetBool("OpenDoor", isOpen);
        end = GetComponent<EndGame>();
        if (dialogueTrigger == null) dialogueTriggerDone = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!dialogueTriggerDone && !dialogueTrigger.gameObject.activeSelf)
        {
            dialogueTriggerDone = true;
            open();
        }
	}

	public bool HasBeenInitialized(){
		return beenInitialized;
	}

	public void Initialized(){
		beenInitialized = true;
	}

    public void changeRoom()
    {
        changeRoom(true);
    }

	public void changeRoom(bool isPlayer){
        if (isPlayer)
        {
            if (linkedDoor == null)
            {
                print("Can't go in this door");
                if (end != null) end.endGame();
                return;
            }
            Camera_Movement mainCamera = Camera.main.GetComponent<Camera_Movement>();
            bool NPCvisible = linkedDoor.GetComponent<Player_EnterDoors>().sameRoomAsNPC;

            mainCamera.EnterDoorCutscene();
            mainCamera.followNPC = NPCvisible;

            //GameObject.FindObjectOfType<NPC_Movement>().setVisibility(NPCvisible);

            linkedDoor.close();
            StartCoroutine(waitForFrame());

            if (npcDoor != null)
            {
                FindObjectOfType<NPC_Movement>().moveNPCto(npcDoor.transform.position);
            }

            if (end != null) end.endGame();
        }
        else
        {
            if (end != null) end.endGameNPC();
        }
        close();
    }

    IEnumerator waitForFrame()
    {
        SpriteRenderer sr = GameObject.Find("Player Prefab").GetComponent<SpriteRenderer>();
        sr.enabled = false;
        yield return new WaitForSeconds(1.8f);

        bool NPCvisible = linkedDoor.GetComponent<Player_EnterDoors>().sameRoomAsNPC;
        GameObject.FindObjectOfType<NPC_Movement>().setVisibility(NPCvisible);
        Vector3 v3 = linkedDoor.transform.TransformVector(linkedDoor.transform.position);
        GameObject.Find("Player Prefab").GetComponent<Player_Movement>().MovePlayerTo(v3);
        sr.enabled = true;
        if (saveGameOnEnter) SaveManager.saveGame();
    }

    public void close()
    {
        if (!isOpen) return;
        slideSource.Play();
        isOpen = false;
        GetComponent<Animator>().SetBool("OpenDoor", false);
    }

    public void open()
    {
        if (isOpen) return;
        if (playSoundOnOpen)
        {
            dingSource.Play();
        }
        slideSource.Play();
        isOpen = true;
        GetComponent<Animator>().SetBool("OpenDoor", true);
    }

    public void checkLocks()
    {
        bool auxOpen = true;
        int numLocksTriggered = 0;
        for( int i = 0; i < locks.Length; i++ )
        {
            if (!locks[i].triggered)
            {
                auxOpen = false;
            }
            else
            {
                numLocksTriggered++;
            }
        }
        if (locks.Length == 0) auxOpen = false;

        if (!isInCutscene && lastNumTriggers != numLocksTriggered)
        {
            updateIndicators(numLocksTriggered);
            lastNumTriggers = numLocksTriggered;
        }
        if (auxOpen && !isOpen) open();
        else if (!auxOpen && isOpen) close();
    }

    public void updateIndicators(int numLocks) //Call when a new lock triggers
    {
        this.transform.GetComponentInChildren<Door_DisplayLockStates>().updateIndicators(numLocks);
    }

    public void load()
    {
        if (!dialogueTriggerDone && dialogueTrigger.gameObject.activeSelf) close();
        else
        {
            lastNumTriggers = -1; //force to update indicators
            checkLocks();
        }
    }
}
