using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public Slider slider;
    public Slider followSlider;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Dmg Taken")]
    public float knockbackForce = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
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

        if (currentHealth <= 0)
        {
            slider.gameObject.SetActive(false);
            Debug.Log("Enemy Died");
            //Destroy(gameObject);
            anim.Play("beetle_die");
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
}
