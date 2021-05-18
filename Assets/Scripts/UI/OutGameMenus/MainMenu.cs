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
        SetupSaveSlot("slot1", region1Text, money1Text, playT1Text);
        SetupSaveSlot("slot2", region2Text, money2Text, playT2Text);
        SetupSaveSlot("slot3", region3Text, money3Text, playT3Text);
        SetupSaveSlot("slot4", region4Text, money4Text, playT4Text);
    }

    private void SetupSaveSlot(string slot, Text regionText, Text moneyText, Text playTText)
    {
        SaveSystem.path = Application.persistentDataPath + "/" + slot + ".state";
        if (SaveSystem.LoadWorld() != null)
        {
            WorldData wd = SaveSystem.LoadWorld();
            regionText.text = wd.lastSavedScene;
            moneyText.text = "Money: " + wd.money;
            playTText.text = "Saved: " + wd.lastSavedTime;
        }
        else
        {
            regionText.text = "New Game";
            moneyText.text = "";
            playTText.text = "";
        }
    }
}
