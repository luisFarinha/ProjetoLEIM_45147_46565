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
    public bool hasCompletedMission;
    public bool finishedFirstMissionConversation;
    public bool hasStartedFirstMissionConversation;
    public bool hasStartedSecondMissionConversation;
    private DialogueManager dialogueManager;

    private PickUp pickUp;

    private void Awake()
    {
        im = new InputMaster();

        im.Player.Interact.started += _ => TalkInteraction();

        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnEnable()
    {
        im.Enable();
    }

    private void OnDisable()
    {
        im.Disable();
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
                    dialogueManager.StartDialogue(dialogue, 1);
                    hasStartedFirstConversation = true;
                }
                else if (hasStartedFirstConversation && !finishedFirstConversation)
                {
                    dialogueManager.DisplayNextSentence(1);
                }
                else if (finishedFirstConversation && !hasStartedSecondConversation)
                {
                    dialogueManager.StartDialogue(dialogue, 2);
                    hasStartedSecondConversation = true;
                }
                else if (finishedFirstConversation && hasStartedSecondConversation)
                {
                    dialogueManager.DisplayNextSentence(2);
                }
            }        
            else
            {
                if (!hasStartedFirstMissionConversation && !finishedFirstMissionConversation)
                {
                    dialogueManager.StartDialogue(dialogue, 3);
                    hasStartedFirstMissionConversation = true;
                }
                else if (hasStartedFirstMissionConversation && !finishedFirstMissionConversation)
                {
                    dialogueManager.DisplayNextSentence(3);
                }
                else if (finishedFirstMissionConversation && !hasStartedSecondMissionConversation)
                {
                    dialogueManager.StartDialogue(dialogue, 4);
                    hasStartedSecondMissionConversation = true;
                }
                else if (finishedFirstMissionConversation && hasStartedSecondMissionConversation)
                {
                    dialogueManager.DisplayNextSentence(4);
                }

            }
        }

    }
}
