using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingBossEnemy : FlyingEnemyAI
{

    GameObject[] tentacle;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        playerCol = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        tentacle = GameObject.FindGameObjectsWithTag("Tentacle");

        seeker = GetComponent<Seeker>();
        InvokeRepeating("CheckDist", 0f, 0.5f);

        smallCoin = (GameObject)Resources.Load(Constants.SMALL_COIN_TEXT);
        mediumCoin = (GameObject)Resources.Load(Constants.MEDIUM_COIN_TEXT);
        largeCoin = (GameObject)Resources.Load(Constants.LARGE_COIN_TEXT);

        SetHealth(maxHealth);

        timeBtwShots = startTimeBtwShots;
    }

    void FixedUpdate()
    {
        EnemyBehaviour();
        SpawnEnemies();

        if (currentHealth <= 0 && !hasDied)
        {
            Die();
            for(int i=0; i<tentacle.Length; i++)
            {
                tentacle[i].SetActive(false);
            }
            
            CancelInvokeUpdatePath();
            path = null;
            hasDied = true;
        }
        else if (currentHealth > 0 && hasDied)
        {
            hasDied = false;
            Physics2D.IgnoreCollision(col, playerCol, false);
            slider.gameObject.SetActive(false);
        }
    }

    void CheckDist()
    {
        float dist = Vector2.Distance(rb.position, target.position);

        if (!hasDied)
        {
            if (dist <= DetectionDist && dist > FireDist)
            {
                InvokeUpdatePath();
            }
            else if (dist <= FireDist)
            {
                rb.velocity = new Vector2(0, 0);
            }
            else
            {
                CancelInvokeUpdatePath();
                path = null;
            }
        } 
    }
}