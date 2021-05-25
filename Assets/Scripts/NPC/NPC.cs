using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Components")]
    public GameObject interaction;
    public GameObject turn;
    private InputMaster im;

    public Dialogue dialogue;
    public bool isNear;

    [Header("FirstInteraction")]
    public bool finishedFirstConversation;
    public bool hasStartedFirstConversation;
    public bool hasStartedSecondConversation;

    [Header("MissionInteraction")]
    private bool hasCompletedMission;
    public bool finishedFirstMissionConversation;
    public bool hasStartedFirstMissionConversation;
    public bool hasStartedSecondMissionConversation;


    private void Awake()
    {
        im = new InputMaster();

        im.Player.Interact.started += _ => TalkInteraction();
    }

    private void OnEnable()
    {
        im.Enable();
    }

    private void OnDisable()
    {
        im.Disable();
    }

    private void FixedUpdate()
    {
        //check ending of conversation
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {          
            interaction.SetActive(true);
            isNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interaction.SetActive(false);
            isNear = false;
        }
    }

    public void TalkInteraction()
    {
        if (isNear) {
            if (!hasCompletedMission)
            {
                if (!hasStartedFirstConversation && !finishedFirstConversation)
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue, 1);
                    hasStartedFirstConversation = true;
                }
                else if (hasStartedFirstConversation && !finishedFirstConversation)
                {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence(1);
                }
                else if (finishedFirstConversation && !hasStartedSecondConversation)
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue, 2);
                    hasStartedSecondConversation = true;
                }
                else if (finishedFirstConversation && hasStartedSecondConversation)
                {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence(2);
                }
            }        
            else
            {
                if (!hasStartedFirstMissionConversation && !finishedFirstMissionConversation)
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue, 3);
                    hasStartedFirstMissionConversation = true;
                }
                else if (hasStartedFirstMissionConversation && !finishedFirstMissionConversation)
                {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence(3);
                }
                else if (finishedFirstMissionConversation && !hasStartedSecondMissionConversation)
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue, 4);
                    hasStartedSecondMissionConversation = true;
                }
                else if (finishedFirstMissionConversation && hasStartedSecondMissionConversation)
                {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence(4);
                }

            }
        }

    }
}
