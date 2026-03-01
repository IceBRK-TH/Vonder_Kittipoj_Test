using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Resources,
    Tool,
    CraftingItem,
    Seed
}

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")] 
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public ItemType type;

    public bool IsStackable => (type == ItemType.Resources || type == ItemType.Seed);
    public int MaxStack => IsStackable ? 10 : 1;
}

