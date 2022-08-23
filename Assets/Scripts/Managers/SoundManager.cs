using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip buttonClickedSound;

    AudioSource audioSource;

    private void OnEnable()
    {
        EventManager.Instance.onJump += PlayJumpSound;
        EventManager.Instance.onPlayerCrash += PlayCrashSound;
    }

    void Start()
    {
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        PlayBackgroundMusic();
    }

    void PlayBackgroundMusic()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
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

    public void PlayClickedButtonSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickedSound);
        }
    }
}
