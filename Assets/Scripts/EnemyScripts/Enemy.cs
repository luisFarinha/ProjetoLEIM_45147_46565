using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class Enemy : MonoBehaviour
{

    private Rigidbody2D rb;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Dmg Taken")]
    public float knockbackForce = 5f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int damage, string DmgDirection)
    {
        currentHealth -= damage;

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

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy Died");
            //Destroy(gameObject);
        }
    }
}
