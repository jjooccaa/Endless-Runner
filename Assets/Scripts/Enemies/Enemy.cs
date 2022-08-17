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

    public static Enemy GetRandomEnemy()
    {
        List<Enemy> allEnemies = new List<Enemy>(Resources.LoadAll<Enemy>("Enemies"));

        Enemy randomEnemy = allEnemies[Random.Range(0, allEnemies.Count)];

        return randomEnemy;
    }
}
