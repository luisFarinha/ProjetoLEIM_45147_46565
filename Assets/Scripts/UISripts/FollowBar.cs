using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowBar : MonoBehaviour
{
    public Slider healthBarSl;
    public float followCooldown = 0.02f;
    private Slider followBarSl;
    private float followTimer;

    // Start is called before the first frame update
    void Start()
    {
        followBarSl = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (followBarSl.value > healthBarSl.value && Time.time > followTimer)
        {
            followBarSl.value -= 1;
            followTimer = Time.time + followCooldown;
        }
    }
}
