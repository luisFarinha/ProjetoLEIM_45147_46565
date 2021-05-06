using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSaver : MonoBehaviour
{
    private PlayerController player;
    private Enemy[] enemiesInScene;
    private Chest[] chestsInScene;
    private UnlockableOrb[] unlockOrbsInScene;

    private bool firstTime = true;
 

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        enemiesInScene = GameObject.FindWithTag("Enemies").GetComponentsInChildren<Enemy>();
        chestsInScene = GameObject.FindWithTag("Chests").GetComponentsInChildren<Chest>();
        unlockOrbsInScene = GameObject.FindWithTag("UnlockableOrbs").GetComponentsInChildren<UnlockableOrb>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!firstTime)
        {
            try { enemiesInScene = GameObject.FindWithTag("Enemies").GetComponentsInChildren<Enemy>(); } catch (Exception) { }
            try { chestsInScene = GameObject.FindWithTag("Chests").GetComponentsInChildren<Chest>(); } catch (Exception) { }
            try { unlockOrbsInScene = GameObject.FindWithTag("UnlockableOrbs").GetComponentsInChildren<UnlockableOrb>(); } catch (Exception) { }

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
        SaveSystem.SaveData(player, enemiesInScene, chestsInScene, unlockOrbsInScene, SceneManager.GetActiveScene().name);
        //Debug.Log("Saved " + SceneManager.GetActiveScene().name);
    }


    public void LoadScene()
    {
        WorldData data = SaveSystem.LoadWorld();
        
        player.SetMoney(data.money);
        player.SetHealth(data.health);

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
                GetPickUpsData(data.chestsStatus01, data.unlockOrbsDone01);
                break;
        }


        //Debug.Log("Loaded " + SceneManager.GetActiveScene().name);
    }

    private void GetEnemyData(bool[] enemiesDead, int[] enemiesHealth)
    {
        for (int i = 0; i < enemiesInScene.Length; i++)
        {
            enemiesInScene[i].isDead = enemiesDead[i];
            enemiesInScene[i].currentHealth = enemiesHealth[i];
        }
    }

    private void GetPickUpsData(bool[] chestsStatus, bool[] unlockOrbsDone)
    {
        for(int i = 0; i < chestsInScene.Length; i++)
        {
            chestsInScene[i].isOpened = chestsStatus[i];
        }
        for (int i = 0; i < unlockOrbsDone.Length; i++)
        {
            try { unlockOrbsInScene[i].gameObject.SetActive(unlockOrbsDone[i]); } catch (Exception) { }
        }
    }
}
