using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal
{

    public string itemID { get; set; }

    public CollectionGoal(Quest quest, string itemID, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.quest = quest;
        this.itemID = itemID;
        this.description = description;
        this.completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = requiredAmount;

    }

    public override void Init()
    {
        base.Init();
        //UIEventHandler.OnItemAddedToInventory += ItemPickedUp;
    }

    /*void ItemPickedUp(Item item)
    {
        if(item.ObjectSlug == this.itemID)
        {

        }
    }*/
}
