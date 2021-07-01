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

    private bool deathChecked;



    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        uim = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        
        enemiesInScene = GameObject.FindWithTag("Enemies").GetComponentsInChildren<Enemy>();
        chestsInScene = GameObject.FindWithTag("Chests").GetComponentsInChildren<Chest>();
        unlockOrbsInScene = GameObject.FindWithTag("UnlockableOrbs").GetComponentsInChildren<UnlockableOrb>();
        checkPoint = GameObject.FindWithTag("StartPos_1").transform.position;
        soul = GameObject.FindWithTag("Soul").GetComponent<Soul>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckPlayerDeath());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try { enemiesInScene = GameObject.FindWithTag("Enemies").GetComponentsInChildren<Enemy>(); } catch (Exception) { }
        try { chestsInScene = GameObject.FindWithTag("Chests").GetComponentsInChildren<Chest>(); } catch (Exception) { }
        try { unlockOrbsInScene = GameObject.FindWithTag("UnlockableOrbs").GetComponentsInChildren<UnlockableOrb>(); } catch (Exception) { }
        try { checkPoint = GameObject.FindWithTag(player.startPos).transform.position; } catch (Exception) { }
        try { soul = GameObject.FindWithTag("Soul").GetComponent<Soul>(); } catch (Exception) { }

        LoadScene();
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
        WorldData data = SaveSystem.LoadWorld();
        if (data != null)
        {
            SaveSystem.SaveCheckPoint(checkPoint, SceneManager.GetActiveScene().name);
            player.SetMoney(data.money);
            player.SetHealthInstantly(data.health);

            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            player.transform.position = position;

            if (data.unlockOrbsDone01[0])
            {
                Unlockables.glideUnlocked = true;
            }
            if (data.unlockOrbsDone03[0])
            {
                Unlockables.doubleJumpUnlocked = true;
            }
            if (data.unlockOrbsDone04[0])
            {
                Unlockables.dashUnlocked = true;
            }
            if (data.unlockOrbsDone07[0])
            {
                Unlockables.wallJumpUnlocked = true;
            }

            switch (SceneManager.GetActiveScene().name)
            {
                case "Room_00":
                    GetEnemyData(data.enemiesDead00, data.enemiesHealth00);
                    GetSoulData(data.soulMoney00, data.soulPosition00, data.soulCanSpawn00);
                    break;
                case "Room_01":
                    GetEnemyData(data.enemiesDead01, data.enemiesHealth01);
                    GetChestData(data.chestsStatus01);
                    GetUnlockableData(data.unlockOrbsDone01);
                    GetSoulData(data.soulMoney01, data.soulPosition01, data.soulCanSpawn01);
                    break;
                case "Room_02":
                    GetEnemyData(data.enemiesDead02, data.enemiesHealth02);
                    GetChestData(data.chestsStatus02);
                    GetSoulData(data.soulMoney02, data.soulPosition02, data.soulCanSpawn02);
                    break;
                case "Room_03":
                    GetEnemyData(data.enemiesDead03, data.enemiesHealth03);
                    GetUnlockableData(data.unlockOrbsDone03);
                    GetSoulData(data.soulMoney03, data.soulPosition03, data.soulCanSpawn03);
                    break;
                case "Room_04":
                    GetUnlockableData(data.unlockOrbsDone04);
                    GetSoulData(data.soulMoney04, data.soulPosition04, data.soulCanSpawn04);
                    break;
                case "Room_05":
                    GetEnemyData(data.enemiesDead05, data.enemiesHealth05);
                    GetSoulData(data.soulMoney05, data.soulPosition05, data.soulCanSpawn05);
                    break;
                case "Room_06":
                    GetSoulData(data.soulMoney06, data.soulPosition06, data.soulCanSpawn06);
                    break;
                case "Room_07":
                    GetUnlockableData(data.unlockOrbsDone07);
                    GetSoulData(data.soulMoney07, data.soulPosition07, data.soulCanSpawn07);
                    break;
                case "Room_08":
                    GetEnemyData(data.enemiesDead08, data.enemiesHealth08);
                    GetChestData(data.chestsStatus08);
                    GetSoulData(data.soulMoney08, data.soulPosition08, data.soulCanSpawn08);
                    break;
                case "Room_09":
                    GetEnemyData(data.enemiesDead09, data.enemiesHealth09);
                    GetSoulData(data.soulMoney09, data.soulPosition09, data.soulCanSpawn09);
                    break;
                case "Room_10":
                    GetSoulData(data.soulMoney10, data.soulPosition10, data.soulCanSpawn10);
                    break;
            }

            Debug.Log("Loaded " + SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("Loaded " + SceneManager.GetActiveScene().name + " for the 1st time");
        }
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

    private void GetChestData(bool[] chestsStatus)
    {
        for (int i = 0; i < chestsInScene.Length; i++)
        {
            chestsInScene[i].isOpened = chestsStatus[i];
        }
    }

    private void GetUnlockableData(bool[] unlockOrbsDone)
    {
        for (int i = 0; i < unlockOrbsDone.Length; i++)
        {
            try { unlockOrbsInScene[i].gameObject.SetActive(!unlockOrbsDone[i]); } catch (Exception) { }
        }
    }

    private void GetSoulData(int soulMoney, float[] soulPosition, bool soulCanSpawn)
    {
        soul.money = soulMoney;
        soul.transform.position = new Vector3(soulPosition[0], soulPosition[1], soulPosition[2]);
        soul.canSpawn = soulCanSpawn;
    }
}
