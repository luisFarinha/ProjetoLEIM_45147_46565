using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> FirstTalkSentences;

    private Queue<string> SecondTalkSentences;

    private Queue<string> FirstTalkSentencesMissionCompleted;

    private Queue<string> SecondTalkSentencesMissionCompleted;

    public Text nameText;
    public Text dialogueText;
    public GameObject dialogueBox;

    public Animator animator;

    public NPC npc;

    //public bool activateSecondConversation;

    void Start()
    {
        FirstTalkSentences = new Queue<string>();
        SecondTalkSentences = new Queue<string>();
        FirstTalkSentencesMissionCompleted = new Queue<string>();
        SecondTalkSentencesMissionCompleted = new Queue<string>();
        GameObject g = GameObject.FindGameObjectWithTag("NPC");
        npc = g.GetComponent<NPC>();
    }

    public void StartDialogue(Dialogue dialogue, int interactionFase)
    {

        animator.SetBool("IsOpen", true);
        //dialogueBox.SetActive(true);
        nameText.text = dialogue.name;

        switch (interactionFase)
        {
            case 1:
                FirstTalkSentences.Clear();

                foreach (string sentence in dialogue.FirstSentences)
                {
                    FirstTalkSentences.Enqueue(sentence);
                }
                break;

            case 2:

                SecondTalkSentences.Clear();

                foreach (string sentence in dialogue.SecondSentences)
                {
                    SecondTalkSentences.Enqueue(sentence);
                }
                break;

            case 3:
                FirstTalkSentencesMissionCompleted.Clear();

                foreach (string sentence in dialogue.FirstMissionCompletedSentences)
                {
                    FirstTalkSentencesMissionCompleted.Enqueue(sentence);
                }
                break;

            case 4:
                SecondTalkSentencesMissionCompleted.Clear();

                foreach (string sentence in dialogue.SecondMissionCompletedSentences)
                {
                    SecondTalkSentencesMissionCompleted.Enqueue(sentence);
                }
                break;
        }
               

        DisplayNextSentence(interactionFase);
    }

    public void DisplayNextSentence(int interactionFase)
    {
        string sentence = null;

        switch (interactionFase)
        {
            case 1:
                if (FirstTalkSentences.Count == 0)
                {
                    EndDialogue();
                    npc.finishedFirstConversation = true;
                    return;
                }
                sentence = FirstTalkSentences.Dequeue();
                break;

            case 2:
                if (SecondTalkSentences.Count == 0)
                {
                    EndDialogue();
                    npc.hasStartedSecondConversation = false;
                    return;
                }
                sentence = SecondTalkSentences.Dequeue();
                break;

            case 3:
                if (FirstTalkSentencesMissionCompleted.Count == 0)
                {
                    EndDialogue();
                    npc.finishedFirstMissionConversation = true;
                    return;
                }
                sentence = FirstTalkSentencesMissionCompleted.Dequeue();
                break;

            case 4:
                if (SecondTalkSentencesMissionCompleted.Count == 0)
                {
                    EndDialogue();
                    npc.hasStartedSecondMissionConversation = false;
                    return;
                }
                sentence = SecondTalkSentencesMissionCompleted.Dequeue();
                break;
        }
        
        StopAllCoroutines(); //Stop doing when doing one already
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        //dialogueBox.SetActive(false);
        //activateSecondConversation = true;
        Debug.Log("End of conversation");
        
    }
}
