using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public LayerMask eLayer;
    public LayerMask gLayer;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private InputMaster im;

    [Header("Walking")]
    public float walkSpeed = 5f;
    private float x, y;
    private bool facingRight = true;

    [Header("Jumping")]
    public float jumpForce = 12.5f;
    private float jumpTimer;
    private float jumpDelay = 0.2f;
    private bool doubleReady;

    [Header("Gliding")]
    public float glideFallingSpeed = -2.5f;
    public bool isGliding;

    [Header("Dashing")]
    public float dashForce = 13;
    public float dashTime = 0.25f;
    public float dashCooldown = 0.5f;
    private float dashTimer;
    private bool isDashing;
    private bool canDash;

    [Header("Falling")]
    public float maxFallingSpeed = -18.75f;

    [Header("Wall Jumping")]
    public float wallJumpForce = 8;
    public float wallJumpTime = 0.2f;
    public float landAnimTime = 0.14f;
    public float doubleJumpAnimTime = 0.23f;
    private bool isWallJumping;
    private bool isLanding;

    [Header("Wall Sliding")]
    public float wallSlideSpeed = -1.5f;
    public bool isWallSliding;

    [Header("Ground Collisions")]
    public bool onGround;
    private float gLength;
    private Vector2 leftGPoint;
    private Vector2 rightGPoint;

    [Header("Wall Collisions")]
    public bool onLeftWall;
    public bool onRightWall;
    private float wLength;

    [Header("Melee Attacks")]
    public float attackAnimTime = 0.23f;
    public bool isAttacking;
    public Transform attackPos;
    public Transform attackUpPos;
    public Transform attackDownPos;
    public float attackRange = 0.75f;
    public float attackCooldown = 0.3f;
    public int meleeAttackDmg = 25;
    private float attackTimer;

    [Header("Knocked Back")]
    public float knockbackForce = 8f;
    public bool isKnocked;
    public float knockbackTime = 0.2f;

    [Header("Stun")]
    public float stunForce = 6f;
    public bool isStunned;
    public float stunTime = 0.3f;
    public float stunCooldown = 1f;
    private float stunTimer;

    [Header("Health and Damage")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Particle Effects")]
    public ParticleSystem dust;
    private bool readyForDust;

    private string currentState = Constants.PLAYER_IDLE;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        gLength = (sr.bounds.size.y / 2);
        wLength = (sr.bounds.size.x / 2) * 0.75f;

        im = new InputMaster();

        im.Player.Dash.started += _ => Dash(); //pressed
        im.Player.Glide.started += _ => isGliding = true; //pressed
        im.Player.Glide.canceled += _ => isGliding = false; //released
        im.Player.Jump.started += _ => Jump(); //pressed
        im.Player.Jump.canceled += _ => StopJump(); //released
        im.Player.Attack.started += _ => Attack(Constants.PLAYER_ATTACK);
        im.Player.AttackUp.started += _ => Attack(Constants.PLAYER_ATTACKUP);
        im.Player.AttackDown.started += _ => Attack(Constants.PLAYER_ATTACKDOWN);
    }

    private void OnEnable()
    {
        im.Enable();
    }

    private void OnDisable()
    {
        im.Disable();
    }

    private void Update()
    {
        //track movement values
        x = im.Player.Walk.ReadValue<float>(); y = im.Player.Jump.ReadValue<float>();

        if (!isDashing && !isWallJumping && !isStunned)
        {
            Glide();
            CheckDirectionDigital();
        }
        
        LimitFallSpeed();
        CheckGrounded();
        CheckWalled();
    }

    private void FixedUpdate()
    {
        CheckForParticles();

        if (!isDashing)
        {
            //delayed Jump
            if (jumpTimer > Time.time && onGround)
            {
                Jump();
            }
            if (!isWallJumping && !isKnocked && !isStunned)
            {
                MoveCharacter();
            }
            WallSlide();
        }
    }

    private void CheckGrounded()
    {
        leftGPoint = new Vector3(transform.position.x - sr.bounds.size.x * 0.3f, transform.position.y, transform.position.z);
        rightGPoint = new Vector3(transform.position.x + sr.bounds.size.x * 0.3f, transform.position.y, transform.position.z);
        if (Physics2D.Raycast(leftGPoint, Vector2.down, gLength, gLayer) || Physics2D.Raycast(rightGPoint, Vector2.down, gLength, gLayer))
        {
            onGround = true;
            doubleReady = true;
            canDash = true;
            isGliding = false;
        }
        else { onGround = false; }
    }

    private void CheckWalled()
    {
        if (Physics2D.Raycast(transform.position, Vector2.left, wLength, gLayer))
        {
            onLeftWall = true;
        }
        else { onLeftWall = false; }
        if (Physics2D.Raycast(transform.position, Vector2.right, wLength, gLayer))
        {
            onRightWall = true;
        }
        else { onRightWall = false; }
    }

    private void CheckDirectionDigital()
    {
        if (x > 0)
        {
            x = 1;
            if (!isAttacking)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            if (!facingRight && onGround) { dust.Play(); }
            facingRight = true;
        }
        else if (x < 0)
        {
            x = -1;
            if (!isAttacking)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            if (facingRight && onGround) { dust.Play(); }
            facingRight = false;
        }
    }

    private void CheckForParticles()
    {
        if (x == 0 && onGround) readyForDust = true;

        if (readyForDust && x != 0 && onGround)
        {
            dust.Play();
            readyForDust = false;
        }
    }

    private void MoveCharacter()
    {
        rb.velocity = new Vector2(x * walkSpeed, rb.velocity.y);
        if (!isAttacking && !isLanding && !isWallSliding && !isGliding && !isStunned)
        {
            if ((int)rb.velocity.y == 0 && (currentState == Constants.PLAYER_FALL || currentState == Constants.PLAYER_STARTFALL))
            {
                ChangeAnimationState(Constants.PLAYER_LAND);
                isLanding = true;
                dust.Play();
                StartCoroutine(ActionComplete("isLanding", landAnimTime));
            }
            else if ((int)rb.velocity.y == 0 && x != 0 && onGround)
            {
                ChangeAnimationState(Constants.PLAYER_RUN);
                dust.Play();
            }
            else if ((int)rb.velocity.y == 0 && x == 0 && onGround)
            {
                ChangeAnimationState(Constants.PLAYER_IDLE);
            }
            else if ((int)rb.velocity.y > 0)
            {
                ChangeAnimationState(Constants.PLAYER_JUMP);
            }
            else if ((int)rb.velocity.y < 0)
            {
                ChangeAnimationState(Constants.PLAYER_STARTFALL);
            }
        }
    }

    private void Dash()
    {
        if (canDash && Time.time > dashTimer && !isWallSliding && !isStunned && Unlockables.dashUnlocked)
        {
            StartCoroutine(Dashing());
            canDash = false;
            dashTimer = Time.time + dashCooldown;
        }

    }
    private IEnumerator Dashing()
    {
        isDashing = true;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        if (facingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            rb.AddForce(new Vector2(dashForce, 0), ForceMode2D.Impulse);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            rb.AddForce(new Vector2(-dashForce, 0), ForceMode2D.Impulse);
        }

        ChangeAnimationState(Constants.PLAYER_DASH);

        dust.Play();

        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        isDashing = false;
    }

    private void WallSlide()
    {
        if (!onGround && rb.velocity.y < 0 && onLeftWall && x < 0 && !isAttacking && !isGliding && !isStunned)
        {
            rb.velocity = new Vector2(0, wallSlideSpeed);
            ChangeAnimationState(Constants.PLAYER_WALLSLIDE);
            dust.Play();
            isWallSliding = true;
        }
        else if (!onGround && rb.velocity.y < 0 && onRightWall && x > 0 && !isAttacking && !isGliding && !isStunned)
        {
            rb.velocity = new Vector2(0, wallSlideSpeed);
            ChangeAnimationState(Constants.PLAYER_WALLSLIDE);
            dust.Play();
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void Jump()
    {
        if (onGround && !isDashing && !isStunned) //jump
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpTimer = 0;
            dust.Play();
        }
        else if (!onGround && doubleReady && !isLanding && !isWallSliding && !isGliding && !isStunned && Unlockables.doubleJumpUnlocked) //double jump
        {
            rb.velocity = new Vector2(x * walkSpeed, 0);
            rb.AddForce(new Vector2(0, jumpForce * 3 / 4), ForceMode2D.Impulse);
            if (!isAttacking)
            {
                ChangeAnimationState(Constants.PLAYER_DOUBLEJUMP);
                isLanding = true;
                StartCoroutine(ActionComplete("isLanding", doubleJumpAnimTime));
            }
            dust.Play();

            doubleReady = false;
        }
        else if (isWallSliding && !isDashing && !isStunned && Unlockables.wallJumpUnlocked)
        {
            isWallSliding = false;
            ChangeAnimationState(Constants.PLAYER_JUMP);
            dust.Play();
            StartCoroutine(WallJumping());
        }
        else if (!onGround && !isWallSliding)
        {
            jumpTimer = Time.time + jumpDelay;
        }
    }

    private IEnumerator WallJumping()
    {
        isWallJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        if (onLeftWall)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingRight = true;
            rb.AddForce(new Vector2(wallJumpForce, wallJumpForce), ForceMode2D.Impulse);
        }
        else if (onRightWall)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            facingRight = false;
            rb.AddForce(new Vector2(-wallJumpForce, wallJumpForce), ForceMode2D.Impulse);
        }
        doubleReady = true;
        canDash = true;
        yield return new WaitForSeconds(wallJumpTime);
        isWallJumping = false;
    }

    private void Glide()
    {
        if (rb.velocity.y < 0 && isGliding && !isWallSliding && !isAttacking && Unlockables.glideUnlocked)
        {
            rb.velocity = new Vector2(x * walkSpeed, glideFallingSpeed);
            ChangeAnimationState(Constants.PLAYER_GLIDE);
        }
    }

    private void StopJump()
    {
        //jump height control
        if (rb.velocity.y > 0)
        {
            rb.AddForce(new Vector2(0, -rb.velocity.y), ForceMode2D.Impulse);
        }
    }

    private void Attack(string attackType)
    {
        if (!isDashing && !isWallSliding && !isAttacking && !isGliding && !isStunned)
        {
            if (onGround && attackType != Constants.PLAYER_ATTACKDOWN)
            {
                ChangeAnimationState(attackType);
                isAttacking = true;
                CheckForDmgToGive(attackType);
                dust.Play();
                StartCoroutine(ActionComplete("isAttacking", attackAnimTime));
            }
            else if (!onGround)
            {
                ChangeAnimationState(attackType);
                isAttacking = true;
                CheckForDmgToGive(attackType);
                StartCoroutine(ActionComplete("isAttacking", attackAnimTime));
            }
        }
    }

    private void CheckForDmgToGive(string attackType)
    {
        Collider2D[] damagedEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, eLayer);
        string DmgDirection = "";
        if (Time.time > attackTimer)
        {
            if (attackType == Constants.PLAYER_ATTACK)
            {
                damagedEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, eLayer);
                if (!facingRight)
                {
                    DmgDirection = "left";
                    if (!onGround) { Knockback(damagedEnemies, knockbackForce, 0); }
                }
                else if (facingRight)
                {
                    DmgDirection = "right";
                    if (!onGround) { Knockback(damagedEnemies, -knockbackForce, 0); }
                }
            }
            else if (attackType == Constants.PLAYER_ATTACKUP)
            {
                damagedEnemies = Physics2D.OverlapCircleAll(attackUpPos.position, attackRange, eLayer);
                DmgDirection = "up";
                if (!onGround) { Knockback(damagedEnemies, 0, -knockbackForce); }
            }
            else if (attackType == Constants.PLAYER_ATTACKDOWN)
            {
                damagedEnemies = Physics2D.OverlapCircleAll(attackDownPos.position, attackRange, eLayer);
                DmgDirection = "down";
                if (!onGround) { Knockback(damagedEnemies, 0, knockbackForce); }
            }
            foreach (Collider2D enemy in damagedEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(meleeAttackDmg, DmgDirection);
            }
            attackTimer = Time.time + attackCooldown;
        }
    }

    private void Knockback(Collider2D[] damagedEnemies, float xKnock, float yKnock)
    {
        if (damagedEnemies.Length > 0)
        {
            isKnocked = true;
            rb.velocity = new Vector2(x * walkSpeed, 0);
            rb.AddForce(new Vector2(xKnock, yKnock), ForceMode2D.Impulse);
            StartCoroutine(ActionComplete("isKnocked", knockbackTime));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {   
            Vector2 dmgHere = gameObject.transform.position - collision.gameObject.transform.position;
            if(dmgHere.x < 1 && dmgHere.x > -1)
            {
                dmgHere.y = dmgHere.y > 0 ? 1 : -1;
            }
            else if(dmgHere.y < 1 && dmgHere.y > -1)
            {
                dmgHere.x = dmgHere.x > 0 ? 1 : -1;
            }
            else
            {
                dmgHere.x = dmgHere.x > 0 ? 1 : -1;
                dmgHere.y = dmgHere.y > 0 ? 1 : -1;
            }


            if (Time.time > stunTimer)
            {
                isStunned = true;
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(new Vector2(dmgHere.x * stunForce, dmgHere.y * stunForce * 1.5f), ForceMode2D.Impulse);
                StartCoroutine(ActionComplete("isStunned", stunTime));
                
                stunTimer = Time.time + stunCooldown;
            }
        }
    }

    private IEnumerator ActionComplete(string action, float time)
    {
        yield return new WaitForSeconds(time);
        switch (action)
        {
            case "isAttacking": isAttacking = false; break;
            case "isLanding": isLanding = false; break;
            case "isKnocked": isKnocked = false; break;
            case "isStunned": isStunned = false; break;
        }
    }

    private void LimitFallSpeed()
    {
        if (rb.velocity.y <= maxFallingSpeed)
        {
            rb.velocity = new Vector2(x * walkSpeed, maxFallingSpeed);
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        anim.Play(newState);
        currentState = newState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(leftGPoint, leftGPoint + Vector2.down * gLength);
        Gizmos.DrawLine(rightGPoint, rightGPoint + Vector2.down * gLength);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wLength);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawWireSphere(attackUpPos.position, attackRange);
        Gizmos.DrawWireSphere(attackDownPos.position, attackRange);
    }
}
