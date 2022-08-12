using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip crashSound;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayCrashSound()
    {
        audioSource.PlayOneShot(crashSound);
    }
}
