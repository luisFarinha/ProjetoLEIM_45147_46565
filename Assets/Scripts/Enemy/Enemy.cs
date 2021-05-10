using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public LayerMask gLayer;
    public Slider slider;
    public Slider followSlider;
    private Collider2D playerCol;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float changeDirectionCooldown = 0.5f;
    public bool facingRight = false;
    private float changeDirectionTimer;

    [Header("Dmg Taken")]
    public float knockbackForce = 8f;
    public float stunDuration = 0.15f;
    public bool isStunned;

    [Header("Death")]
    public bool isDead;
    public bool hasDied;
    public int nSmallCoins;
    public int nMediumCoins;
    public int nLargeCoins;
    private GameObject smallCoin;
    private GameObject mediumCoin;
    private GameObject largeCoin;

    [Header("Ground Collisions")]
    public bool onGround;
    private float gLength;
    private Vector2 leftGPoint;
    private Vector2 rightGPoint;
    private RaycastHit2D gRayDiretion;

    [Header("Wall Collisions")]
    public bool onWall;
    private float wLength;
    private Vector2 wallPoint;
    private RaycastHit2D wRayDiretion;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        playerCol = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();

        smallCoin = (GameObject)Resources.Load(Constants.SMALL_COIN_TEXT);
        mediumCoin = (GameObject)Resources.Load(Constants.MEDIUM_COIN_TEXT);
        largeCoin = (GameObject)Resources.Load(Constants.LARGE_COIN_TEXT);

        SetMaxHealth(maxHealth);
        gLength = (col.bounds.size.y / 1.6f);
        wLength = (col.bounds.size.x / 1.8f);
    }

    void Update()
    {
        if (currentHealth <= 0 && !hasDied) { 
            Die(); 
            hasDied = true; 
        }else if(currentHealth > 0)
        {
            hasDied = false;
        }
        if (!isDead)
        {
            CheckGrounded();
            CheckWall();
            CheckDirection();
            Move();
        }
    }

    private void CheckGrounded()
    {
        leftGPoint = new Vector2(transform.position.x - col.bounds.size.x * 0.5f, transform.position.y);
        rightGPoint = new Vector2(transform.position.x + col.bounds.size.x * 0.5f, transform.position.y);
        gRayDiretion = (facingRight) ? Physics2D.Raycast(rightGPoint, Vector2.down, gLength, gLayer) : Physics2D.Raycast(leftGPoint, Vector2.down, gLength, gLayer);
        if (gRayDiretion || GameObject.Find("Fly")) //alterar para um parent "FlyEnemies"
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }

    private void CheckWall()
    {
        wallPoint = new Vector2(transform.position.x, transform.position.y);
        wRayDiretion = (facingRight) ? Physics2D.Raycast(wallPoint, Vector2.right, wLength, gLayer) : Physics2D.Raycast(wallPoint, Vector2.left, wLength, gLayer);

        if (wRayDiretion)
        {
            onWall = true;
        }
        else { onWall = false; }
    }

    private void Move()
    {
        if (!isStunned)
        {
            if (!anim.name.Equals(Constants.ENEMY_WALK))
            {
                anim.Play(Constants.ENEMY_WALK);
            }
            if (facingRight)
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            else if (!facingRight)
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
        }
    }

    public void TakeDamage(int damage, string DmgDirection)
    {
        if (!isDead)
        {
            slider.gameObject.SetActive(true);

            SetHealth(damage);
            rb.velocity = Vector2.zero;
            if (DmgDirection == "right")
            {
                rb.AddForce(new Vector2(knockbackForce, 0), ForceMode2D.Impulse);
            }
            else if (DmgDirection == "left")
            {
                rb.AddForce(new Vector2(-knockbackForce, 0), ForceMode2D.Impulse);
            }
            else if (DmgDirection == "up")
            {
                rb.AddForce(new Vector2(0, knockbackForce), ForceMode2D.Impulse);
            }
            else if (DmgDirection == "down")
            {
                rb.AddForce(new Vector2(0, -knockbackForce), ForceMode2D.Impulse);
            }

            isStunned = true;
            StartCoroutine(ActionComplete("isStunned", stunDuration));
        }
        if (!isDead)
        {
            anim.Play(Constants.ENEMY_GET_HIT);
        }
    }

    public void Die()
    {
        slider.gameObject.SetActive(false);
        //Destroy(gameObject);
        if (!isDead){
            SpawnCoins(smallCoin, nSmallCoins);
            SpawnCoins(mediumCoin, nMediumCoins);
            SpawnCoins(largeCoin, nLargeCoins);
            anim.Play(Constants.ENEMY_DEATH);
        }else if(isDead && !anim.name.Equals(Constants.ENEMY_DEATH)) {
            anim.Play(Constants.ENEMY_DEATH); 
        }
        isDead = true;
        Physics2D.IgnoreCollision(col, playerCol);
        rb.drag = 5;
        rb.gravityScale = 3;
    }

    private void SpawnCoins(GameObject coin, int nCoins)
    {
        for (int i = 0; i < nCoins; i++)
        {
            GameObject c = (GameObject)GameObject.Instantiate(coin, transform.position, Quaternion.identity);
            c.GetComponent<Rigidbody2D>().AddForce(new Vector2(rb.velocity.x, 0), ForceMode2D.Impulse);
        }
    }

    public void SetHealth(int damage)
    {
        currentHealth -= damage;
        slider.value = currentHealth;
    }

    public void SetMaxHealth(int maxhealth)
    {
        currentHealth = maxhealth;
        slider.maxValue = maxhealth;
        slider.value = maxhealth;
        followSlider.maxValue = maxhealth;
        followSlider.value = maxhealth;
    }

    private IEnumerator ActionComplete(string action, float time)
    {
        yield return new WaitForSeconds(time);
        switch (action)
        {
            case "isStunned": isStunned = false; break;
        }
    }

    private void CheckDirection()
    {
        if ((!onGround || onWall) && Time.time > changeDirectionTimer)
        {
            changeDirectionTimer = Time.time + changeDirectionCooldown;
            
            if (facingRight)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingRight = false;
            }
            else if (!facingRight)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                facingRight = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (facingRight)
        {
            Gizmos.DrawLine(rightGPoint, rightGPoint + Vector2.down * gLength);
        }
        else
        {
            Gizmos.DrawLine(leftGPoint, leftGPoint + Vector2.down * gLength);
        }
        Gizmos.color = Color.blue;
        if (facingRight)
        {
            Gizmos.DrawLine(wallPoint, wallPoint + Vector2.right * wLength);
        }
        else
        {
            Gizmos.DrawLine(wallPoint, wallPoint + Vector2.left * wLength);
        }

    }




}
