using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public DataSaver ds;

    public int money;
    public bool canSpawn;
    
    [Header("Coins")]
    public float timeBetweenCoins = 0.2f;
    private GameObject smallCoin;

    [Header("Coin Spawn Randomizer")]
    private float randomSpawnX;
    private float randomSpawnY;

    private void Awake()
    {
        ds = GameObject.FindWithTag("DataSaver").GetComponent<DataSaver>();

        if (!canSpawn)
        {
            gameObject.transform.position = new Vector3(500, 500, 500);
        }
        smallCoin = (GameObject)Resources.Load(Constants.SMALL_COIN_TEXT);

        randomSpawnX = transform.localScale.x / 3;
        randomSpawnY = transform.localScale.y / 2;
    }

    private IEnumerator SpawnCoins(GameObject coin, int nCoins){
        for (int i = 0; i < nCoins; i++){
            Instantiate(coin, new Vector2(transform.position.x + Random.Range(-randomSpawnX, randomSpawnX),
                transform.position.y + randomSpawnY), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenCoins);
        }
        gameObject.transform.position = new Vector3(500, 500, 500);
        ds.ResetSoulData();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            StartCoroutine(SpawnCoins(smallCoin, money));
        }
    }
}
