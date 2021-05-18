using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;


/*
*Options (as soon as modified)
**** ON CHECKPOINT ****
*Player (health, money, position, unlockables)
*Enemy (restart health, position)
*Inventory (collectibles, unlockables, ...)
*Treasure Chests (state)
*NPC Missions
*
**** ON CHANGE SCENE ****
*Enemy (state, health)
*Treasure Chests (state)
*Collectibles
*
*
*/

[System.Serializable]
public class WorldData
{
    //SCENE00
    public bool[] enemiesDead00 = new bool[1];
    public int[] enemiesHealth00 = { 100 };
    public int soulMoney00;
    public float[] soulPosition00 = { 500, 500, 500 };
    public bool soulCanSpawn00;
    //SCENE01
    public bool[] enemiesDead01 = new bool[3];
    public int[] enemiesHealth01 = { 100, 100, 100 };
    public bool[] chestsStatus01 = new bool[1];
    public bool[] unlockOrbsDone01 = new bool[1];
    public int soulMoney01;
    public float[] soulPosition01 = { 500, 500, 500 };
    public bool soulCanSpawn01;
    //SCENE02
    public bool[] enemiesDead02 = new bool[2];
    public int[] enemiesHealth02 = new int[2];
    public int soulMoney02;
    public float[] soulPosition02 = { 500, 500, 500 };
    public bool soulCanSpawn02;
    //SCENE03
    public bool[] enemiesDead03 = new bool[2];
    public int[] enemiesHealth03 = new int[2];
    public int soulMoney03;
    public float[] soulPosition03 = { 500, 500, 500 };
    public bool soulCanSpawn03;
    //SCENE04
    public bool[] enemiesDead04 = new bool[2];
    public int[] enemiesHealth04 = new int[2];
    public int soulMoney04;
    public float[] soulPosition04 = { 500, 500, 500 };
    public bool soulCanSpawn04;
    //SCENE05
    public bool[] enemiesDead05 = new bool[2];
    public int[] enemiesHealth05 = new int[2];
    public int soulMoney05;
    public float[] soulPosition05 = { 500, 500, 500 };
    public bool soulCanSpawn05;
    //SCENE06
    public bool[] enemiesDead06 = new bool[2];
    public int[] enemiesHealth06 = new int[2];
    public int soulMoney06;
    public float[] soulPosition06 = { 500, 500, 500 };
    public bool soulCanSpawn06;

    public int money = 0;
    public int health = 100;
    public float[] position;

    public string lastSavedScene = "Room_00";
    public string lastSavedTime;

    public void InsertData(PlayerController player, Enemy[] enemies, Chest[] chests, UnlockableOrb[] unlockOrbs, string scene) {
        switch (scene)
        {
            case "Room_00":
                UpdateEnemyData(enemies, enemiesDead00, enemiesHealth00);
                break;
            case "Room_01":
                UpdateEnemyData(enemies, enemiesDead01, enemiesHealth01);
                UpdatePickUpsStatus(chests, chestsStatus01, unlockOrbs, unlockOrbsDone01);
                break;
            case "Room_02":
                UpdateEnemyData(enemies, enemiesDead02, enemiesHealth02);
                break;
            case "Room_03":
                UpdateEnemyData(enemies, enemiesDead03, enemiesHealth03);
                break;
            case "Room_04":
                UpdateEnemyData(enemies, enemiesDead04, enemiesHealth04);
                break;
        }
        UpdatePlayerData(player);
    }

