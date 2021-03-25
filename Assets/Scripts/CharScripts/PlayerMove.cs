using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Components")]
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
    public bool canWallJump;
    private bool isWallJumping;

    [Header("Wall Sliding")]
    public float wallSlideSpeed = -1.5f;

    [Header("Ground Collisions")]
    public bool onGround;
    private float gLength;
    private Vector3 leftGPoint;
    private Vector3 rightGPoint;

    [Header("Wall Collisions")]
    public bool onLeftWall;
    public bool onRightWall;
    private float wLength;

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
        leftGPoint = new Vector3(transform.position.x - sr.bounds.size.x * 0.2f, transform.position.y, transform.position.z);
        rightGPoint = new Vector3(transform.position.x + sr.bounds.size.x * 0.2f, transform.position.y, transform.position.z);
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

        if (!isDashing && !isWallJumping)
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
            if (!isWallJumping)
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

        if (x != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
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
            canWallJump = true;
            rb.velocity = new Vector2(0, wallSlideSpeed);
        }
        else if (!onGround && rb.velocity.y < 0 && onRightWall && x > 0)
        {
            canWallJump = true;
            rb.velocity = new Vector2(0, wallSlideSpeed);
        }
        else
        {
            canWallJump = false;
        }
    }

    private void jump()
    {
        if (onGround && !isDashing) //jump
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpTimer = 0;
        }
        else if (!onGround && doubleReady && !canWallJump && !isDashing) //double jump
        {
            rb.velocity = new Vector2(x * walkSpeed, 0);
            rb.AddForce(new Vector2(0, jumpForce * 3 / 4), ForceMode2D.Impulse);
            doubleReady = false;
        }
        else if (canWallJump && !isDashing)
        {
            canWallJump = false;
            StartCoroutine(wallJumping());
        }
        else if (!onGround && !canWallJump)
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

    private void limitFallSpeed()
    {
        if (rb.velocity.y <= maxFallingSpeed)
        {
            rb.velocity = new Vector2(x * walkSpeed, maxFallingSpeed);
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
    }
}
