using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureClass : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Plants Gathering");
        questName = "Plants Gathering";
        description = "Find these plants";
        //itemReward
        //experienceReward

        Goals.Add(new CollectionGoal(this, "Tree2", "Find the sacred tree", false, 0, 1));

        Goals.ForEach(g => g.Init());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
