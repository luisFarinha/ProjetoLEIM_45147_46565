using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private InputMaster im;
    public Animator anim;

    public GameObject background;

    [Header("Unlocks")]
    public GameObject unlocks;
    public Text unlockTitle;
    public Text unlockKeybind;
    public Image unlockImage;
    public Text unlockDescription;
    public Text unlockLore;
    public Text unlockExit;

    [Header("PlayerUI")]
    public Slider slider;
    public Slider followSlider;
    public Text moneyText;

    [Header("Inventory")]
    public GameObject inventory;
    public Image inventoryMenuImage;

    [Header("Map")]
    public GameObject map;

    [Header("InGameMenus")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject soundMenu;
    public GameObject videoMenu;
    public GameObject controlsMenu;

    public static bool isPaused;

    private void Awake()
    {
        im = new InputMaster();
        anim = GetComponent<Animator>();

        moneyText = GameObject.Find(Constants.MONEY_TEXT).GetComponent<Text>();

        im.Player.Interact.started += _ => HideUI();
        im.Player.Inventory.started += _ => Inventory();
        im.Player.Menu.started += _ => MainMenu();

        isPaused = false;
    }

    private void OnEnable()
    {
        im.Enable();
    }

    private void OnDisable()
    {
        im.Disable();
    }

    public void UnlockableFound(Constants.UnlockableType unlockable)
    {
        switch (unlockable)
        {
            case Constants.UnlockableType.GLIDE:
                unlockTitle.text = Constants.GLIDE_TITLE;
                unlockKeybind.text = Constants.GLIDE_KEYBIND;
                unlockImage.sprite = Resources.Load<Sprite>(Constants.GLIDE_SPRITE);
                unlockDescription.text = Constants.GLIDE_DESCRIPTION;
                unlockLore.text = Constants.GLIDE_LORE;
                break;
            
            case Constants.UnlockableType.DASH:
                unlockTitle.text = Constants.DASH_TITLE;
                unlockKeybind.text = Constants.DASH_KEYBIND;
                unlockImage.sprite = Resources.Load<Sprite>(Constants.DASH_SPRITE);
                unlockDescription.text = Constants.DASH_DESCRIPTION;
                unlockLore.text = Constants.DASH_LORE;
                break;
            
            case Constants.UnlockableType.WALL_JUMP:
                unlockTitle.text = Constants.WALL_JUMP_TITLE;
                unlockKeybind.text = Constants.WALL_JUMP_KEYBIND;
                unlockImage.sprite = Resources.Load<Sprite>(Constants.WALL_JUMP_SPRITE);
                unlockDescription.text = Constants.WALL_JUMP_DESCRIPTION;
                unlockLore.text = Constants.WALL_JUMP_LORE;
                break;
            
            case Constants.UnlockableType.DOUBLE_JUMP:
                unlockTitle.text = Constants.DOUBLE_JUMP_TITLE;
                unlockKeybind.text = Constants.DOUBLE_JUMP_KEYBIND;
                unlockImage.sprite = Resources.Load<Sprite>(Constants.DOUBLE_JUMP_SPRITE);
                unlockDescription.text = Constants.DOUBLE_JUMP_DESCRIPTION;
                unlockLore.text = Constants.DOUBLE_JUMP_LORE;
                break;
        }
        
        Pause();
        unlockExit.text = Constants.UNLOCKABLES_EXIT;
        unlocks.SetActive(true);
        anim.Play(Constants.UNLOCK_FADE_IN);

    }

    public void SceneTransitionFadeIn()
    {
        Pause();
        anim.Play(Constants.SCENE_TRANSITION_FADE_IN);
        StartCoroutine(ResumeTime(anim.speed));
    }

    public void SceneTransitionFadeOut()
    {
        anim.Play(Constants.SCENE_TRANSITION_FADE_OUT);
        StartCoroutine(Resume(anim.speed));
    }

    private void HideUI()
    {
        if (unlocks.activeSelf && anim.GetCurrentAnimatorStateInfo(0).IsName(Constants.UNLOCK_IDLE))
        {
            anim.Play(Constants.UNLOCK_FADE_OUT);
            StartCoroutine(Resume(anim.speed));
        }
    }

    private void Inventory()
    {
        if (!isPaused)
        {
            inventoryMenuImage.sprite = Resources.Load<Sprite>(Constants.INVENTORY_SPRITE);
            inventory.SetActive(true);
            Pause();
        } else
        {
            StartCoroutine(Resume(0f));
        }
        
    }

    private void MainMenu()
    {
        if (!isPaused)
        {
            inventory.SetActive(false);
            map.SetActive(false);

            mainMenu.SetActive(true);
            Pause();
        } else
        {
            if (mainMenu.activeSelf)
            {
                StartCoroutine(Resume(0f));
            }
            else
            {
                mainMenu.SetActive(true);

                inventory.SetActive(false);
                optionsMenu.SetActive(false);
                soundMenu.SetActive(false);
                videoMenu.SetActive(false);
                controlsMenu.SetActive(false);
            }
        }
    }

    private void Pause()
    {
        background.SetActive(true);
        
        isPaused = true;
        
        Time.timeScale = 0;
    }

    private IEnumerator Resume(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        background.SetActive(false);
        unlocks.SetActive(false);
        inventory.SetActive(false);
        mainMenu.SetActive(false);
        
        isPaused = false;
        
        Time.timeScale = 1f;
    }

    private IEnumerator ResumeTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }

    public void ContinueGame() // função usada pelo continue button
    {
        StartCoroutine(Resume(0f));
    }
}
