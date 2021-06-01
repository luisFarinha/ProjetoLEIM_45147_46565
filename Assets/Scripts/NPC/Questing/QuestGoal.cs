using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public int currentAmount;
    public int requiredAmount;

    public GoalType goalType;

    public bool IsReached() //see if you reached the amount necessary to complete the quest
    {
        return (currentAmount >= requiredAmount);
    }

    public void EnemyKilled()
    {
        if(goalType == GoalType.Kill)
        {
            currentAmount++;
        }
    }

    public void ItemCollected()
    {
        if(goalType == GoalType.Gathering)
        {
            currentAmount++; 
        }
    }

    

    public enum GoalType
    {
        Kill,
        Gathering,
        Puzzle
    }
}
