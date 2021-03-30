using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 3f;

    void FixedUpdate()
    {
        doSlowmotion();
    }

    void doSlowmotion()
    {
        Time.timeScale = slowdownFactor;
    }

}
