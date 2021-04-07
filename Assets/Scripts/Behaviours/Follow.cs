using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public float minModifier = 7;
    public float maxModifier = 11;

    private Vector2 vel = Vector2.zero;
    private bool isFollowing = false;
    
    void Start()
    {
        
    }

    private void StartFollowing()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            transform.position = Vector2.SmoothDamp(transform.position, target.position, ref vel, Time.deltaTime * Random.Range(minModifier, maxModifier));
        }
    }
}
