using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //to specify what I am looking for in a quest

[System.Serializable]
public class Quest
{

    public string questName;

    [TextArea(3, 10)]
    public string description;

    public int moneyReward;

    public bool completed;

    public bool isActive;
    public QuestGoal goal;

    public void Complete()
    {
        isActive = false;
        completed = true;
        //Debug.Log(questName + " was completed!");
    }

    /*void GiveReward()
    {
        if(itemReward != null)
        {
            Debug.Log("TODO");
            //Give a Reward
            //InventoryConstroller.Instance.GiveItem(itemReward);
        }
    }*/
}
