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

    [Header("Mission")]
    private bool hasCompletedMission;
    private bool finishedFirstConversation;
    private bool finishedFirstTalk;
    public bool hasStartedConversation;


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
        if (isNear && !hasStartedConversation) {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            hasStartedConversation = true;
        } 
        else if(isNear && hasStartedConversation)
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
            //hasStartedConversation = false;
        }
    }
}
