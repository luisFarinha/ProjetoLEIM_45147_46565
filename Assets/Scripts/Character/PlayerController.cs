using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public LayerMask eLayer;
    public LayerMask gLayer;
    public Slider slider;
    public Slider followSlider;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private CinemachineShake cineShake;

    private InputMaster im;
    private bool imIsEnabled = true;

    [Header("Walking")]
    public float walkSpeed = 5f;
    private float xMove;
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
    private float attackTime = 0.7f;
    private float lastHitTime;
    private float timeSinceLastHit;

    [Header("Knocked Back")]
    public float knockbackForce = 8f;
    public bool isKnocked;
    public float knockbackTime = 0.2f;

    [Header("Stun")]
    public float stunForce = 6f;
    public bool isStunned;
    public float stunTime = 0.3f;
    public float stunCooldown = 0.5f;
    private float stunTimer;

    [Header("Health and Damage")]
    public int maxHealth = 100;
    public int currentHealth;
    public int dmgTaken = 20;

    [Header("Particle Effects")]
    private ParticleSystem dust;
    private ParticleSystem landingDirt;
    private ParticleSystem dashingDust;
    private ParticleSystem dashingShine;
    private ParticleSystem wallSlidingDust;
    private ParticleSystem wallSlidingDirt;
    private ParticleSystem doubleJumpShine;
    private ParticleSystem Slash01;
    private ParticleSystem Slash01Shine;
    private ParticleSystem Slash01ShineUp;
    private ParticleSystem Slash01ShineDown;
    private ParticleSystem Slash02;
    private ParticleSystem Slash02Shine;
    private ParticleSystem Slash03;
    private ParticleSystem Slash03Shine;
    private ParticleSystem SlashUp;
    private ParticleSystem SlashDown;
    
    private int attackCounter = 0;

    [Header("SceneTransitions")]
    public bool isSpawning = true;
    public string startPos = "StartPos";

    private string currentState = Constants.PLAYER_IDLE;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cineShake = GameObject.FindWithTag("Cinemachine").GetComponent<CinemachineShake>();

        dust = GameObject.Find(Constants.DUST).GetComponent<ParticleSystem>();
        landingDirt = GameObject.Find(Constants.LANDING_DIRT).GetComponent<ParticleSystem>();
        dashingDust = GameObject.Find(Constants.DASHING_DUST).GetComponent<ParticleSystem>();
        dashingShine = GameObject.Find(Constants.DASHING_SHINE).GetComponent<ParticleSystem>();
        wallSlidingDust = GameObject.Find(Constants.WALL_SLIDING_DUST).GetComponent<ParticleSystem>();
        wallSlidingDirt = GameObject.Find(Constants.WALL_SLIDING_DIRT).GetComponent<ParticleSystem>();
        doubleJumpShine = GameObject.Find(Constants.DOUBLE_JUMP_SHINE).GetComponent<ParticleSystem>();
        Slash01 = GameObject.Find(Constants.SLASH01).GetComponent<ParticleSystem>();
        Slash01Shine = GameObject.Find(Constants.SLASH01SHINE).GetComponent<ParticleSystem>();
        Slash01ShineUp = GameObject.Find(Constants.SLASH01SHINEUP).GetComponent<ParticleSystem>();
        Slash01ShineDown = GameObject.Find(Constants.SLASH01SHINEDOWN).GetComponent<ParticleSystem>();
        SlashUp = GameObject.Find(Constants.SLASHUP).GetComponent<ParticleSystem>();
        SlashDown = GameObject.Find(Constants.SLASHDOWN).GetComponent<ParticleSystem>();
        Slash02 = GameObject.Find(Constants.SLASH02).GetComponent<ParticleSystem>();
        Slash02Shine = GameObject.Find(Constants.SLASH02SHINE).GetComponent<ParticleSystem>();
        Slash03 = GameObject.Find(Constants.SLASH03).GetComponent<ParticleSystem>();
        Slash03Shine = GameObject.Find(Constants.SLASH03SHINE).GetComponent<ParticleSystem>();

        gLength = (sr.bounds.size.y / 2) * 0.85f;
        wLength = (sr.bounds.size.x / 2) * 0.55f;

        im = new InputMaster();

        im.Player.Dash.started += _ => Dash(); //pressed
        im.Player.Glide.started += _ => isGliding = true; //pressed
        im.Player.Glide.canceled += _ => isGliding = false; //released
        im.Player.Jump.started += _ => Jump(); //pressed
        im.Player.Jump.canceled += _ => StopJump(); //released
        im.Player.Attack.started += _ => Attack(Constants.PLAYER_ATTACK);
        im.Player.AttackUp.started += _ => Attack(Constants.PLAYER_ATTACKUP);
        im.Player.AttackDown.started += _ => Attack(Constants.PLAYER_ATTACKDOWN);
        
        currentHealth = maxHealth;
    }

    public void Start()
    {
        isSpawning = false;
    }

    private void Update()
    {
        //track movement values
        xMove = im.Player.Walk.ReadValue<float>();
        timeSinceLastHit = Time.time - lastHitTime;

        if (!isDashing && !isWallJumping && !isStunned)
        {
            Glide();
            CheckDirectionDigital();
        }

        LimitFallSpeed();
        CheckGrounded();
        CheckWalled();

        RestrictControls();
    }

    private void FixedUpdate()
    {
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isSpawning)
        {
            transform.position = GameObject.FindWithTag(startPos).transform.position;
            StartCoroutine(StartMoving(0.7f));
        }
    }

    private IEnumerator StartMoving(float time)
    {
        //isStunned from OnCollisionEnter2D
        if (facingRight && onGround)
        {
            rb.velocity = new Vector2(walkSpeed, 0);
        }
        else if(onGround)
        {
            rb.velocity = new Vector2(-walkSpeed, 0);
        }
        yield return new WaitForSeconds(time);
        isStunned = false;    
    }

    private void OnEnable()
    {
        im.Enable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        im.Disable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void RestrictControls()
    {
        if (Time.timeScale == 0 && imIsEnabled)
        {
            OnDisable();
            imIsEnabled = false;
        }
        else if (Time.timeScale != 0 && !imIsEnabled)
        {
            OnEnable();
            imIsEnabled = true;
        }
    }

    private void CheckGrounded()
    {
        leftGPoint = new Vector3(transform.position.x - sr.bounds.size.x * 0.4f, transform.position.y, transform.position.z);
        rightGPoint = new Vector3(transform.position.x + sr.bounds.size.x * 0.4f, transform.position.y, transform.position.z);
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
        if (xMove > 0)
        {
            xMove = 1;
            if (!isAttacking)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            facingRight = true;
        }
        else if (xMove < 0)
        {
            xMove = -1;
            if (!isAttacking)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            facingRight = false;
        }
    }

    private void MoveCharacter()
    {
        rb.velocity = new Vector2(xMove * walkSpeed, rb.velocity.y);
        if (!isAttacking && !isLanding && !isWallSliding && !isGliding && !isStunned)
        {
            if ((int)rb.velocity.y == 0 && (currentState == Constants.PLAYER_FALL || currentState == Constants.PLAYER_STARTFALL))
            {
                ChangeAnimationState(Constants.PLAYER_LAND);
                isLanding = true;
                dust.Play();
                landingDirt.Play();
                StartCoroutine(ActionComplete(Constants.ActionType.LANDING, landAnimTime));
            }
            else if ((int)rb.velocity.y == 0 && xMove != 0 && onGround)
            {
                ChangeAnimationState(Constants.PLAYER_RUN);
                dust.Play();
            }
            else if ((int)rb.velocity.y == 0 && xMove == 0 && onGround)
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

        if (onGround)
        {
            dashingDust.Play();
            landingDirt.Play();
            dashingShine.Play();
        }
        else if (!onGround)
        {
            dashingShine.Play();
        }


        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        isDashing = false;
    }

    private void WallSlide()
    {
        if (!onGround && rb.velocity.y < 0 && onLeftWall && xMove < 0 && !isAttacking && !isGliding && !isStunned && Unlockables.wallJumpUnlocked)
        {
            rb.velocity = new Vector2(0, wallSlideSpeed);
            ChangeAnimationState(Constants.PLAYER_WALLSLIDE);
            wallSlidingDust.Play();
            wallSlidingDirt.Play();
            isWallSliding = true;
        }
        else if (!onGround && rb.velocity.y < 0 && onRightWall && xMove > 0 && !isAttacking && !isGliding && !isStunned && Unlockables.wallJumpUnlocked)
        {
            rb.velocity = new Vector2(0, wallSlideSpeed);
            ChangeAnimationState(Constants.PLAYER_WALLSLIDE);
            wallSlidingDust.Play();
            wallSlidingDirt.Play();
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
        else if (!onGround && doubleReady && !isLanding && !isWallSliding && !isGliding && !isStunned && !isDashing && Unlockables.doubleJumpUnlocked) //double jump
        {
            rb.velocity = new Vector2(xMove * walkSpeed, 0);
            rb.AddForce(new Vector2(0, jumpForce * 3 / 4), ForceMode2D.Impulse);
            if (!isAttacking)
            {
                ChangeAnimationState(Constants.PLAYER_DOUBLEJUMP);
                isLanding = true;
                StartCoroutine(ActionComplete(Constants.ActionType.LANDING, doubleJumpAnimTime));
            }
            doubleJumpShine.Play();

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
            rb.AddForce(new Vector2(wallJumpForce, wallJumpForce * 1.25f), ForceMode2D.Impulse);
        }
        else if (onRightWall)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            facingRight = false;
            rb.AddForce(new Vector2(-wallJumpForce, wallJumpForce * 1.25f), ForceMode2D.Impulse);
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
            rb.velocity = new Vector2(xMove * walkSpeed, glideFallingSpeed);
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
            if (attackType == Constants.PLAYER_ATTACK)
            {            
                if (attackCounter == 0)
                {
                    //attackType = Constants.PLAYER_ATTACK;
                    attackCounter = 1;
                    lastHitTime = Time.time;
                }
                else if (attackCounter == 1)
                {
                    if (timeSinceLastHit < attackTime)
                    {
                        attackType = Constants.PLAYER_ATTACK2;
                        attackCounter = 2;
                        lastHitTime = Time.time;
                    }
                    else
                    { 
                        attackCounter = 0;
                        lastHitTime = Time.time;
                    }
                }
                else if (attackCounter == 2)
                {
                    if (timeSinceLastHit < attackTime)
                    {
                        attackType = Constants.PLAYER_ATTACK3;
                        attackCounter = 0;
                        lastHitTime = Time.time;
                        StartCoroutine(ActionComplete(Constants.ActionType.ATTAKING, attackAnimTime));
                    } else
                    {
                        lastHitTime = Time.time;
                        attackCounter = 0;
                    }  
                }
            }
            if (onGround && attackType != Constants.PLAYER_ATTACKDOWN)
            {
                ChangeAnimationState(attackType);
                isAttacking = true;
                CheckForDmgToGive(attackType);
                StartCoroutine(ActionComplete(Constants.ActionType.ATTAKING, attackAnimTime));
            }
            else if (!onGround)
            {
                ChangeAnimationState(attackType);
                isAttacking = true;
                CheckForDmgToGive(attackType);
                StartCoroutine(ActionComplete(Constants.ActionType.ATTAKING, attackAnimTime));
            }
            AttackParticlesAnimations(attackType);
        }
    }


    
    private void AttackParticlesAnimations(string attackType)
    {
        if (!onGround && attackType == Constants.PLAYER_ATTACKDOWN)
        {
            SlashDown.Play();
            Slash01ShineDown.Play();
        }
        else if (attackType == Constants.PLAYER_ATTACKUP)
        {
            SlashUp.Play();
            Slash01ShineUp.Play();
        }
        else if (attackType == Constants.PLAYER_ATTACK)
        {
            Slash02.Play();
            Slash02Shine.Play();
        }
        else if(attackType == Constants.PLAYER_ATTACK2)
        {
            Slash01.Play();
            Slash01Shine.Play();
        }
        else if (attackType == Constants.PLAYER_ATTACK3)
        {
            Slash03.Play();
            Slash03Shine.Play();
            
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
                    Knockback(damagedEnemies, knockbackForce, 0);
                }
                else if (facingRight)
                {
                    DmgDirection = "right";
                    Knockback(damagedEnemies, -knockbackForce, 0);
                }
            }
            else if (attackType == Constants.PLAYER_ATTACKUP)
            {
                damagedEnemies = Physics2D.OverlapCircleAll(attackUpPos.position, attackRange, eLayer);
                DmgDirection = "up";
                Knockback(damagedEnemies, 0, -knockbackForce);
            }
            else if (attackType == Constants.PLAYER_ATTACKDOWN)
            {
                damagedEnemies = Physics2D.OverlapCircleAll(attackDownPos.position, attackRange, eLayer);
                DmgDirection = "down";
                Knockback(damagedEnemies, 0, knockbackForce);
            }
            foreach (Collider2D enemy in damagedEnemies)
            {
                if(!enemy.GetComponent<Enemy>().isDead)
                {
                    if (attackType == Constants.PLAYER_ATTACK3)
                    {
                        cineShake.ShakeCamera(3f, 0.15f);
                    }
                    else {
                        cineShake.ShakeCamera(1f, 0.15f);
                    }
                    enemy.GetComponent<Enemy>().TakeDamage(meleeAttackDmg, DmgDirection);
                }
            }
            attackTimer = Time.time + attackCooldown;
        }
    }

    private void Knockback(Collider2D[] damagedEnemies, float xKnock, float yKnock)
    {
        bool canKnockback = false;
        foreach (Collider2D enemy in damagedEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                canKnockback = true;
            }
        }
      
        if (damagedEnemies.Length > 0 && canKnockback && !onGround)
        {
            isKnocked = true;
            rb.velocity = new Vector2(xMove * walkSpeed, 0);
            rb.AddForce(new Vector2(xKnock, yKnock), ForceMode2D.Impulse);
            StartCoroutine(ActionComplete(Constants.ActionType.KNOCKED, knockbackTime));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && Time.time > stunTimer)
        {
            Vector2 dmgHere = gameObject.transform.position - collision.gameObject.transform.position;
            if (dmgHere.x < 1 && dmgHere.x > -1)
            {
                dmgHere.y = dmgHere.y > 0 ? 1 : -1;
            }
            else if (dmgHere.y < 1 && dmgHere.y > -1)
            {
                dmgHere.x = dmgHere.x > 0 ? 1 : -1;
            }
            else
            {
                dmgHere.x = dmgHere.x > 0 ? 1 : -1;
                dmgHere.y = dmgHere.y > 0 ? 1 : -1;
            }
                                  
            isStunned = true;
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(dmgHere.x * stunForce, dmgHere.y * stunForce * 1.5f), ForceMode2D.Impulse);
            SetHealth(dmgTaken);
            StartCoroutine(ActionComplete(Constants.ActionType.STUNNED, stunTime));

            stunTimer = Time.time + stunCooldown;
            StartCoroutine(FlashRed(stunTime / 4, 2));
            cineShake.ShakeCamera(3f, stunTime);
        }
    }

    private IEnumerator FlashRed(float time, int nTimes)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < nTimes; i++)
        {
            sr.color = new Color(0, 0, 0, 0);
            yield return new WaitForSeconds(time);
            sr.color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(time);
        }
    }

    public void SetHealth(int damage)
    {
        currentHealth -= damage;
        slider.value = currentHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Entrance"))
        {
            isStunned = true;
            rb.velocity = new Vector2(0, 0);
            anim.Play(Constants.PLAYER_RUN);
        }
    }

    private IEnumerator ActionComplete(Constants.ActionType action, float time)
    {
        yield return new WaitForSeconds(time);
        switch (action)
        {
            case Constants.ActionType.ATTAKING: isAttacking = false; break;
            case Constants.ActionType.LANDING: isLanding = false; break;
            case Constants.ActionType.KNOCKED: isKnocked = false; break;
            case Constants.ActionType.STUNNED: isStunned = false; break;
        }
    }

    private void LimitFallSpeed()
    {
        if (rb.velocity.y <= maxFallingSpeed)
        {
            rb.velocity = new Vector2(xMove * walkSpeed, maxFallingSpeed);
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
