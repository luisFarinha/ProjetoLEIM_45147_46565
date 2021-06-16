using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : GroundedEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        playerCol = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();

        smallCoin = (GameObject)Resources.Load(Constants.SMALL_COIN_TEXT);
        mediumCoin = (GameObject)Resources.Load(Constants.MEDIUM_COIN_TEXT);
        largeCoin = (GameObject)Resources.Load(Constants.LARGE_COIN_TEXT);

        SetHealth(maxHealth);
        gLength = (col.bounds.size.y / 1.6f);
        wLength = (col.bounds.size.x / 1.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && !hasDied)
        {
            Die();
            hasDied = true;
        }
        else if (currentHealth > 0 && hasDied)
        {
            hasDied = false;
            Physics2D.IgnoreCollision(col, playerCol, false);
            slider.gameObject.SetActive(false);
        }
        if (!isDead)
        {
            CheckGrounded();
            CheckWall();
            CheckDirection();
            Move();
        }
    }
}
