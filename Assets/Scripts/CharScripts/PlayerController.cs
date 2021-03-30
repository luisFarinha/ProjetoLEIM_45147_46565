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

    [Header("Jumping")]
    public float jumpForce = 12.5f;
    private float jumpTimer;
    private float jumpDelay = 0.2f;
    private bool doubleReady;

    [Header("Gliding")]
    public float glideFallingSpeed = -2.5f;
    private bool canGlide;

    [Header("Dashing")]
    public float dashForce = 11;
    public float dashTime = 0.2f;
    public float dashCooldown = 0.5f;
    private bool facingRight = true;
    private float dashTimer;
    private bool isDashing;
    private bool canDash;

    [Header("Falling")]
    public float maxFallingSpeed = -18.75f;

    [Header("Wall Jumping")]
    public float wallJumpForce = 8;
    public float wallJumpTime = 0.25f;
    public float landAnimTime = 0.2f;
    private bool isWallJumping;
    private bool isLanding;

    [Header("Wall Sliding")]
    public float wallSlideSpeed = -1.5f;
    public bool isWallSliding;

    [Header("Ground Collisions")]
    public bool onGround;
    private float gLength;
    private Vector3 leftGPoint;
    private Vector3 rightGPoint;

    [Header("Wall Collisions")]
    public bool onLeftWall;
    public bool onRightWall;
    private float wLength;

    [Header("Melee Attacks")]
    public float attackAnimTime = 0.33f;
    public bool isAttacking;
    public Transform attackPos;
    public Transform attackUpPos;
    public Transform attackDownPos;
    public float attackRange = 2f;
    public float attackCooldown = 0.3f;
    public int meleeAttackDmg = 25;
    private float attackTimer;

    [Header("Knocked Back")]
    public float knockbackForce = 15f;
    public bool isKnocked;
    public float knockbackTime = 0.4f;

    private string currentState = "player_idle";

    private string PLAYER_IDLE = "player_idle";
    private string PLAYER_DASH = "player_dash";
    private string PLAYER_RUN = "player_run";
    private string PLAYER_JUMP = "player_jump";
    private string PLAYER_DOUBLEJUMP = "player_doubleJump";
    private string PLAYER_STARTFALL = "player_startFall";
    private string PLAYER_FALL = "player_fall";
    private string PLAYER_LAND = "player_land";
    private string PLAYER_ATTACK = "player_attack";
    private string PLAYER_ATTACKUP = "player_attackUp";
    private string PLAYER_ATTACKDOWN = "player_attackDown";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        gLength = (sr.bounds.size.y / 2);
        wLength = (sr.bounds.size.x / 2) * 0.8f;

        im = new InputMaster();

        im.Player.Dash.started += _ => dash(); //pressed
        im.Player.Glide.started += _ => canGlide = true; //pressed
        im.Player.Glide.canceled += _ => canGlide = false; //released
        im.Player.Jump.started += _ => jump(); //pressed
        im.Player.Jump.canceled += _ => stopJump(); //released
        im.Player.Attack.started += _ => attack(PLAYER_ATTACK);
        im.Player.AttackUp.started += _ => attack(PLAYER_ATTACKUP);
        im.Player.AttackDown.started += _ => attack(PLAYER_ATTACKDOWN);
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
        leftGPoint = new Vector3(transform.position.x - sr.bounds.size.x * 0.3f, transform.position.y, transform.position.z);
        rightGPoint = new Vector3(transform.position.x + sr.bounds.size.x * 0.3f, transform.position.y, transform.position.z);
        if (Physics2D.Raycast(leftGPoint, Vector2.down, gLength, gLayer) || Physics2D.Raycast(rightGPoint, Vector2.down, gLength, gLayer))
        {
            onGround = true;
        }
        else { onGround = false; }


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


        x = im.Player.Walk.ReadValue<float>(); y = im.Player.Jump.ReadValue<float>();

        if (!isDashing && !isWallJumping && !isAttacking)
        {
            glide();
            limitFallSpeed();
            checkDirectionDigital();
        }
        checkGrounded();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            //delayed Jump
            if (jumpTimer > Time.time && onGround)
            {
                jump();
            }
            if (!isDashing && !isWallJumping && !isKnocked)
            {
                moveCharacter();
            }
            wallSlide();
        }

    }

    private void checkDirectionDigital()
    {
        if (x > 0)
        {
            x = 1;
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingRight = true;
        }
        else if (x < 0)
        {
            x = -1;
            transform.eulerAngles = new Vector3(0, 180, 0);
            facingRight = false;
        }
    }

    private void moveCharacter()
    {
        rb.velocity = new Vector2(x * walkSpeed, rb.velocity.y);


        if ((int)rb.velocity.y == 0 && (currentState == PLAYER_FALL || currentState == PLAYER_STARTFALL))
        {
            ChangeAnimationState(PLAYER_LAND);
            isLanding = true;
            Invoke("landComplete", landAnimTime);
        }
        else if ((int)rb.velocity.y == 0 && x != 0 && onGround)
        {
            ChangeAnimationState(PLAYER_RUN);
        }
        else if((int)rb.velocity.y == 0 && x == 0 && onGround)
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
        else if((int)rb.velocity.y > 0)
        {
            ChangeAnimationState(PLAYER_JUMP);
        }
        else if((int)rb.velocity.y < 0)
        {
            ChangeAnimationState(PLAYER_STARTFALL);
        }
    }

    private void landComplete()
    {
        isLanding = false;
    }

    private void dash()
    {
        if (canDash && Time.time > dashTimer)
        {
            StartCoroutine(dashing());
            canDash = false;
            dashTimer = Time.time + dashCooldown;
        }

    }
    private IEnumerator dashing()
    {
        isDashing = true;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        if (facingRight)
        {
            rb.AddForce(new Vector2(dashForce, 0), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(-dashForce, 0), ForceMode2D.Impulse);
        }

        ChangeAnimationState(PLAYER_DASH);

        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        isDashing = false;
    }

    private void wallSlide()
    {
        if (!onGround && rb.velocity.y < 0 && onLeftWall && x < 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(0, wallSlideSpeed);
        }
        else if (!onGround && rb.velocity.y < 0 && onRightWall && x > 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(0, wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void jump()
    {
        if (onGround && !isDashing) //jump
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpTimer = 0;
        }
        else if (!onGround && doubleReady && !isWallSliding && !isDashing) //double jump
        {
            rb.velocity = new Vector2(x * walkSpeed, 0);
            rb.AddForce(new Vector2(0, jumpForce * 3 / 4), ForceMode2D.Impulse);

            ChangeAnimationState(PLAYER_DOUBLEJUMP);
            isLanding = true;
            Invoke("landComplete", 0.25f);

            doubleReady = false;
        }
        else if (isWallSliding && !isDashing)
        {
            isWallSliding = false;
            StartCoroutine(wallJumping());
        }
        else if (!onGround && !isWallSliding)
        {
            jumpTimer = Time.time + jumpDelay;
        }
    }

    private IEnumerator wallJumping()
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

    private void glide()
    {
        if (rb.velocity.y < 0 && canGlide)
        {
            rb.velocity = new Vector2(x * walkSpeed, glideFallingSpeed);
        }
    }

    private void checkGrounded()
    {
        if (onGround)
        {
            doubleReady = true;
            canDash = true;
            canGlide = false;
        }
    }

    private void stopJump()
    {
        //jump height control
        if (rb.velocity.y > 0)
        {
            rb.AddForce(new Vector2(0, -rb.velocity.y), ForceMode2D.Impulse);
        }
    }

    private void attack(string attackType)
    {
        if (!isDashing && !isWallSliding && !isAttacking)
        {
            if (onGround && attackType != PLAYER_ATTACKDOWN)
            {
                ChangeAnimationState(attackType);
                isAttacking = true;
                checkForDmg(attackType);
                Invoke("attackComplete", attackAnimTime);
            }
            else if (!onGround)
            {
                ChangeAnimationState(attackType);
                isAttacking = true;
                checkForDmg(attackType);
                Invoke("attackComplete", attackAnimTime);
            }
        }
    }

        private void attackComplete()
    {
        isAttacking = false;
    }

    private void checkForDmg(string attackType)
    {
        Collider2D[] damagedEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, eLayer);
        string DmgDirection = "";
        if (Time.time > attackTimer)
        {
            if (attackType == PLAYER_ATTACK)
            {
                damagedEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, eLayer);
                if (!facingRight)
                {
                    DmgDirection = "left";
                    if(!onGround) knockback(damagedEnemies, knockbackForce, 0);
                }
                else if (facingRight)
                {
                    DmgDirection = "right";
                    if(!onGround) knockback(damagedEnemies, -knockbackForce, 0);
                }
            }
            else if (attackType == PLAYER_ATTACKUP)
            {
                damagedEnemies = Physics2D.OverlapCircleAll(attackUpPos.position, attackRange, eLayer);
                DmgDirection = "up";
                if (!onGround) knockback(damagedEnemies, 0, -knockbackForce);

            }
            else if (attackType == PLAYER_ATTACKDOWN)
            {
                damagedEnemies = Physics2D.OverlapCircleAll(attackDownPos.position, attackRange, eLayer);
                DmgDirection = "down";
                if (!onGround) knockback(damagedEnemies, 0, knockbackForce);

            }
            foreach (Collider2D enemy in damagedEnemies)
            {
                enemy.GetComponent<Enemy>().takeDamage(meleeAttackDmg, DmgDirection);
                Debug.Log("We hit" + enemy.name);
            }
            attackTimer = Time.time + attackCooldown;
        }
    }

    private void knockback(Collider2D[] damagedEnemies, float xKnock, float yKnock)
    {
        if (damagedEnemies.Length > 0)
        {
            isKnocked = true;
            rb.velocity = new Vector2(x * walkSpeed, 0);
            rb.AddForce(new Vector2(xKnock, yKnock), ForceMode2D.Impulse);
            Invoke("knockComplete", knockbackTime);
        }
    }

    private void knockComplete()
    {
        isKnocked = false;
    }

    private void limitFallSpeed()
    {
        if (rb.velocity.y <= maxFallingSpeed)
        {
            rb.velocity = new Vector2(x * walkSpeed, maxFallingSpeed);
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        
        if (!isAttacking && !isLanding)
        {
            anim.Play(newState);
            currentState = newState;
        }else if(!isAttacking && (newState == PLAYER_ATTACK || newState == PLAYER_ATTACKUP || newState == PLAYER_ATTACKDOWN)){
            anim.Play(newState);
            currentState = newState;
        }else if(newState == PLAYER_DASH)
        {
            anim.Play(newState);
            currentState = newState;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(leftGPoint, leftGPoint + Vector3.down * gLength);
        Gizmos.DrawLine(rightGPoint, rightGPoint + Vector3.down * gLength);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wLength);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawWireSphere(attackUpPos.position, attackRange);
        Gizmos.DrawWireSphere(attackDownPos.position, attackRange);
    }
}
