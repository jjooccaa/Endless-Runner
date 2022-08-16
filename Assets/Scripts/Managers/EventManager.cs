using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public UnityAction onPlayerCrash;
    public UnityAction onGameOver;
    public UnityAction<GameObject, int> onPowerUpPickUp;
}
