using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class OrangeFly : FlyingEnemyAI
{


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        playerCol = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();

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
        Shoot();

        if (currentHealth <= 0 && !hasDied)
        {
            //Die();
            gameObject.SetActive(false);
            //puuuff

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

        if (dist <= DetectionDist && dist > FireDist)
        {
            InvokeUpdatePath();
            //CancelGoBackInvoke();
        }
        else if (dist <= FireDist /*&& dist > 2*/)
        {
            //Shoot();
            rb.velocity = new Vector2(0, 0);
            //CancelInvokeUpdatePath();
            //CancelGoBackInvoke();
            //path = null;
        }
        /*else if(dist <= 2)
        {
            //GoBackInvoke();
            

        }*/
        else
        {
            //CancelGoBackInvoke();
            CancelInvokeUpdatePath();
            path = null;
        }
    }


    
}
