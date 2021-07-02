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
    public int[] enemiesMaxHealth00 = { 100 };
    public int soulMoney00;
    public float[] soulPosition00 = { 500, 500, 500 };
    public bool soulCanSpawn00;
    //SCENE01
    public bool[] enemiesDead01 = new bool[4];
    public int[] enemiesHealth01 = { 200, 100, 100, 100 };
    public int[] enemiesMaxHealth01 = { 200, 100, 100, 100 };
    public bool[] chestsStatus01 = new bool[1];
    public bool[] unlockOrbsDone01 = new bool[1];
    public int soulMoney01;
    public float[] soulPosition01 = { 500, 500, 500 };
    public bool soulCanSpawn01;
    //SCENE02
    public bool[] enemiesDead02 = new bool[3];
    public int[] enemiesHealth02 = { 300, 100, 100 };
    public int[] enemiesMaxHealth02 = { 300, 100, 100 };
    public bool[] chestsStatus02 = new bool[1];
    public int soulMoney02;
    public float[] soulPosition02 = { 500, 500, 500 };
    public bool soulCanSpawn02;
    //SCENE03
    public bool[] enemiesDead03 = new bool[6];
    public int[] enemiesHealth03 = { 100, 100, 100, 100, 100, 100 };
    public int[] enemiesMaxHealth03 = { 100, 100, 100, 100, 100, 100 };
    public bool[] unlockOrbsDone03 = new bool[1];
    public int soulMoney03;
    public float[] soulPosition03 = { 500, 500, 500 };
    public bool soulCanSpawn03;
    //SCENE04
    public bool[] unlockOrbsDone04 = new bool[1];
    public int soulMoney04;
    public float[] soulPosition04 = { 500, 500, 500 };
    public bool soulCanSpawn04;
    //SCENE05
    public bool[] enemiesDead05 = new bool[5];
    public int[] enemiesHealth05 = { 100, 100, 100, 100, 100 };
    public int[] enemiesMaxHealth05 = { 100, 100, 100, 100, 100 };
    public int soulMoney05;
    public float[] soulPosition05 = { 500, 500, 500 };
    public bool soulCanSpawn05;
    //SCENE06
    public int soulMoney06;
    public float[] soulPosition06 = { 500, 500, 500 };
    public bool soulCanSpawn06;
    //SCENE07
    public bool[] unlockOrbsDone07 = new bool[1];
    public int soulMoney07;
    public float[] soulPosition07 = { 500, 500, 500 };
    public bool soulCanSpawn07;
    //SCENE08
    public bool[] enemiesDead08 = new bool[2];
    public int[] enemiesHealth08 = { 300, 200 };
    public int[] enemiesMaxHealth08 = { 300, 200 };
    public bool[] chestsStatus08 = new bool[1];
    public int soulMoney08;
    public float[] soulPosition08 = { 500, 500, 500 };
    public bool soulCanSpawn08;
    //SCENE09
    public bool[] enemiesDead09 = new bool[1];
    public int[] enemiesHealth09 = { 400 };
    public int[] enemiesMaxHealth09 = { 400 };
    public int soulMoney09;
    public float[] soulPosition09 = { 500, 500, 500 };
    public bool soulCanSpawn09;
    //SCENE10
    public int soulMoney10;
    public float[] soulPosition10 = { 500, 500, 500 };
    public bool soulCanSpawn10;

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
                UpdateUnlockableStatus(unlockOrbs, unlockOrbsDone01);
                UpdateChestStatus(chests, chestsStatus01);
                break;
            case "Room_02":
                UpdateEnemyData(enemies, enemiesDead02, enemiesHealth02);
                UpdateChestStatus(chests, chestsStatus02);
                break;
            case "Room_03":
                UpdateEnemyData(enemies, enemiesDead03, enemiesHealth03);
                UpdateUnlockableStatus(unlockOrbs, unlockOrbsDone03);
                break;
            case "Room_04":
                UpdateUnlockableStatus(unlockOrbs, unlockOrbsDone04);
                break;
            case "Room_05":
                UpdateEnemyData(enemies, enemiesDead05, enemiesHealth05);
                break;
            case "Room_06":
                //-------------
                break;
            case "Room_07":
                UpdateUnlockableStatus(unlockOrbs, unlockOrbsDone07);
                break;
            case "Room_08":
                UpdateEnemyData(enemies, enemiesDead08, enemiesHealth08);
                UpdateChestStatus(chests, chestsStatus08);
                break;
            case "Room_09":
                UpdateEnemyData(enemies, enemiesDead09, enemiesHealth09);
                break;
            case "Room_10":
                //-------------
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

    private void UpdateUnlockableStatus(UnlockableOrb[] unlockableOrbs, bool[] unlockOrbsDone)
    {
        for (int i = 0; i < unlockableOrbs.Length; i++)
        {
            unlockOrbsDone[i] = !unlockableOrbs[i].isActiveAndEnabled;
        }
    }

    private void UpdateChestStatus(Chest[] chests, bool[] chestsStatus)
    {
        for (int i = 0; i < chests.Length; i++)
        {
            chestsStatus[i] = chests[i].isOpened;
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
        case "Room_05":
            soulCanSpawn05 = true;
            soulMoney05 = player.currentMoney;
            soulPosition05 = new float[3];
            soulPosition05[0] = player.transform.position.x;
            soulPosition05[1] = player.transform.position.y;
            soulPosition05[2] = player.transform.position.z;
            break;
        case "Room_06":
            soulCanSpawn06 = true;
            soulMoney06 = player.currentMoney;
            soulPosition06 = new float[3];
            soulPosition06[0] = player.transform.position.x;
            soulPosition06[1] = player.transform.position.y;
            soulPosition06[2] = player.transform.position.z;
            break;
        case "Room_07":
            soulCanSpawn07 = true;
            soulMoney07 = player.currentMoney;
            soulPosition07 = new float[3];
            soulPosition07[0] = player.transform.position.x;
            soulPosition07[1] = player.transform.position.y;
            soulPosition07[2] = player.transform.position.z;
            break;
        case "Room_08":
            soulCanSpawn08 = true;
            soulMoney08 = player.currentMoney;
            soulPosition08 = new float[3];
            soulPosition08[0] = player.transform.position.x;
            soulPosition08[1] = player.transform.position.y;
            soulPosition08[2] = player.transform.position.z;
            break;
        case "Room_09":
            soulCanSpawn09 = true;
            soulMoney09 = player.currentMoney;
            soulPosition09 = new float[3];
            soulPosition09[0] = player.transform.position.x;
            soulPosition09[1] = player.transform.position.y;
            soulPosition09[2] = player.transform.position.z;
            break;
        case "Room_10":
            soulCanSpawn10 = true;
            soulMoney10 = player.currentMoney;
            soulPosition10 = new float[3];
            soulPosition10[0] = player.transform.position.x;
            soulPosition10[1] = player.transform.position.y;
            soulPosition10[2] = player.transform.position.z;
            break;
        }
    money = 0;
    health = 100;

    ReviveEnemies(enemiesDead00, enemiesHealth00, "Room_00"); //SCENE00
    ReviveEnemies(enemiesDead01, enemiesHealth01, "Room_01"); //SCENE01
    ReviveEnemies(enemiesDead02, enemiesHealth02, "Room_02"); //SCENE02
    ReviveEnemies(enemiesDead03, enemiesHealth03, "Room_03"); //SCENE03
    //ReviveEnemies(enemiesDead04, enemiesHealth04); //SCENE04
    ReviveEnemies(enemiesDead05, enemiesHealth05, "Room_05"); //SCENE05
    //ReviveEnemies(enemiesDead06, enemiesHealth06); //SCENE06
    //ReviveEnemies(enemiesDead07, enemiesHealth07); //SCENE07
    ReviveEnemies(enemiesDead08, enemiesHealth08, "Room_08"); //SCENE08
    ReviveEnemies(enemiesDead09, enemiesHealth09, "Room_09"); //SCENE09
    //ReviveEnemies(enemiesDead10, enemiesHealth10); //SCENE10
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
        soulCanSpawn07 = false;
        soulCanSpawn08 = false;
        soulCanSpawn09 = false;
        soulCanSpawn10 = false;

        soulPosition00[0] = 500; soulPosition00[1] = 500; soulPosition00[2] = 500;
        soulPosition01[0] = 500; soulPosition01[1] = 500; soulPosition01[2] = 500;
        soulPosition02[0] = 500; soulPosition02[1] = 500; soulPosition02[2] = 500;
        soulPosition03[0] = 500; soulPosition03[1] = 500; soulPosition03[2] = 500;
        soulPosition04[0] = 500; soulPosition04[1] = 500; soulPosition04[2] = 500;
        soulPosition05[0] = 500; soulPosition05[1] = 500; soulPosition05[2] = 500;
        soulPosition06[0] = 500; soulPosition06[1] = 500; soulPosition06[2] = 500;
        soulPosition07[0] = 500; soulPosition07[1] = 500; soulPosition07[2] = 500;
        soulPosition08[0] = 500; soulPosition08[1] = 500; soulPosition08[2] = 500;
        soulPosition09[0] = 500; soulPosition09[1] = 500; soulPosition09[2] = 500;
        soulPosition10[0] = 500; soulPosition10[1] = 500; soulPosition10[2] = 500;
    }

    public void ReviveEnemies(bool[] enemiesDead, int[] enemiesHealth, string scene)
    {
        switch (scene)
        {
            case "Room_00":
                for (int i = 0; i < enemiesHealth.Length; i++)
                {
                    enemiesDead[i] = false;
                    enemiesHealth[i] = enemiesMaxHealth00[i];
                }
                break;
            case "Room_01":
                for (int i = 0; i < enemiesHealth.Length; i++)
                {
                    enemiesDead[i] = false;
                    enemiesHealth[i] = enemiesMaxHealth01[i];
                }
                break;
            case "Room_02":
                for (int i = 0; i < enemiesHealth.Length; i++)
                {
                    enemiesDead[i] = false;
                    enemiesHealth[i] = enemiesMaxHealth02[i];
                }
                break;
            case "Room_03":
                for (int i = 0; i < enemiesHealth.Length; i++)
                {
                    enemiesDead[i] = false;
                    enemiesHealth[i] = enemiesMaxHealth03[i];
                }
                break;
            case "Room_05":
                for (int i = 0; i < enemiesHealth.Length; i++)
                {
                    enemiesDead[i] = false;
                    enemiesHealth[i] = enemiesMaxHealth05[i];
                }
                break;
            case "Room_08":
                for (int i = 0; i < enemiesHealth.Length; i++)
                {
                    enemiesDead[i] = false;
                    enemiesHealth[i] = enemiesMaxHealth08[i];
                }
                break;
            case "Room_09":
                for (int i = 0; i < enemiesHealth.Length; i++)
                {
                    enemiesDead[i] = false;
                    enemiesHealth[i] = enemiesMaxHealth09[i];
                }
                break;
        }
    }
}
