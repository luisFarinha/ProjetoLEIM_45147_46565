using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundedEnemy : Enemy
{
    [Header("Ground Collisions")]
    [HideInInspector] public bool onGround;
    [HideInInspector] public float gLength;
    [HideInInspector] public Vector2 leftGPoint;
    [HideInInspector] public Vector2 rightGPoint;
    [HideInInspector] public RaycastHit2D gRayDiretion;

    [Header("Wall Collisions")]
    [HideInInspector] public bool onWall;
    [HideInInspector] public float wLength;
    [HideInInspector] public Vector2 wallPoint;
    [HideInInspector] public RaycastHit2D wRayDiretion;

    public void CheckGrounded()
    {
        leftGPoint = new Vector2(transform.position.x - col.bounds.size.x * 0.5f, transform.position.y);
        rightGPoint = new Vector2(transform.position.x + col.bounds.size.x * 0.5f, transform.position.y);
        gRayDiretion = (facingRight) ? Physics2D.Raycast(rightGPoint, Vector2.down, gLength, gLayer) : Physics2D.Raycast(leftGPoint, Vector2.down, gLength, gLayer);
        if (gRayDiretion || GameObject.Find("Fly")) //alterar para um parent "FlyEnemies"
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }

    public void CheckWall()
    {
        wallPoint = new Vector2(transform.position.x, transform.position.y);
        wRayDiretion = (facingRight) ? Physics2D.Raycast(wallPoint, Vector2.right, wLength, gLayer) : Physics2D.Raycast(wallPoint, Vector2.left, wLength, gLayer);

        if (wRayDiretion)
        {
            onWall = true;
        }
        else { onWall = false; }
    }

    public void Move()
    {
        if (!isStunned)
        {
            if (!anim.name.Equals(Constants.ENEMY_WALK))
            {
                anim.Play(Constants.ENEMY_WALK);
            }
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

    public void CheckDirection()
    {
        if ((!onGround || onWall) && Time.time > changeDirectionTimer)
        {
            changeDirectionTimer = Time.time + changeDirectionCooldown;

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

    public void Charge()
    {
        if (Mathf.Abs(playerCol.transform.position.x - transform.position.x) + Mathf.Abs(playerCol.transform.position.y - transform.position.y) < 6)
        {
            moveSpeed = 5;
        }
        else
        {
            moveSpeed = 2;
        }
    }

    public void Chase()
    {
        if (Mathf.Abs(playerCol.transform.position.x - transform.position.x) + Mathf.Abs(playerCol.transform.position.y - transform.position.y) < 10)
        {
            if (playerCol.transform.position.x < transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                facingRight = true;
            }
        }
    }


    public void OnDrawGizmos()
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
