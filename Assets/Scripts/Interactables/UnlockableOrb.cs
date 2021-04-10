using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableOrb : MonoBehaviour
{
    public string unlockable;
    private UIManager uim;
    private ParticleSystem ps;
    private float psLifeTime;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        uim = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        var main = ps.main;
        main.useUnscaledTime = true;
        psLifeTime = main.startLifetime.constant;
}

private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0;
            ps.Play();

            StartCoroutine(Deactivate(psLifeTime));
        }
    }

    private IEnumerator Deactivate(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        ChooseUnlockable(unlockable);
        gameObject.SetActive(false);
    }

    private void ChooseUnlockable(string unlockable)
    {
        switch (unlockable)
        {
            case "glide": 
                Unlockables.glideUnlocked = true;
                uim.UnlockableFound(Constants.UnlockableType.GLIDE);
            break;
            case "dash": 
                Unlockables.dashUnlocked = true;
                uim.UnlockableFound(Constants.UnlockableType.DASH);
                break;
            case "wallJump": 
                Unlockables.wallJumpUnlocked = true;
                uim.UnlockableFound(Constants.UnlockableType.WALL_JUMP);
                break;
            case "doubleJump": 
                Unlockables.doubleJumpUnlocked = true;
                uim.UnlockableFound(Constants.UnlockableType.DOUBLE_JUMP);
                break;
        }
    }
}
