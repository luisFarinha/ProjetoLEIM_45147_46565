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

        SetMaxHealth(maxHealth);
        gLength = (col.bounds.size.y / 1.8f);
        wLength = (col.bounds.size.x / 1.8f);
    }

    void Update()
    {
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
        if (gRayDiretion)
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

    public void TakeDamage(int damage, string DmgDirection)
    {
        if (!isDead)
        {
            slider.gameObject.SetActive(true);

            SetHealth(damage);

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

        if (currentHealth <= 0)
        {
            slider.gameObject.SetActive(false);
            //Destroy(gameObject);
            anim.Play("beetle_die");
            isDead = true;
            Physics2D.IgnoreCollision(col, playerCol);
            rb.drag = 1;
        }
        else
        {
            anim.Play("beetle_takedmg");
        }
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
        if (!onGround || onWall)
        {
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
