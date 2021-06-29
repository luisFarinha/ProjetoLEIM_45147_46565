
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string path = Application.persistentDataPath + "/world.state"; //To enable saving on different operating systems (Mac, Windows, ...)
    private static WorldData worldData;

    public static void SaveData(PlayerController player, Enemy[] enemies, Chest[] chests, UnlockableOrb[] unlockOrbs, string scene)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Create); //Enables to read and write from a file
        worldData.InsertData(player, enemies, chests, unlockOrbs, scene);
        formatter.Serialize(file, worldData);
        file.Close();
    }

    public static void SaveCheckPoint(Vector3 checkPoint, string scene)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Create); //Enables to read and write from a file
        worldData.InsertCheckpointData(checkPoint, scene);
        formatter.Serialize(file, worldData);
        file.Close();
    }

    public static void SaveDataOnPlayerDeath(PlayerController player, string scene)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Create); //Enables to read and write from a file
        worldData.InsertDataOnPlayerDeath(player, scene);
        formatter.Serialize(file, worldData);
        file.Close();
    }

    public static void ResetSoul() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Create); //Enables to read and write from a file
        worldData.ResetSoulData();
        formatter.Serialize(file, worldData);
        file.Close();
    }

    public static WorldData LoadWorld()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);

            worldData = formatter.Deserialize(file) as WorldData; //Read from the stream, Binary to Readable format

            file.Close();

            return worldData;
        }
        else
        {
            worldData = new WorldData(); //Create new data object
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
