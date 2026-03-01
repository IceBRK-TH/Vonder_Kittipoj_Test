using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Resources,
    Tool,
    Consumable,
    Weapon,
    Equipment,
    Placeable,
    Seed
}

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")] 
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public GameObject prefabToPlace;

    public bool IsStackable => (type == ItemType.Resources || type == ItemType.Seed || type == ItemType.Consumable);
    public int MaxStack => IsStackable ? 10 : 1;
}

