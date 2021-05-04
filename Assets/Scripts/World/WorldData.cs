using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;


[System.Serializable]
public class WorldData
{
    //SCENE00
    public bool[] enemiesAlive00 = new bool[1];
    public int[] enemiesHealth00 = new int[1];
    //SCENE01
    public bool[] enemiesAlive01 = new bool[3];
    public int[] enemiesHealth01 = new int[3];
    //SCENE02
    public bool[] enemiesAlive02 = new bool[2];
    public int[] enemiesHealth02 = new int[2];
    //SCENE03
    public bool[] enemiesAlive03 = new bool[2];
    public int[] enemiesHealth03 = new int[2];
    //SCENE04
    public bool[] enemiesAlive04 = new bool[2];
    public int[] enemiesHealth04 = new int[2];
    //SCENE05
    public bool[] enemiesAlive05 = new bool[2];
    public int[] enemiesHealth05 = new int[2];
    //SCENE06
    public bool[] enemiesAlive06 = new bool[2];
    public int[] enemiesHealth06 = new int[2];

    public int money;
    public int health;
    public float[] position;
    public WorldData(PlayerController player, Enemy[] enemies, string scene) {
        switch (scene)
        {
            case "Room_00":
                UpdateEnemyData(enemies, enemiesAlive00, enemiesHealth00);
                break;
            case "Room_01":
                UpdateEnemyData(enemies, enemiesAlive01, enemiesHealth01);
                break;
            case "Room_02":
                UpdateEnemyData(enemies, enemiesAlive02, enemiesHealth02);
                break;
            case "Room_03":
                UpdateEnemyData(enemies, enemiesAlive03, enemiesHealth03);
                break;
            case "Room_04":
                UpdateEnemyData(enemies, enemiesAlive04, enemiesHealth04);
                break;
        }

        UpdatePlayerData(player);
    }

    private void UpdateEnemyData(Enemy[] enemies, bool[] enemiesAlive, int[] enemiesHealth)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesAlive[i] = enemies[i].isDead;
            enemiesHealth[i] = enemies[i].currentHealth;
        }
    }

    private void UpdatePlayerData(PlayerController player)
    {
        health = player.currentHealth;
        money = player.currentMoney;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
