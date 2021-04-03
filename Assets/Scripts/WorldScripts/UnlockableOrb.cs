using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableOrb : MonoBehaviour
{
    public string unlockable;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            chooseUnlockable(unlockable);
            gameObject.SetActive(false);
        }
    }

    private void chooseUnlockable(string unlockable)
    {
        switch (unlockable)
        {
            case "glide": Unlockables.glideUnlocked = true; break;
            case "dash": Unlockables.dashUnlocked = true; break;
            case "wallJump": Unlockables.wallJumpUnlocked = true; break;
            case "doubleJump": Unlockables.doubleJumpUnlocked = true; break;
        }
    }
}