    private void UpdateEnemyData(Enemy[] enemies, bool[] enemiesDead, int[] enemiesHealth)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesDead[i] = enemies[i].isDead;
            enemiesHealth[i] = enemies[i].currentHealth;
        }
    }

    private void UpdatePlayerData(PlayerController player)
    {
        health = player.currentHealth;
        money = player.currentMoney;
    }

    public void InsertCheckpointData(Vector3 checkPoint, string scene)
    {
        position = new float[3];
        position[0] = checkPoint.x;
        position[1] = checkPoint.y;
        position[2] = checkPoint.z;

        lastSavedScene = scene;
        lastSavedTime = System.DateTime.Now.ToString();
    }

    private void UpdatePickUpsStatus(Chest[] chests, bool[] chestsStatus, UnlockableOrb[] unlockableOrbs, bool[] unlockOrbsDone)
    {
        for(int i = 0; i < chests.Length; i++)
        {
            chestsStatus[i] = chests[i].isOpened;
        }
        for (int i = 0; i < unlockableOrbs.Length; i++)
        {
            unlockOrbsDone[i] = unlockableOrbs[i].isActiveAndEnabled;
        }
    }

    public void InsertDataOnPlayerDeath(PlayerController player, string scene)
    {
    switch (scene)
    {
        case "Room_00":
            soulCanSpawn00 = true;
            soulMoney00 = player.currentMoney;
            soulPosition00 = new float[3];
            soulPosition00[0] = player.transform.position.x;
            soulPosition00[1] = player.transform.position.y;
            soulPosition00[2] = player.transform.position.z;
            break;
        case "Room_01":
            soulCanSpawn01 = true;
            soulMoney01 = player.currentMoney;
            soulPosition01 = new float[3];
            soulPosition01[0] = player.transform.position.x;
            soulPosition01[1] = player.transform.position.y;
            soulPosition01[2] = player.transform.position.z;
            break;
        case "Room_02":
            soulCanSpawn02 = true;
            soulMoney02 = player.currentMoney;
            soulPosition02 = new float[3];
            soulPosition02[0] = player.transform.position.x;
            soulPosition02[1] = player.transform.position.y;
            soulPosition02[2] = player.transform.position.z;
            break;
        case "Room_03":
            soulCanSpawn03 = true;
            soulMoney03 = player.currentMoney;
            soulPosition03 = new float[3];
            soulPosition03[0] = player.transform.position.x;
            soulPosition03[1] = player.transform.position.y;
            soulPosition03[2] = player.transform.position.z;
            break;
        case "Room_04":
            soulCanSpawn04 = true;
            soulMoney04 = player.currentMoney;
            soulPosition04 = new float[3];
            soulPosition04[0] = player.transform.position.x;
            soulPosition04[1] = player.transform.position.y;
            soulPosition04[2] = player.transform.position.z;
            break;
    }
    money = 0;
    health = 100;

    ReviveEnemies(enemiesDead00, enemiesHealth00); //SCENE00
    ReviveEnemies(enemiesDead01, enemiesHealth01); //SCENE01
    ReviveEnemies(enemiesDead02, enemiesHealth02); //SCENE02
    }

    public void ResetSoulData()
    {
        soulCanSpawn00 = false;
        soulCanSpawn01 = false;
        soulCanSpawn02 = false;
        soulCanSpawn03 = false;
        soulCanSpawn04 = false;
        soulCanSpawn05 = false;
        soulCanSpawn06 = false;

        soulPosition00[0] = 500; soulPosition00[1] = 500; soulPosition00[2] = 500;
        soulPosition01[0] = 500; soulPosition01[1] = 500; soulPosition01[2] = 500;
        soulPosition02[0] = 500; soulPosition02[1] = 500; soulPosition02[2] = 500;
        soulPosition03[0] = 500; soulPosition03[1] = 500; soulPosition03[2] = 500;
        soulPosition04[0] = 500; soulPosition04[1] = 500; soulPosition04[2] = 500;
        soulPosition05[0] = 500; soulPosition05[1] = 500; soulPosition05[2] = 500;
        soulPosition06[0] = 500; soulPosition06[1] = 500; soulPosition06[2] = 500;
    }

    public void ReviveEnemies(bool[] enemiesDead, int[] enemiesHealth)
    {
        for(int i = 0; i < enemiesHealth.Length; i++)
        {
            enemiesDead[i] = false;
            enemiesHealth[i] = 100;
        }
    }
}
