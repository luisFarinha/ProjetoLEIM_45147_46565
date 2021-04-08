using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private InputMaster im;
    private Animator anim;

    public GameObject background;

    [Header("Unlocks")]
    public GameObject unlocks;
    public Text unlockTitle;
    public Image unlockImage;
    public Text unlockDescription;
    public Text unlockLore;
    public Text unlockExit;

    private void Awake()
    {
        im = new InputMaster();
        anim = GetComponent<Animator>();

        im.Player.Interact.started += _ => HideUI();
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
                unlockImage.sprite = Resources.Load<Sprite>(Constants.GLIDE_SPRITE);
                unlockDescription.text = Constants.GLIDE_DESCRIPTION;
                unlockLore.text = Constants.GLIDE_LORE;
                break;
            
            case Constants.UnlockableType.DASH:
                unlockTitle.text = Constants.DASH_TITLE;
                unlockImage.sprite = Resources.Load<Sprite>(Constants.DASH_SPRITE);
                unlockDescription.text = Constants.DASH_DESCRIPTION;
                unlockLore.text = Constants.DASH_LORE;
                break;
            
            case Constants.UnlockableType.WALL_JUMP:
                unlockTitle.text = Constants.WALL_JUMP_TITLE;
                unlockImage.sprite = Resources.Load<Sprite>(Constants.WALL_JUMP_SPRITE);
                unlockDescription.text = Constants.WALL_JUMP_DESCRIPTION;
                unlockLore.text = Constants.WALL_JUMP_LORE;
                break;
            
            case Constants.UnlockableType.DOUBLE_JUMP:
                unlockTitle.text = Constants.DOUBLE_JUMP_TITLE;
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

    private void Pause()
    {
        background.SetActive(true);
        Time.timeScale = 0;
    }

    private void HideUI()
    {
        if (unlocks.activeSelf && anim.GetCurrentAnimatorStateInfo(0).IsName(Constants.UNLOCK_IDLE))
        {
            anim.Play(Constants.UNLOCK_FADE_OUT);
            StartCoroutine(Resume(anim.speed));
        }
    }

    private IEnumerator Resume(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        background.SetActive(false);
        unlocks.SetActive(false);
        Time.timeScale = 1f;
    }
}
