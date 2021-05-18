using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameMenu : MonoBehaviour
{

    public void StartGame(string slot)
    {
        SaveSystem.path = Application.persistentDataPath + "/" + slot + ".state";
        SceneManager.LoadScene(SaveSystem.LoadWorld().lastSavedScene);
    }
}
