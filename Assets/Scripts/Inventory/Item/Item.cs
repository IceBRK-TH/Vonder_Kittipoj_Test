using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")] 
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public int num;
    public bool isStackable;
}

