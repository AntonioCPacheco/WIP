using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Answer : MonoBehaviour {

    public Dialogue DominantAgreedAnswer;
    public Dialogue DominantOpposingAnswer;
    public Dialogue DominantFrustratedAnswer;

    public Dialogue SubmissiveAgreedAnswer;
    public Dialogue SubmissiveOpposingAnswer;

    public float[] npcSteps;

    public bool NPCWantsToGoRight;

    public bool triggerPlayerAnswer(bool PlayerWantsToGoRight)
    {
        DialogueManager dm = FindObjectOfType<DialogueManager>();
        bool opposing = (NPCWantsToGoRight && PlayerWantsToGoRight) || (!NPCWantsToGoRight && !PlayerWantsToGoRight);
        bool assNPC = dm.assertive;
        Dialogue dial;
        if (opposing)
        {
            if(dm.opposing > 0 && assNPC)
            {
                dial = DominantFrustratedAnswer;
                FindObjectOfType<NPC_Movement>().changeTargetDirectly(npcSteps[0]);
            }
            else
            {
                dial = assNPC ? DominantOpposingAnswer : SubmissiveOpposingAnswer;
            }
            dm.opposing++;
        }
        else
        {
            dial = assNPC ? DominantAgreedAnswer : SubmissiveAgreedAnswer;
        }
        if (dial.sentences.Length != 0)
        {
            dm.StartDialogue(dial, npcSteps, 1);
            return true;
        }
        return false;
    }
}
