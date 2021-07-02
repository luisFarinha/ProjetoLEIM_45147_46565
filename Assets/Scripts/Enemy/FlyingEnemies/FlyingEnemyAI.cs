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

    [HideInInspector] public Path path;
    [HideInInspector] public int currentWayPoint = 0;
    [HideInInspector] public Seeker seeker;

    public void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
    }


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


    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
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
        if (dist <= FireDist)
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
        Vector3 firepoint = transform.Find("BossFly").transform.Find("Boss").transform.Find("Firepoint").position;
        if (dist <= FireDist)
        {
            if (gameObject.activeSelf)
            {
                if (timeBtwShots <= 0)
                {
                    Instantiate(Resources.Load("FlyingEnemy"), firepoint, Quaternion.identity);
                    Instantiate(Resources.Load("FlyingEnemy3"), firepoint, Quaternion.identity);
                    timeBtwShots = startTimeBtwShots;
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }
            }
        }
    }
}
