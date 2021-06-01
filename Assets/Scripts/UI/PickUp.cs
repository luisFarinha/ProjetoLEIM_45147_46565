using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;
    public Quest quest;
    public QuestGiver questGiver;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for(int i = 0; i < inventory.slots.Length; i++)
            {
                if(inventory.isFull[i] == false)
                {

                    //ifActive
                    if (quest.isActive){
                        for(int j=0; j<questGiver.pickUp.Count; j++)
                        {
                            if (itemButton.name == questGiver.pickUp[j].name)
                            {
                                questGiver.pickUp.Remove(questGiver.pickUp[j]);
                                quest.goal.ItemCollected();
                                if (quest.goal.IsReached())
                                {
                                    quest.Complete();
                                }
                            }

                        }
                        
                    }
                                    
                    
                    //ITEM CAN BE ADDED TO INVENTORY
                    inventory.isFull[i] = true;
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    
                    break;
                }
            }
        }
    }
}
