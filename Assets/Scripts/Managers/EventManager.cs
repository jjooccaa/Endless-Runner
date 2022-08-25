using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public UnityAction onPlayerCrash;
    public UnityAction onGameOver;
    public UnityAction onJump;
    public UnityAction<GameObject, int> onPowerUpPickUp;
    public UnityAction onIncreasedDifficulty;
    public UnityAction<float> onMovementSpeedChange;
    public UnityAction onSpawnTrigger;
    public UnityAction onRemoveTrigger;
    public UnityAction<Vector3> onSpawnShootingArrow;
    public UnityAction<GameObject> onShoot;
    public UnityAction<GameObject> onArrowHit;
    public UnityAction onArrowPickUp;
    public UnityAction onArrowShoot;
    public UnityAction onCoinPickUp;
    public UnityAction onTutorialTrigger;
    public UnityAction<string, string> onRegister;
    public UnityAction<string, string> onLogin;
    public UnityAction<string> onResetPassword;
    public UnityAction onLoginSuccess;
    public UnityAction<string> onLoginInfoChange;
    public UnityAction<int, string, string> onLeaderboardGet;
}
