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
    //SCENE01
    public bool[] enemiesDead01 = new bool[3];
    public int[] enemiesHealth01 = { 100, 100, 100 };
    public bool[] chestsStatus01 = new bool[1];
    public bool[] unlockOrbsDone01 = new bool[1];
    //SCENE02
    public bool[] enemiesDead02 = new bool[2];
    public int[] enemiesHealth02 = new int[2];
    //SCENE03
    public bool[] enemiesDead03 = new bool[2];
    public int[] enemiesHealth03 = new int[2];
    //SCENE04
    public bool[] enemiesDead04 = new bool[2];
    public int[] enemiesHealth04 = new int[2];
    //SCENE05
    public bool[] enemiesDead05 = new bool[2];
    public int[] enemiesHealth05 = new int[2];
    //SCENE06
    public bool[] enemiesDead06 = new bool[2];
    public int[] enemiesHealth06 = new int[2];

    public int money = 0;
    public int health = 100;
    public float[] position;
    
    //[System.NonSerialized]
    //public Enemy jones;
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

    public void InsertCheckpointData(Vector3 checkPoint)
    {
        position = new float[3];
        position[0] = checkPoint.x;
        position[1] = checkPoint.y;
        position[2] = checkPoint.z;
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

    public void InsertDataOnPlayerDeath()
    {
        money = 0;
        health = 100;

        ReviveEnemies(enemiesDead00, enemiesHealth00); //SCENE00
        ReviveEnemies(enemiesDead01, enemiesHealth01); //SCENE01
        ReviveEnemies(enemiesDead02, enemiesHealth02); //SCENE02
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
