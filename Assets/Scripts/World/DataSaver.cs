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
    private Vector3 checkPoint;
    private Soul soul;

    [Header("Animation Components")]
    private UIManager uim;

    private bool firstTime = true;
    private bool deathChecked;



    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        enemiesInScene = GameObject.FindWithTag("Enemies").GetComponentsInChildren<Enemy>();
        chestsInScene = GameObject.FindWithTag("Chests").GetComponentsInChildren<Chest>();
        unlockOrbsInScene = GameObject.FindWithTag("UnlockableOrbs").GetComponentsInChildren<UnlockableOrb>();
        checkPoint = GameObject.FindWithTag("StartPos_1").transform.position;
        soul = GameObject.FindWithTag("Soul").GetComponent<Soul>();

        uim = GameObject.FindWithTag("UI").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckPlayerDeath());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!firstTime)
        {
            try { enemiesInScene = GameObject.FindWithTag("Enemies").GetComponentsInChildren<Enemy>(); } catch (Exception) { }
            try { chestsInScene = GameObject.FindWithTag("Chests").GetComponentsInChildren<Chest>(); } catch (Exception) { }
            try { unlockOrbsInScene = GameObject.FindWithTag("UnlockableOrbs").GetComponentsInChildren<UnlockableOrb>(); } catch (Exception) { }
            try { checkPoint = GameObject.FindWithTag("StartPos_1").transform.position; } catch (Exception) { }
            try { soul = GameObject.FindWithTag("Soul").GetComponent<Soul>(); } catch (Exception) { }

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
        Debug.Log("Saved " + SceneManager.GetActiveScene().name);
    }

    public void LoadScene()
    {
        SaveSystem.SaveCheckPoint(checkPoint);
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
                GetSoulData(data.soulMoney00, data.soulPosition00, data.soulCanSpawn00);
                break;
            case "Room_01":
                GetEnemyData(data.enemiesDead01, data.enemiesHealth01);
                GetPickUpsData(data.chestsStatus01, data.unlockOrbsDone01);
                GetSoulData(data.soulMoney01, data.soulPosition01, data.soulCanSpawn01);
                break;
        }

        Debug.Log("Loaded " + SceneManager.GetActiveScene().name);
    }

    public IEnumerator CheckPlayerDeath()
    {
        if (player.isDead && !deathChecked)
        {
            deathChecked = true;
            Debug.Log("Player Died");
            ResetSoulData();
            SaveSystem.SaveDataOnPlayerDeath(player, SceneManager.GetActiveScene().name);
            uim.SceneTransitionFadeIn();
            yield return new WaitForSecondsRealtime(uim.anim.speed + 0.1f); //(+0.1) para que a personagem se consiga teleportar com a OnLoadScene do PlayerController
            LoadScene();
            uim.SceneTransitionFadeOut();
            deathChecked = false;
        }

    }

    public void ResetSoulData()
    {
        SaveSystem.ResetSoul();
    }

    private void GetEnemyData(bool[] enemiesDead, int[] enemiesHealth)
    {
        for (int i = 0; i < enemiesInScene.Length; i++)
        {
            enemiesInScene[i].isDead = enemiesDead[i];
            enemiesInScene[i].SetHealth(enemiesHealth[i]);
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

    private void GetSoulData(int soulMoney, float[] soulPosition, bool soulCanSpawn)
    {
        soul.money = soulMoney;
        soul.transform.position = new Vector3(soulPosition[0], soulPosition[1], soulPosition[2]);
        soul.canSpawn = soulCanSpawn;
    }
}
