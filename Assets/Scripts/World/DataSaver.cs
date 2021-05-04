using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSaver : MonoBehaviour
{
    private Enemy[] EnemiesInScene;
    private PlayerController player;
    void Start()
    {
        EnemiesInScene = GameObject.FindWithTag("Enemies").GetComponentsInChildren<Enemy>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(player, EnemiesInScene, SceneManager.GetActiveScene().name);
        Debug.Log("Saved");
    }


    public void LoadPlayer()
    {
        WorldData data = SaveSystem.LoadWorld();
        Debug.Log(player.currentMoney);

        player.currentMoney = data.money;
        player.moneyText.text = player.currentMoney.ToString();
        player.currentHealth = data.health;
        player.slider.value = player.currentHealth;
        player.followSlider.value = player.currentHealth;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        player.transform.position = position;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Room_00":
                GetEnemyData(data.enemiesAlive00, data.enemiesHealth00);
                break;
            case "Room_01":
                GetEnemyData(data.enemiesAlive01, data.enemiesHealth01);
                break;
        }


        Debug.Log("Loaded");
    }

    private void GetEnemyData(bool[] enemiesAlive, int[] enemiesHealth)
    {
        Debug.Log(enemiesAlive.Length);

        for (int i = 0; i < EnemiesInScene.Length; i++)
        {
            EnemiesInScene[i].isDead = enemiesAlive[i];
            EnemiesInScene[i].currentHealth = enemiesHealth[i];
        }
    }
}
