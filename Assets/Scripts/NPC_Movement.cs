using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC_Movement : MonoBehaviour
{
    public float nextStep = 160;

    public float maxSpeed = 4f;
    public float jumpForce = 100f;
    bool facingPlayer;
    public bool facingRight = false;

    bool grounded = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    
    Animator anim;
    Rigidbody2D rbody;
    GameObject player;

    public bool followPlayer = false; //mimic player movement

    float xBeforeTarget;
    float target;
    bool reachedTarget = true;

    bool isJumping = false;

    bool inDialogue = false;
    bool changedNextStep = true;

    TriggerDialogueAfterTime tdat;
    bool nextChoiceFinal = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player Prefab");
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 A = groundCheck.position + new Vector3(6, -0.2f);
        Vector2 B = groundCheck.position + new Vector3(-6, -0.2f);
        grounded = Physics2D.OverlapCircle(A, 0.2f, whatIsGround);
        grounded = grounded || Physics2D.OverlapCircle(B, 0.2f, whatIsGround);
        
        checkReachedTarget(); //Update reachedTarget flag

        if (!inDialogue)
        {
            if (isReadyForNextStep())
            {
                xBeforeTarget = this.transform.position.x;
                target = getNextStep();
                FaceObjective();
            }
            if (reachedTarget && !changedNextStep)
            {
                rbody.velocity = new Vector2(0, rbody.velocity.y);
            }
            else if (followPlayer) //For final cutscene
            {
                print("Following");
                followPlayerFunction();
            }
            else
            {
                FaceObjective();
                facingRight = (this.transform.localScale.x > 0);
                rbody.velocity = new Vector2((facingRight ? 1 : -1) * 0.3f * maxSpeed, rbody.velocity.y);
            }
        }

        anim.SetBool("Grounded", grounded);

        anim.SetFloat("Speed", Mathf.Abs(rbody.velocity.x));
        anim.SetFloat("vSpeed", rbody.velocity.y);
    }

    float getDistance(float x1, float x2)
    {
        return Mathf.Abs((x1 + Mathf.Abs(x1) + Mathf.Abs(x2)) - (x2 + Mathf.Abs(x1) + Mathf.Abs(x2)));
    }

    public void Turn()
    {
        if (facingRight)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = Mathf.Abs(theScale.x);
            transform.localScale = theScale;
        }
        else
        {
            Vector3 theScale = transform.localScale;
            theScale.x = - Mathf.Abs(theScale.x);
            transform.localScale = theScale;
        }
    }

    void FaceObjective()
    {
        if ((target > this.transform.position.x && this.transform.localScale.x == -1) || (target < this.transform.position.x && this.transform.localScale.x == 1))
        {
            Vector3 theScale = transform.localScale;
            theScale.x = -theScale.x;
            transform.localScale = theScale;
        }
    }
    void facePlayer()
    {
        Transform npc = player.transform;
        if ((npc.position.x > this.transform.position.x && this.transform.localScale.x == -1) || (npc.position.x < this.transform.position.x && this.transform.localScale.x == 1))
        {
            Vector3 theScale = transform.localScale;
            theScale.x = - theScale.x;
            transform.localScale = theScale;
        }
    }

    void followPlayerFunction()
    {
        Vector2 pv = player.GetComponent<Rigidbody2D>().velocity;
        Vector2 npcV = new Vector2(0, rbody.velocity.y);
        FaceObjective();
        if (pv.x > 0)
        {
            float diff = this.transform.position.x - player.transform.position.x;
            if (diff > 0)
            {
                npcV.x = ((diff > 30f) ? 0.2f : 0.3f) * maxSpeed;
                FindObjectOfType<Player_Movement>().maxSpeed = 60f;
            }
            else
            {
                npcV.x = ((diff < -5f) ? 0.5f : 0.4f) * maxSpeed;
                FindObjectOfType<Player_Movement>().maxSpeed = 40f;
            }
        }
        else
        {
            npcV.x = 0.2f * maxSpeed;
        }
        rbody.velocity = npcV;
    }

    void checkReachedTarget()
    {
        if(xBeforeTarget > target)
        {
            reachedTarget = (this.transform.position.x <= target);
        }
        else
        {
            reachedTarget = (this.transform.position.x > target);
        }
    }
    bool isReadyForNextStep()
    {
        return (!inDialogue && changedNextStep);
    }
    public void setNextStep(float nextStep)
    {
        changedNextStep = true;
        this.nextStep = nextStep;
    }
    float getNextStep()
    {
        changedNextStep = false;
        return nextStep;
    }

    //USE ONLY IF YOU REALLY NEED TO
    //Look at setNextStep first
    public void changeTargetDirectly(float newTarget)
    {
        target = newTarget;
        setNextStep(newTarget);
    }

    public void startDialogue()
    {
        facePlayer();
        rbody.velocity = new Vector2(0, rbody.velocity.y);
        inDialogue = true;
    }

    public void stopDialogue()
    {
        if (nextChoiceFinal)
        {
            if (nextStep != 2851)
            {
                GameObject d12 = GameObject.Find("DialogueTrigger 12");
                if(d12 != null)
                    d12.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
            followPlayer = false;
            nextChoiceFinal = false;
            //FindObjectOfType<Player_Movement>().maxSpeed = 0;
        }
        if (tdat != null)
        {
            followPlayer = true;
            
            tdat.arm();
            tdat = null;
            nextChoiceFinal = true;
        }
        FaceObjective();
        inDialogue = false;
    }

    public void moveNPCto(Vector2 newPos)
    {
        this.transform.position = newPos;
        setNextStep(newPos.x + 2);
    }

    public bool tryEnterDoor(Player_EnterDoors sc)
    {
        if (reachedTarget)
        {
            enterDoor(sc);
            return true;
        }
        return false;
    }

    private void enterDoor(Player_EnterDoors sc)
    {
        sc.changeRoom(false);
        setVisibility(false);
    }

    public void setVisibility(bool visibility)
    {
        this.GetComponent<SpriteRenderer>().enabled = visibility;
    }

    public void jump()
    {
        isJumping = true;
        rbody.AddForce(new Vector2(0, (jumpForce - (rbody.velocity.y * 45))));
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Floor")) isJumping = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.GetComponent<ZoomOut>() != null)
        {
            tdat = FindObjectOfType<TriggerDialogueAfterTime>();
            maxSpeed += 5;
        }
    }

    public void addVerticalForce(float force)
    {
        isJumping = true;
        rbody.AddForce(new Vector2(0, (force - (rbody.velocity.y * 45))));
    }

    public void load(Vector3 pos, float goal)
    {
        moveNPCto(pos);

        reachedTarget = true;
        setNextStep(goal);
    }
}