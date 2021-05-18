using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameMenu : MonoBehaviour
{

    public void StartGame(string slot)
    {
        SaveSystem.path = Application.persistentDataPath + "/" + slot + ".state";
        if (SaveSystem.LoadWorld() != null)
        {
            SceneManager.LoadScene(SaveSystem.LoadWorld().lastSavedScene);
        }
        else
        {
            SceneManager.LoadScene("Room_00");
        }
    }

    public void DeleteSaveFile(string slot)
    {
        File.Delete(Application.persistentDataPath + "/" + slot + ".state");

    }
}
