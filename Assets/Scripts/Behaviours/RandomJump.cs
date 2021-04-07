using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomJump : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Force Range")]
    public float xRange = 3f;
    public float yMinRange = 3f;
    public float yMaxRange = 6f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(Random.Range(-xRange, xRange), Random.Range(yMinRange, yMaxRange)), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
