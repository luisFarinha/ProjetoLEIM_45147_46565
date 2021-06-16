using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    private AudioSource source;
    private Animator anim;
    private PlayerController pc;

    public AudioClip[] footsteps;
    
    public AudioClip jump;
    public bool playJump = true;

    public AudioClip doubleJump;

    public AudioClip fall;

    public AudioClip land;

    public AudioClip dash;

    public AudioClip glide;
    public bool playGlide = true;

    public AudioClip[] wallSlides;
    public bool canAudioSlide = true;

    public AudioClip[] attacks;

    public AudioClip takeDamage;

    public AudioClip die;
    public bool canPlayDeathAudio;

    public AudioClip unlockablePickUp;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();
    }

    private void Update()
    {
        try
        {
            if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Constants.PLAYER_GLIDE))
            {
                playGlide = true;
            }
        }
        catch (Exception) { }
        try
        {
            if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Constants.PLAYER_WALLSLIDE))
            {
                if (source.clip.name.Equals("dirt_slide_01") || source.clip.name.Equals("dirt_slide_02"))
                {
                    source.Stop();
                }
                canAudioSlide = true;
            }
            if (pc.isDead && canPlayDeathAudio)
            {
                AudioDie();
                canPlayDeathAudio = false;
            }
        }catch (Exception) { }
    }

    public void AudioFootStep()
    {
        source.clip = footsteps[Mathf.FloorToInt(UnityEngine.Random.value * footsteps.Length)];
        source.Play();
    }

    public void AudioJump()
    {
        if (playJump)
        {
            playJump = false;
            source.clip = jump;
            source.Play();
        }
    }

    public void AudioDoubleJump()
    {
        source.clip = doubleJump;
        source.Play();
    }

    public void AudioFall()
    {
        source.clip = fall;
        source.Play();
    }

    public void AudioLand()
    {
        source.clip = land;
        source.Play();
        playJump = true;
    }

    public void AudioDash()
    {
        source.clip = dash;
        source.Play();
    }

    public void AudioGlide()
    {
        if (playGlide)
        {
            source.clip = glide;
            source.Play();
            playGlide = false;
            Debug.Log(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }
    }

    public void AudioWallSlide()
    {
        if (canAudioSlide)
        {
            source.clip = wallSlides[Mathf.FloorToInt(UnityEngine.Random.value * wallSlides.Length)]; ;
            source.Play();
            canAudioSlide = false;
        }
    }

    public void AudioAttack()
    {
        source.clip = attacks[0];
        source.Play();
    }

    public void AudioTakeDamage()
    {
        source.clip = takeDamage;
        source.Play();
        canPlayDeathAudio = true;
    }

    public void AudioDie()
    {
        source.clip = die;
        source.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Unlock"))
        {
            source.clip = unlockablePickUp;
            source.Play();
        }
    }
}
