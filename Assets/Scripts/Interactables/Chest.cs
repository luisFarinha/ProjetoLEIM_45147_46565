using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [Header("Components")]
    public GameObject warning;
    public GameObject openMeter;
    private Slider meter;
    private Image img;
    private InputMaster im;
    private Animator anim;
    private SpriteRenderer sr;

    [Header("Opening Action")]
    public float openIncrement = 20f;
    public float closeIncrement = 1f;
    public bool isOpened;
    private bool isCloseBy;
    private bool hasOpened;

    [Header("Coins")]
    public float timeBetweenCoins = 0.2f;
    public int nSmallCoins;
    public int nMediumCoins;
    public int nLargeCoins;
    private GameObject smallCoin;
    private GameObject mediumCoin;
    private GameObject largeCoin;

    [Header("Coin Spawn Randomizer")]
    private float randomSpawnX;
    private float randomSpawnY;

    // Start is called before the first frame update
    void Awake()
    {
        meter = openMeter.GetComponent<Slider>();
        img = meter.GetComponentInChildren<Image>();
        anim = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        smallCoin = (GameObject)Resources.Load(Constants.SMALL_COIN_TEXT);
        mediumCoin = (GameObject)Resources.Load(Constants.MEDIUM_COIN_TEXT);
        largeCoin = (GameObject)Resources.Load(Constants.LARGE_COIN_TEXT);

        randomSpawnX = transform.localScale.x / 3;
        randomSpawnY = transform.localScale.y / 2;

        im = new InputMaster();

        im.Player.Interact.started += _ => IncreaseMeter();
    }

    private void OnEnable()
    {
        im.Enable();
    }

    private void OnDisable()
    {
        im.Disable();
    }

    private void FixedUpdate()
    {
        CheckOpened();
        DecreaseMeter();
        ChangeMeterColor();
    }

    private void IncreaseMeter()
    {
        if (meter.value < 100 && !isOpened && isCloseBy)
        {
            anim.Play(Constants.CHEST_WIGGLE);
            meter.value += openIncrement;
        }
    }

    private void DecreaseMeter()
    {
        if(meter.value > 0)
        {
            meter.value -= closeIncrement;
        }
    }

    private void CheckOpened()
    {
        if(meter.value == 100)
        {
            isOpened = true;
            warning.SetActive(false);
            openMeter.SetActive(false);

            StartCoroutine(SpawnCoins(smallCoin, nSmallCoins));
            StartCoroutine(SpawnCoins(mediumCoin, nMediumCoins));
            StartCoroutine(SpawnCoins(largeCoin, nLargeCoins));

            sr.sprite = Resources.Load<Sprite>("chestOpened");
            anim.Play(Constants.CHEST_OPENED_IDLE);
        }
        else if(isOpened && !hasOpened)
        {
            hasOpened = true;
            warning.SetActive(false);
            openMeter.SetActive(false);
            sr.sprite = Resources.Load<Sprite>("chestOpened");
            anim.Play(Constants.CHEST_OPENED_IDLE);
        }
    }

    private void ChangeMeterColor()
    {
        img.color = new Color32((byte)(meter.value*2.55f), (byte)(255 - meter.value * 2.55f), 0, 255);
    }

    private IEnumerator SpawnCoins(GameObject coin, int nCoins)
    {
        for (int i = 0; i < nCoins; i++)
        {
            Instantiate(coin, new Vector2(transform.position.x + Random.Range(-randomSpawnX, randomSpawnX), transform.position.y + randomSpawnY), Quaternion.identity);
            
            yield return new WaitForSeconds(timeBetweenCoins);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpened)
        {
            isCloseBy = true;
            warning.SetActive(true);
            openMeter.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpened)
        {
            isCloseBy = false;
            warning.SetActive(false);
            openMeter.SetActive(false);
            meter.value = 0;
        }
    }
}
