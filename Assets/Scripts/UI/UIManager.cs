using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private InputMaster im;

    public GameObject background;

    [Header("Unlocks")]
    public GameObject unlocks;
    public Text unlockTitle;
    public Image unlockImage;
    public Text unlockDescription;
    public Text unlockExit;

    private void Awake()
    {
        im = new InputMaster();

        im.Player.Interact.started += _ => DisplayOrHideUI();
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
                //unlockImage
                unlockDescription.text = Constants.glideDescription;
                break;
            
            case Constants.UnlockableType.Dash:
                unlockTitle.text = Constants.dashTitle;
                //unlockImage
                unlockDescription.text = Constants.dashDescription;
                break;
            
            case Constants.UnlockableType.WallJump:
                unlockTitle.text = Constants.wallJumpTitle;
                //unlockImage
                unlockDescription.text = Constants.wallJumpDescription;
                break;
            
            case Constants.UnlockableType.DoubleJump:
                unlockTitle.text = Constants.doubleJumpTitle;
                //unlockImage
                unlockDescription.text = Constants.doubleJumpDescription;
                break;
        }

        unlockExit.text = Constants.unlockablesExit;
        background.SetActive(true);
        unlocks.SetActive(true);
    }

    private void DisplayOrHideUI()
    {
        if (unlocks.activeSelf)
        {
            background.SetActive(false);
            unlocks.SetActive(false);
        }
    }
}
