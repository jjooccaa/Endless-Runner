using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] public string iD;
    [SerializeField] public string newName;
    [SerializeField] public int cost;

    public const string HEALTH_POTION_NAME = "Health_Potion";
    public const string BACKPACK_NAME = "Backpack";

    public void AssignItemInfo(int itemCost)
    {
        cost = itemCost;
    }

}
