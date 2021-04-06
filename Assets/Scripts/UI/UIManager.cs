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
            case Constants.UnlockableType.Glide:
                unlockTitle.text = Constants.glideTitle;
                unlockImage.sprite = Resources.Load<Sprite>(Constants.glideSprite);
                unlockDescription.text = Constants.glideDescription;
                unlockLore.text = Constants.glideLore;
                break;
            
            case Constants.UnlockableType.Dash:
                unlockTitle.text = Constants.dashTitle;
                unlockImage.sprite = Resources.Load<Sprite>(Constants.dashSprite);
                unlockDescription.text = Constants.dashDescription;
                unlockLore.text = Constants.dashLore;
                break;
            
            case Constants.UnlockableType.WallJump:
                unlockTitle.text = Constants.wallJumpTitle;
                unlockImage.sprite = Resources.Load<Sprite>(Constants.wallJumpSprite);
                unlockDescription.text = Constants.wallJumpDescription;
                unlockLore.text = Constants.wallJumpLore;
                break;
            
            case Constants.UnlockableType.DoubleJump:
                unlockTitle.text = Constants.doubleJumpTitle;
                unlockImage.sprite = Resources.Load<Sprite>(Constants.doubleJumpSprite);
                unlockDescription.text = Constants.doubleJumpDescription;
                unlockLore.text = Constants.doubleJumpLore;
                break;
        }
        
        Pause();
        unlockExit.text = Constants.unlockablesExit;
        unlocks.SetActive(true);
        anim.Play(Constants.unlockFadeIn);

    }

    private void Pause()
    {
        background.SetActive(true);
        Time.timeScale = 0;
    }

    private void HideUI()
    {
        if (unlocks.activeSelf && anim.GetCurrentAnimatorStateInfo(0).IsName(Constants.unlockIdle))
        {
            anim.Play(Constants.unlockFadeOut);
            StartCoroutine(Resume(anim.speed));
        }
    }

    private IEnumerator Resume(float time)
    {
        Debug.Log(anim.speed);
        yield return new WaitForSecondsRealtime(time);
        Debug.Log(time);
        background.SetActive(false);
        unlocks.SetActive(false);
        Time.timeScale = 1f;
    }
}
