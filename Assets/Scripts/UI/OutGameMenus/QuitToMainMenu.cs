using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMainMenu : MonoBehaviour
{
    public GameObject charObjects;

    public void QuitToMain()
    {
        Destroy(charObjects);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
