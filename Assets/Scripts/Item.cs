using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public string iD;
    public new string name;
    public int cost;
    public string description;

    public const string HEALTH_POTION_NAME = "Health Potion";
    public const string BACKPACK_NAME = "Backpack";

    public void AssignItemInfo(int itemCost, string itemDescription)
    {
        cost = itemCost;
        description = itemDescription;
    }
}
