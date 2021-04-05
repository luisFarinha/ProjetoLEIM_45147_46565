using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableOrb : MonoBehaviour
{
    public string unlockable;
    public UIManager uim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ChooseUnlockable(unlockable);
            gameObject.SetActive(false);
        }
    }

    private void ChooseUnlockable(string unlockable)
    {
        switch (unlockable)
        {
            case "glide": 
                Unlockables.glideUnlocked = true;
                uim.UnlockableFound(Constants.UnlockableType.Glide);
            break;
            case "dash": 
                Unlockables.dashUnlocked = true;
                uim.UnlockableFound(Constants.UnlockableType.Dash);
                break;
            case "wallJump": 
                Unlockables.wallJumpUnlocked = true;
                uim.UnlockableFound(Constants.UnlockableType.WallJump);
                break;
            case "doubleJump": 
                Unlockables.doubleJumpUnlocked = true;
                uim.UnlockableFound(Constants.UnlockableType.DoubleJump);
                break;
        }
    }
}
