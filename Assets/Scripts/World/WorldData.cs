using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WorldData
{
    public int money;
    public int health;
    public float[] position;

    public WorldData(PlayerController player) {

        health = player.currentHealth;
        money = player.currentMoney;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
    
}
