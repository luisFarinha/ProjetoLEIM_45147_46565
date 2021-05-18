using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Region Text")]
    public Text region1Text;
    public Text region2Text;
    public Text region3Text;
    public Text region4Text;
    [Header ("Money Text")]
    public Text money1Text;
    public Text money2Text;
    public Text money3Text;
    public Text money4Text;
    [Header("Play Time Text")]
    public Text playT1Text;
    public Text playT2Text;
    public Text playT3Text;
    public Text playT4Text;

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void SetupSaveSlots()
    {
        SaveSystem.path = Application.persistentDataPath + "/slot1.state";
        if(SaveSystem.LoadWorld() != null) {
            WorldData wd1 = SaveSystem.LoadWorld();
            region1Text.text = wd1.lastSavedScene;
            money1Text.text = "Money: " + wd1.money;
            playT1Text.text = "Saved: " + wd1.lastSavedTime;
        }

        SaveSystem.path = Application.persistentDataPath + "/slot2.state";
        if (SaveSystem.LoadWorld() != null)
        {
            WorldData wd2 = SaveSystem.LoadWorld();
            region2Text.text = wd2.lastSavedScene;
            money2Text.text = "Money: " + wd2.money;
            playT2Text.text = "Saved: " + wd2.lastSavedTime;
        }

        SaveSystem.path = Application.persistentDataPath + "/slot3.state";
        if (SaveSystem.LoadWorld() != null)
        {
            WorldData wd3 = SaveSystem.LoadWorld();
            region3Text.text = wd3.lastSavedScene;
            money3Text.text = "Money: " + wd3.money;
            playT3Text.text = "Saved: " + wd3.lastSavedTime;
        }

        SaveSystem.path = Application.persistentDataPath + "/slot4.state";
        if (SaveSystem.LoadWorld() != null)
        {
            WorldData wd4 = SaveSystem.LoadWorld();
            region4Text.text = wd4.lastSavedScene;
            money4Text.text = "Money: " + wd4.money;
            playT4Text.text = "Saved: " + wd4.lastSavedTime;
        }
    }
}
