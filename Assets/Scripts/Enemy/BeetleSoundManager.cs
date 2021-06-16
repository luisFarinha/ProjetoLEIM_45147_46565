using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleSoundManager : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] footSteps;
    public AudioClip[] takeDamage;
    public AudioClip die;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void AudioBeetleFootStep()
    {
        source.clip = footSteps[Mathf.FloorToInt(UnityEngine.Random.value * footSteps.Length)];
        source.Play();
    }

    public void AudioBeetleTakeDamage()
    {
        source.clip = takeDamage[Mathf.FloorToInt(UnityEngine.Random.value * takeDamage.Length)];
        source.Play();
    }

    public void AudioBeetleDie()
    {
        source.clip = die;
        source.Play();
    }
 }
