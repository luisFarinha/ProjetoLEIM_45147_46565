using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{


    [Header("Components")]
    private Transform target;
    private Text moneyText;
    private Rigidbody2D rb;
    private ParticleSystem ps;
    private SpriteRenderer sr;

    [Header("Force Range")]
    private float xRange = Constants.COIN_X_RANGE;
    private float yMinRange = Constants.COIN_Y_MIN_RANGE;
    private float yMaxRange = Constants.COIN_Y_MAX_RANGE;
    private float timeUntilFollow = Constants.COIN_TIME_UNTIL_FOLLOW;

    [Header("Follow Range")]
    private float acceleration = Constants.COIN_ACCELERATION;
    private float speedVariation = Constants.COIN_SPEED_VARIATION;
    private float followSpeed = 0;
    private bool isFollowing = false;
    
    void Start()
    {
        target = GameObject.Find(Constants.PLAYER_OBJECT).GetComponent<Transform>();
        moneyText = GameObject.Find(Constants.MONEY_TEXT).GetComponent<Text>();
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();

        rb.AddForce(new Vector2(Random.Range(-xRange, xRange), Random.Range(yMinRange, yMaxRange)), ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }
    
    private void FollowPlayer()
    {

        if (isFollowing)
        {

            rb.gravityScale = 0;

            transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * (followSpeed + Random.Range(0, speedVariation)));
            followSpeed += acceleration;         
        }
        else
        {
            Invoke("StartFollowing", timeUntilFollow);
        }
    }

    private void StartFollowing()
    {
        isFollowing = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isFollowing)
        {
            if (sr.enabled) //make sure coins only add once to the money text
            {
                if (gameObject.CompareTag(Constants.SMALL_COIN_TEXT))
                {
                    collision.GetComponent<PlayerController>().AddMoney(Constants.SMALL_COIN_VALUE);
                }
                else if (gameObject.CompareTag(Constants.MEDIUM_COIN_TEXT))
                {
                    collision.GetComponent<PlayerController>().AddMoney(Constants.SMALL_COIN_VALUE);
                }
                else if (gameObject.CompareTag(Constants.LARGE_COIN_TEXT))
                {
                    collision.GetComponent<PlayerController>().AddMoney(Constants.SMALL_COIN_VALUE);
                }
            }
            sr.enabled = false;
            ps.Play();
            Invoke("KillMe", 0.2f);
        }
    }

    private void KillMe()
    {
        Destroy(gameObject);
    }
}
