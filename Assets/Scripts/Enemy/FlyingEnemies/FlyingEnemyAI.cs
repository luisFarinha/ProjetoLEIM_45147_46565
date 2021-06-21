using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemyAI : Enemy
{

    public Transform target;
    public Transform enemyGFX;
    public Transform firePoint;

    public float speed;
    private float nextWaypointDistance = 3f;
    public float DetectionDist;
    public float FireDist;


    [HideInInspector] public float timeBtwShots;
    public float startTimeBtwShots;

    //public float currentSpeed;
    //public float decreaseVelocity;

    [HideInInspector] public Path path;
    [HideInInspector] public int currentWayPoint = 0;
    [HideInInspector] public Seeker seeker;


    public void EnemyBehaviour(){
        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {

            return;
        }


        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

        if (force.x >= 0.01f)
        {
            enemyGFX.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (force.x <= -0.01f)
        {
            enemyGFX.transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }

    public void InvokeUpdatePath()
    {
        InvokeRepeating("UpdatePath", 0, .5f);
    }
    public void CancelInvokeUpdatePath()
    {
        CancelInvoke("UpdatePath");
    }

    /*public void GoBackInvoke()
    {
        InvokeRepeating("GoBack", 0, .1f);
    }

    public void CancelGoBackInvoke()
    {
        CancelInvoke("GoBack");
    }*/


    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    

    public void SlowDown()
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


    public void Shoot()
    {
        float dist = Vector2.Distance(rb.position, target.position);
        if (dist <= FireDist /*&& dist > 2*/)
        {
            if (gameObject.activeSelf)
            {
                if (timeBtwShots <= 0)
                {
                    Instantiate(Resources.Load("Fireball"), transform.Find("Fly2").transform.Find("Firepoint").position, Quaternion.identity);
                    timeBtwShots = startTimeBtwShots;
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }
            }
        }

    }

    public void SpawnEnemies() {
        float dist = Vector2.Distance(rb.position, target.position);
        if (dist <= FireDist)
        {
            if (gameObject.activeSelf)
            {
                if (timeBtwShots <= 0)
                {
                    Instantiate(Resources.Load("FlyingEnemy"), transform.Find("BossFly").transform.Find("Boss").transform.Find("Firepoint").position, Quaternion.identity);
                    Instantiate(Resources.Load("FlyingEnemy3"), transform.Find("BossFly").transform.Find("Boss").transform.Find("Firepoint").position, Quaternion.identity);
                    timeBtwShots = startTimeBtwShots;
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }
            }
        }
    }
    /*void GoBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, -10 * Time.deltaTime);
    }*/
}
