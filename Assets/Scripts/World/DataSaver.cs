using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSaver : MonoBehaviour
{
    private Enemy[] EnemiesInScene;
    private PlayerController player;
    private bool firstTime = true;
 

    void Start()
    {
        EnemiesInScene = GameObject.FindWithTag("Enemies").GetComponentsInChildren<Enemy>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!firstTime)
        {
            EnemiesInScene = GameObject.FindWithTag("Enemies").GetComponentsInChildren<Enemy>();
            LoadScene();
        }
        firstTime = false;
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SaveScene()
    {
        SaveSystem.SaveData(player, EnemiesInScene, SceneManager.GetActiveScene().name);
        Debug.Log("Saved " + SceneManager.GetActiveScene().name);
    }


    public void LoadScene()
    {
        WorldData data = SaveSystem.LoadWorld();
        
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
                GetEnemyData(data.enemiesDead00, data.enemiesHealth00);
                break;
            case "Room_01":
                GetEnemyData(data.enemiesDead01, data.enemiesHealth01);
                break;
        }


        Debug.Log("Loaded " + SceneManager.GetActiveScene().name);
    }

    private void GetEnemyData(bool[] enemiesDead, int[] enemiesHealth)
    {
        Debug.Log(EnemiesInScene.Length);
        for (int i = 0; i < EnemiesInScene.Length; i++)
        {
            EnemiesInScene[i].isDead = enemiesDead[i];
            EnemiesInScene[i].currentHealth = enemiesHealth[i];
        }
    }
}
