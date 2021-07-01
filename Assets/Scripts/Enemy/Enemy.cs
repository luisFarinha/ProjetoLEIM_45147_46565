using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    public LayerMask gLayer;
    public Slider slider;
    public Slider followSlider;
    [HideInInspector] public Collider2D playerCol;
    [HideInInspector] public Rigidbody2D rb;
    public Animator anim;
    [HideInInspector] public Collider2D col;

    [Header("Health")]
    public int maxHealth;
    [HideInInspector] public int currentHealth;

    [Header("Movement")]
    public float moveSpeed;
    public float changeDirectionCooldown;
    [HideInInspector] public bool facingRight;
    [HideInInspector] public float changeDirectionTimer;

    [Header("Dmg Taken")]
    public float knockbackForce;
    public float stunDuration;
    [HideInInspector] public bool isStunned;

    [Header("Dmg Given")]
    public float stunForce;

    [Header("Death")]
    public int nSmallCoins;
    public int nMediumCoins;
    public int nLargeCoins;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool hasDied;
    [HideInInspector] public GameObject smallCoin;
    [HideInInspector] public GameObject mediumCoin;
    [HideInInspector] public GameObject largeCoin;

    public void TakeDamage(int damage, string DmgDirection)
    {
        if (!isDead)
        {
            slider.gameObject.SetActive(true);

            SetDamagedHealth(damage);
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
        if (!isDead)
        {
            SpawnCoins(smallCoin, nSmallCoins);
            SpawnCoins(mediumCoin, nMediumCoins);
            SpawnCoins(largeCoin, nLargeCoins);
            anim.Play(Constants.ENEMY_DEATH);
        }
        else if (isDead && !anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Constants.ENEMY_DEATH))
        {
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

    public void SetDamagedHealth(int damage)
    {
        currentHealth -= damage;
        slider.value = currentHealth;
    }

    public void SetHealth(int maxhealth)
    {
        if (maxhealth == maxHealth)
        {
            slider.gameObject.SetActive(false);
        }
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
}
