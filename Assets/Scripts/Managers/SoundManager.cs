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
        EventManager.Instance.onPlayerCrash += PlayCrashSound;
    }

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

    private void OnDisable()
    {
        EventManager.Instance.onPlayerCrash -= PlayCrashSound;
    }
}
