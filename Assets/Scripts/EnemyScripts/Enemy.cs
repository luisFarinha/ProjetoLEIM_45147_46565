using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public LayerMask gLayer;
    public Slider slider;
    public Slider followSlider;
    public Collider2D playerCol;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public bool facingRight = false;

    [Header("Dmg Taken")]
    public float knockbackForce = 5f;
    public float stunDuration = 0.5f;
    public bool isStunned;
    public bool isDead;

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
        
        setMaxHealth(maxHealth);
        gLength = (col.bounds.size.y / 2);
        wLength = (col.bounds.size.x / 1.5f);
    }

    void Update()
    {
        if (!isDead)
        {
            checkGrounded();
            checkWalled();
            checkDirection();
            move();
        }
    }

    private void checkGrounded()
    {
        leftGPoint = new Vector2(transform.position.x - col.bounds.size.x * 0.5f, transform.position.y + 0.2f);
        rightGPoint = new Vector2(transform.position.x + col.bounds.size.x * 0.5f, transform.position.y + 0.2f);
        gRayDiretion = (facingRight) ? Physics2D.Raycast(rightGPoint, Vector2.down, gLength, gLayer) : Physics2D.Raycast(leftGPoint, Vector2.down, gLength, gLayer);
        if (gRayDiretion)
        {
            onGround = true;
        }
        else { 
            onGround = false;
        }
    }

    private void checkWalled()
    {
        wallPoint = new Vector2(transform.position.x, transform.position.y + 0.5f);
        wRayDiretion = (facingRight) ? Physics2D.Raycast(wallPoint, Vector2.right, wLength, gLayer) : Physics2D.Raycast(wallPoint, Vector2.left, wLength, gLayer);

        if (wRayDiretion)
        {
            onWall = true;
        }
        else { onWall = false; }
    }

    private void move()
    {
        if (!isStunned)
        {
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

    public void setHealth(int damage)
    {
        currentHealth -= damage;
        slider.value = currentHealth;
    }

    public void setMaxHealth(int maxhealth)
    {
        currentHealth = maxhealth;
        slider.maxValue = maxhealth;
        slider.value = maxhealth;
        followSlider.maxValue = maxhealth;
        followSlider.value = maxhealth;
    }

    public void takeDamage(int damage, string DmgDirection)
    {
        slider.gameObject.SetActive(true);

        setHealth(damage);

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
        StartCoroutine(actionComplete("isStunned", stunDuration));

        if (currentHealth <= 0)
        {
            slider.gameObject.SetActive(false);
            //Destroy(gameObject);
            anim.Play("beetle_die");
            isDead = true;
            Physics2D.IgnoreCollision(col, playerCol);
        }
    }
    private IEnumerator actionComplete(string action, float time)
    {
        yield return new WaitForSeconds(time);
        switch (action)
        {
            case "isStunned": isStunned = false; break;
        }
    }

    private void checkDirection()
    {
        if (!onGround || onWall) {
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
