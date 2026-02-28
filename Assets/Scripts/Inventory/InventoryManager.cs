using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // The Dictionary: Tracks how many of each ScriptableObject you have
    private Dictionary<Item, int> inventory = new Dictionary<Item, int>();

    // Add an item to the inventory
    public void AddItem(Item item, int amount)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += amount;
        }
        else
        {
            inventory.Add(item, amount);
        }
        Debug.Log($"Added {amount} of {item.itemName}. Total: {inventory[item]}");
    }

    // Check if you have enough for crafting
    public bool HasRequiredItem(Item item, int requiredAmount)
    {
        if (inventory.ContainsKey(item))
        {
            return inventory[item] >= requiredAmount;
        }
        return false;
    }

    // Remove items after crafting
    public void RemoveItem(Item item, int amount)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= amount;
            if (inventory[item] <= 0) inventory.Remove(item);
        }
    }
}
