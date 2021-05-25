using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //to specify what I am looking for in a quest

public class Quest : MonoBehaviour
{
    public List<Goal> Goals { get; set; } = new List<Goal>();
    public string questName { get; set; }
    public string description { get; set; }
    public int experienceReward { get; set; }
    //public Item itemReward { get; set; }
    public bool completed { get; set; }

    public void CheckGoals()
    {
        completed = Goals.All(g => g.completed); //checks if this statement is true or false, if is one or more goals to achieve, returns false
        //if (completed) { GiveReward; }
    }

    /*void GiveReward()
    {
        if(itemReward != null)
        {
            //InventoryConstroller.Instance.GiveItem(itemReward);
        }
    }*/

    
}
