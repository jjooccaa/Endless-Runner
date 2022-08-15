using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : Singleton<EventManager>
{
    public Action onPlayerCrash;

    public Action onGameOver;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerCrashAction();

        SetGameOverAction();
    }

    void SetPlayerCrashAction()
    {
        onPlayerCrash += SoundManager.Instance.PlayCrashSound;
        onPlayerCrash += GameManager.Instance.Death;
    }

    void SetGameOverAction()
    {
        onGameOver += GameObject.Find("Player").GetComponent<PlayerController>().DisableMovement;
        onGameOver += GameManager.Instance.GameOver;
    }
}
