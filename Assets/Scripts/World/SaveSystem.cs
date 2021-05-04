
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.Experimental.RestService;

public static class SaveSystem
{

    private static string path = Application.persistentDataPath + "/player.state"; //To enable saving on different operating systems (Mac, Windows, ...)
    private static WorldData worldData = new WorldData();

    public static void SaveData(PlayerController player, Enemy[] enemies, string scene)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create); //Enables to read and write from a file
        worldData.InsertData(player, enemies, scene);
        formatter.Serialize(stream, worldData);
        stream.Close();
    }

    public static WorldData LoadWorld()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            WorldData data = formatter.Deserialize(stream) as WorldData; //Read from the stream, Binary to Readable format
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
