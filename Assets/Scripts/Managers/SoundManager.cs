using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip crashSound;

    AudioSource audioSource;
    private void OnEnable()
    {
        EventManager.Instance.onJump += PlayJumpSound;
        EventManager.Instance.onPlayerCrash += PlayCrashSound;
    }

    void Start()
    {
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    public void PlayCrashSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(crashSound);
        }
    }
}
