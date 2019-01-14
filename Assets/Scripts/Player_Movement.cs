using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Movement : MonoBehaviour {
	
	public float maxSpeed = 5f;
	public float maxJumpingSpeed = 7f;

	float stamina = 1.0f;
	bool running = false;
    bool ableToRun = false;

    public bool onTopOfSomething = false;
    public Vector2 somethingsVelocity;

	bool facingRight = true;

	bool isWaiting = false;
	int framesToWait = 0;

	Animator anim;

	//Ground Check Variables
	bool grounded = false;
	float groundRadius = 0.1f;
	public Transform groundCheck;

    //WallJumping Variables
    bool CanWallJump = false;
	bool isWallJumping = false;
	bool alreadyWallJumped = false;
	bool huggingBackWall = false;
	bool huggingFrontWall = false;
	public Transform rightWallCheck;
	public Transform leftWallCheck;

    //Box variables
    bool hasBox = false;
    GameObject box = null;
    bool isPickingUp = false;

	//Jumped Twice boolean
	//bool jumpedTwice = false;

	//Masks
	public LayerMask whatIsGround;
	LayerMask whatIsWall;
    public LayerMask whatIsBoxes;

    //Jump Forces
    public float jumpForce = 700f;
	float jumpSideForce = 1000f;

    float knockback = 300f;

	bool jumpNext = false;
	int jumpNextWindow = 10;
	int jumpNextDeltaFrames = 0;

    bool alreadyPressedJump = false;
    //Trampolin variables
    bool inTrampolin = false;
    bool inTrampolinUp = false;

    //Sound Objects
    AudioSource footstepsSource;
    AudioSource singleStompSource;

    Rigidbody2D rBody;
	Camera_Movement mainCamera;

    bool startedDialogue = false;
    public bool followNPC = false;

    void Start(){
		anim = GetComponent<Animator>();
		rBody = GetComponent<Rigidbody2D> ();
		mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera_Movement> ();

        singleStompSource = GetComponents<AudioSource>()[0];
        footstepsSource = GetComponents<AudioSource>()[1];
    }

	//Deals with collision checks(ground, walls, boxes) and physics
	void FixedUpdate(){
        if (!canMove())
        {
            anim.SetFloat("Speed", 0);
            return;
        }

        checkGround();
        checkJumpStatus();
        checkWalls();
        //also handles animator variables
        handleHorizontalInput();
	}

    void followNPCFunction()
    {
        Transform npc = FindObjectOfType<NPC_Movement>().transform;
        Vector2 npcV = npc.GetComponent<Rigidbody2D>().velocity;
        Vector2 pV = new Vector2(0, rBody.velocity.y);

        facingRight = FindObjectOfType<NPC_Movement>().facingRight;
        if (npcV.x > 0)
        {
            if (npc.transform.position.x > this.transform.position.x)
            {
                pV.x = ((npc.transform.position.x - this.transform.position.x > 20f) ? 1.2f : 1.1f) * npcV.x;
            }
            else
            {
                pV.x = 0.9f * npcV.x;
            }
        }
        rBody.velocity = pV;

        anim.SetFloat("Speed", 1);
        anim.SetFloat("vSpeed", rBody.velocity.y);
        anim.SetFloat("hSpeed", rBody.velocity.x - somethingsVelocity.x);
        anim.SetFloat("AbsHSpeed", Mathf.Abs(rBody.velocity.x - somethingsVelocity.x));
    }

	//Main function, deals with most of the player input. I should probably move the input elsewhere...
	void Update(){
		if (!canMove ()) {
			rBody.velocity = new Vector2(0, rBody.velocity.y);
            return;
		}
		if (isWaiting) {
			framesToWait--;
			if (framesToWait == 0) {
				isWaiting = false;
			} else {
				return;
			}
		}

        handleJumpInput1();

        if (!followNPC) handleRunInput();

        handleJumpInput2();

        if (!followNPC) { 
            handleVerticalInput();
            handlePickUp();
        }

        if(Input.GetKeyDown(KeyCode.H))
            halpImStuck();
    }
		
	//Wall jump if player is facing the wall
	void JumpBackward(){
		isWallJumping = true;
		rBody.velocity.Set (0,rBody.velocity.y);
		rBody.AddForce (new Vector2 ((facingRight ? -1 : 1) *jumpSideForce, jumpForce - (rBody.velocity.y * 35)));
		Flip ();
	}

	//Wall jump if player is with their back against the wall
	void JumpForward(){
		isWallJumping = true;
		rBody.velocity.Set (0,rBody.velocity.y);
		rBody.AddForce (new Vector2 ((facingRight ? 1 : -1) *jumpSideForce, jumpForce - (rBody.velocity.y * 35)));
	}

	//Main jump
	void Jump(){
        alreadyPressedJump = true;
        rBody.AddForce(Vector2.up * (jumpForce * (hasBox ? 0.65f : 1f)), ForceMode2D.Impulse);
	}

	//Flip player sprite to face the opposite direction
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;	
	}

	//Wait
	void waitXFrames(int xFrames){
		if (isWaiting){
			return;
		}
		isWaiting = true;
		framesToWait = xFrames;
	}

	//Teleport player to given position
	public void MovePlayerTo(Vector3 vec3){
		//mainCamera.followPlayer = false;
		GameObject.Find ("Main Camera").transform.Translate (vec3-transform.position);
		transform.Translate (vec3-transform.position);
		//mainCamera.followPlayer = true;
		//Wait ();
	}

	//Secondary jump. Used when the player takes damage and enters invicibility frames
	public void TakeDamage(){
		waitXFrames (30);
		isWallJumping = true; //Horrible hack need to change
		rBody.AddForce ((facingRight ? -1 : 1) * transform.right * knockback);
        rBody.AddForce(transform.up * knockback);
        rBody.velocity.Set ((anim.GetFloat("Speed") > 0 ? -1 : 1) * 10000, 0);
		Flip ();
		/* rBody.AddForce (new Vector2 (0, (jumpForce * 0.4f)));
		Wait ();
		rBody.AddForce (new Vector2 ((facingRight ? -1 : 1) * (jumpForce *), 0)); */
	}

	//Auxiliary function
	IEnumerator Wait() {
		yield return new WaitForEndOfFrame();
	}

	//Secondary jump, or rather bounce. Used when the player jumps on an enemy
	public void jumpOnEnemy() {
		rBody.AddForce (new Vector2 (0, (jumpForce * 0.8f) - (rBody.velocity.y * 45)));
	}

	//Function that returns true if the player is facing right (x>0 direction) and false if the player is facing left (x<0 direction)
	public bool isFacingRight(){
		return facingRight;
	}

	//Function that returns true if the player is allowed to move and false otherwise
	//Needs work though
	bool canMove(){
		return (!gameObject.GetComponent<Player_Health> ().isDead () && !startedDialogue);
	}

	public bool isGrounded(){
		return (grounded || inTrampolin);
	}

	public float getStamina(){
		return stamina;
	}

    void handleVerticalInput()
    {
        //Enter doors
        if (MyInput.GetEnterDoor())
        {
            Player_EnterDoors[] doors = FindObjectsOfType<Player_EnterDoors>();
            for (int i = 0; i < doors.Length; i++)
            {
                Transform child = doors[i].transform;
                if (GetComponent<BoxCollider2D>().OverlapPoint(new Vector2(child.Find("EntrancePoint").position.x, child.Find("EntrancePoint").position.y)) && doors[i].isOpen)
                {
                    doors[i].changeRoom();
                    break;
                }
            }
        }
        /*
        //Open chests
        if (Input.GetAxis("Vertical") < 0.1)
        {
            List<Transform> cases = roomManager.GetActiveCases();
            for (int i = 0; i < cases.Count; i++)
            {
                Transform child = cases[i];
                Debug.Log("[Player_Movement] Found a case in " + child.parent.parent.gameObject.name);
                if (GetComponent<BoxCollider2D>().OverlapPoint(new Vector2(child.Find("OpeningPoint").position.x, child.Find("OpeningPoint").position.y)))
                {
                    child.GetComponent<Case_Open>().Open();
                    Debug.Log("[Player_Movement] Tried to open a case in " + child.parent.parent.gameObject.name);
                    break;
                }
            }

            List<Transform> items = roomManager.GetActiveItems();
            for (int i = 0; i < items.Count; i++)
            {
                Transform child = items[i];
                Debug.Log("[Player_Movement] Found an item in " + child.parent.parent.gameObject.name);
                if (GetComponent<BoxCollider2D>().OverlapPoint(new Vector2(child.Find("PickUpPoint").position.x, child.Find("PickUpPoint").position.y)))
                {
                    child.GetComponent<Item_SuperClass>().OnPickUp();
                    Debug.Log("[Player_Movement] Tried to pick up an item in " + child.parent.parent.gameObject.name);
                    break;
                }
            }
        }*/
    }
    void handleJumpInput1()
    {
        if (!alreadyPressedJump && !inTrampolin && MyInput.GetJump())
        {
            if (grounded /*|| !jumpedTwice*/)
            {
                //if (!grounded) {
                //	jumpedTwice = true;
                //	Jump ();
                //} else {
                singleStompSource.pitch = 0.8f;
                singleStompSource.Play();
                Jump();
                VibrateController.vibrateControllerForXSeconds(0.05f, 0.2f, 0.2f);
                //anim.SetBool ("Ground", false);		
                //}
            }
            else if (CanWallJump && !alreadyWallJumped)
            {
                if (huggingFrontWall)
                {
                    JumpBackward();
                    alreadyWallJumped = true;
                }
                else if (huggingBackWall)
                {
                    JumpForward();
                    alreadyWallJumped = true;
                }
                else
                {
                    jumpNext = true;
                }
            }
        }
    }
    void handleJumpInput2()
    {
        if (!MyInput.GetJump() || inTrampolinUp)
        {
            if (alreadyPressedJump)
                alreadyPressedJump = false;
            if(rBody.velocity.y > 0f)
                rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y * 0.6f);
            //jumpFrames = maxJumpFrames + 1;
        }
    }
    void handleRunInput()
    {
        if (running)
        {
            stamina -= 0.0025f;
        }
        else if (stamina < 1.0f)
        {
            stamina += 0.0025f;
        }

        if (((Input.GetAxis("Run") > 0.1 && stamina > 0.1f) || ((!running && Input.GetAxis("Run") > 0.1) && stamina > 0.3f)) && ableToRun)
        {
            anim.SetBool("Running", true);
            running = true;
        }
        if (Input.GetAxis("Run") < 0.1 || stamina < 0.01)
        {
            anim.SetBool("Running", false);
            running = false;
        }
    }
    void handlePickUp()
    {
        if(MyInput.GetPickUp() && !isPickingUp)
        {
            isPickingUp = true;
            if (!hasBox)
            {
                Collider2D boxsCollider = Physics2D.OverlapCircle(rightWallCheck.position, groundRadius, whatIsBoxes);
                if (boxsCollider != null)
                {
                    setBox(boxsCollider.transform);
                }
            }
            else
            {
                RaycastHit2D hit2D = Physics2D.Raycast(this.transform.position, this.transform.localScale.x * Vector2.right, 10f, 1 << LayerMask.NameToLayer("Floor"));
                
                box.SetActive(true);
                box.transform.SetParent(null);
                box.transform.position = this.transform.position + new Vector3(facingRight ? 12.5f : -12f, 6.5f, -28);
                if (hit2D.collider!=null && hit2D.distance < 10f)
                {
                    print("worked");
                    box.transform.Translate((-1 * this.transform.localScale.x) * (this.transform.right * 10f));
                }
                box = null;
                hasBox = false;
                anim.SetTrigger("lostBox");
            }
        }
        else if(isPickingUp && !MyInput.GetPickUp())
        {
            isPickingUp = false;
        }
        anim.SetBool("hasBox", hasBox);
    }

    void checkGround()
    {
        bool prevGrounded = grounded;
        //Check if the GameObject is on the ground
        Vector3 auxGCPos1 = new Vector3(groundCheck.position.x + 4.0f, groundCheck.position.y, groundCheck.position.z);
        Vector3 auxGCPos2 = new Vector3(groundCheck.position.x - 4.0f, groundCheck.position.y, groundCheck.position.z);
        grounded = Physics2D.OverlapCircle(auxGCPos1, groundRadius, whatIsGround);
        grounded = grounded ? grounded : Physics2D.OverlapCircle(auxGCPos2, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded); //telling the Animator whether the GameObject is on the ground
        if (!prevGrounded && grounded)
        {
            singleStompSource.pitch = 0.58f;
            singleStompSource.Play();
        }
        else if (footstepsSource.isPlaying && !grounded) footstepsSource.Stop();

        if (grounded && !inTrampolinUp) inTrampolin = false; 
    }
    void checkJumpStatus()
    {
        //Setting jump booleans in the case the GameObject is on the ground
        if (grounded)
        {
            //jumpedTwice = false;
            alreadyWallJumped = false;
            isWallJumping = false;
            if (jumpNext)
            {
                //Jump();
            }
            //jumpFrames = 0;
        }
        else
        {
            if (jumpNext)
            {
                jumpNextDeltaFrames++;
            }
            if (jumpNextDeltaFrames > jumpNextWindow)
            {
                jumpNext = false;
                jumpNextDeltaFrames = 0;
            }
        }
    }
    void checkWalls()
    { 
        //Checking if the GameObject is hugging any walls
        if (CanWallJump)
        {
            huggingFrontWall = Physics2D.OverlapCircle(rightWallCheck.position, groundRadius, whatIsWall);
            if (!huggingFrontWall)
            {
                huggingBackWall = Physics2D.OverlapCircle(leftWallCheck.position, groundRadius, whatIsWall);
            }
        }
    }
    void handleHorizontalInput()
    {
        if (!isWallJumping)
        {
            float move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", Mathf.Abs(move));

            if (!isWaiting)
            {
                if (grounded)
                {
                    rBody.velocity = new Vector2(move * (maxSpeed * (running ? 2 : 1) * (hasBox ? .6f : 1)), rBody.velocity.y);

                    if (!footstepsSource.isPlaying && Mathf.Abs(rBody.velocity.x) > 1)
                    {
                        if (singleStompSource.isPlaying)
                            footstepsSource.PlayDelayed(0.3f);
                        else
                            footstepsSource.Play();
                    }
                    else if (Mathf.Abs(rBody.velocity.x) < 1) footstepsSource.Stop();

                    if (onTopOfSomething)
                    {
                        rBody.velocity += somethingsVelocity;
                        somethingsVelocity = Vector2.zero;
                    }
                }
                else
                {
                    if (inTrampolin)
                    {
                        rBody.velocity = (rBody.velocity) * 0.8f + new Vector2(move * (maxJumpingSpeed * (running ? 2 : 1) * (hasBox ? .6f : 1)), rBody.velocity.y) * 0.2f;
                    }
                    else
                    {
                        rBody.velocity = new Vector2(move * (maxJumpingSpeed * (running ? 2 : 1) * (hasBox ? .6f : 1)), rBody.velocity.y);
                    }
                }

            }
            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();
        }
        anim.SetFloat("vSpeed", rBody.velocity.y);
        anim.SetFloat("hSpeed", rBody.velocity.x - somethingsVelocity.x);
        anim.SetFloat("AbsHSpeed", Mathf.Abs(rBody.velocity.x - somethingsVelocity.x));
    }

    public void startDialogue()
    {
        Transform npc = GameObject.Find("NPC").transform;
        if ((npc.position.x > this.transform.position.x && this.transform.localScale.x == -1) || (npc.position.x < this.transform.position.x && this.transform.localScale.x == 1)) Flip();
        anim.SetTrigger("enteredDialogue");
        anim.SetBool("inDialogue", true);
        startedDialogue = true;
        if (footstepsSource.isPlaying) footstepsSource.Stop();
    }
    public void stopDialogue()
    {
        alreadyPressedJump = true;
        anim.SetBool("inDialogue", false);
        startedDialogue = false;
    }
    IEnumerator stopDialogueC()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("inDialogue", false);
        startedDialogue = false;
    }

    public void setBox(Transform box)
    {
        this.box = box.gameObject;
        this.box.gameObject.SetActive(false);
        this.box.transform.SetParent(this.transform);
        this.box.gameObject.SetActive(false);
        hasBox = true;
        anim.SetTrigger("pickBox");
    }
    public void dropBoxAnim()
    {
        anim.SetTrigger("lostBox");
        hasBox = false;
    }
    public bool doesPlayerHaveBox()
    {
        return hasBox;
    }

    void halpImStuck()
    {
        MovePlayerTo(this.transform.position + new Vector3(0, 20, 0));
    }

    public void addDirectionalForce(Vector2 direction, float force, float jumpTime)
    {
        force *= hasBox ? 0.75f : 1f;
        StartCoroutine(ForceRoutine(direction, force, jumpTime));
    }    
    IEnumerator ForceRoutine(Vector2 direction, float force, float jumpTime)
    {
        inTrampolinUp = true;
        inTrampolin = true;
        //rBody.velocity = new Vector2(rBody.velocity.x, 0);
        float timer = 0;
        
        while (timer < jumpTime)
        {
            //Calculate how far through the jump we are as a percentage
            //apply the full jump force on the first frame, then apply less force
            //each consecutive frame
            
            float proportionCompleted = timer / jumpTime;
            Vector2 thisFrameJumpVector = Vector2.Lerp(direction* force, Vector2.zero, proportionCompleted);
            thisFrameJumpVector.y *= 1.5f;
            rBody.AddForce(thisFrameJumpVector);
            timer += Time.deltaTime;
            yield return null;
        }

        inTrampolinUp = false;
    }
}