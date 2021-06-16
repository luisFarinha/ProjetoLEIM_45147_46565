using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemyAI : MonoBehaviour
{

    public Transform target;
    public Transform enemyGFX;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float DetectionDist;
    public float FireDist;

    public float currentSpeed;
    public float decreaseVelocity;

    Path path;

    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        


        InvokeRepeating("CheckDist", 0f, 0.5f);
        
    }

    void FixedUpdate()
    {

        //currentSpeed = rb.velocity.magnitude;

        if (path == null)
        {
            return;
        }

        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if(distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

        if(force.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        } else if(force.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void CheckDist()
    {
        float dist = Vector2.Distance(rb.position, target.position);

        if (dist <= DetectionDist /*&& dist > FireDist*/)
        {
            InvokeRepeating("UpdatePath", 0, .5f);
        }
        /*else if(dist <= FireDist)
        {
            SlowDown();
            CancelInvoke("UpdatePath");
            path = null;
        }*/
        else
        {
            CancelInvoke("UpdatePath");
            path = null;
        }
    }

    void SlowDown()
    {
        //Diminuir velocidade

        /*if (velocidade <= 0)
        {
            rb.velocity = new Vector2(0, 0);
            Debug.Log("pew pew");
            //Dispara
        }*/
    }


    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
}
