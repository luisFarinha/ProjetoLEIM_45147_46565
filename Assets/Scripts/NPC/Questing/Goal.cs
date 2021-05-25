using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public Quest quest { get; set; }
    public string description { get; set; }
    public bool completed { get; set; }
    public int currentAmount { get; set; }
    public int requiredAmount {get;set;}


    public virtual void Init() //initialization method
    {
        // default init stuff
    }

    public void Evaluate() //see if you reached the amount necessary to complete the quest
    {
        if(currentAmount >= requiredAmount)
        {
            Complete();
        }
    }

    public void Complete() //reached the necessary amount
    {
        completed = true;
    }
}
