using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{   
    [Header("Components")]
    public LayerMask eLayer;
    private Rigidbody2D rb;
    //private Animator anim;
    private SpriteRenderer sr;
    private InputMaster im;

    [Header("Melee Attacks")]
    public Transform attackPos;
    public float attackRange = 2f;
    public float attackCooldown = 0.3f;
    public int meleeAttackDmg = 25;
    private float attackTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        im = new InputMaster();
        im.Player.Attack.performed += _ => Attack();
    }

    private void OnEnable()
    {
        im.Enable();
    }

    private void OnDisable()
    {
        im.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Attack()
    {
        if(Time.time > attackTimer)
        {
            Collider2D[] damagedEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, eLayer);
            foreach (Collider2D enemy in damagedEnemies)
            {
                enemy.GetComponent<Enemy>().takeDamage(meleeAttackDmg);
                Debug.Log("We hit" + enemy.name);
            }
            attackTimer = Time.time + attackCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
