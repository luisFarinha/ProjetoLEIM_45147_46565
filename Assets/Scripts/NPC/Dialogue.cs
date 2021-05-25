using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextArea(3, 10)]
    public string[] FirstSentences;

    [TextArea(3, 10)]
    public string[] SecondSentences;

    [TextArea(3, 10)]
    public string[] FirstMissionCompletedSentences;

    [TextArea(3, 10)]
    public string[] SecondMissionCompletedSentences;
}
