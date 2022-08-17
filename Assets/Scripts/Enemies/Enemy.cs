using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public new string name;
    public float scale;
    public float speed;
    public bool canMoveSideways;
    public float sidewaysSpeed;
}