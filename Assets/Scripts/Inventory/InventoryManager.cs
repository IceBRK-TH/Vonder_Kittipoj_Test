using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlotData
{
    public Item item;
    public int amount;

    public InventorySlotData(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Settings")]
    public int maxInventorySlots = 46; 

    public List<InventorySlotData> inventory = new List<InventorySlotData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < maxInventorySlots; i++)
        {
            inventory.Add(new InventorySlotData(null, 0));
        }
    }
    public void AddItem(Item itemToAdd, int amountToAdd)
    {
        if (itemToAdd == null) return;

        int hotbarSize = 6; // The number of slots reserved for Quick Access

        // 1. Create a custom search order: Backpack first (6-45), Hotbar last (0-5)
        List<int> searchOrder = new List<int>();
        for (int i = hotbarSize; i < maxInventorySlots; i++) searchOrder.Add(i);
        for (int i = 0; i < hotbarSize; i++) searchOrder.Add(i);

        // PASS 1: Try to add to EXISTING stacks following our custom order
        foreach (int index in searchOrder)
        {
            var slot = inventory[index];
            if (slot.item == itemToAdd && slot.amount < slot.item.MaxStack)
            {
                int spaceLeft = slot.item.MaxStack - slot.amount;
                int amountCanAdd = Mathf.Min(amountToAdd, spaceLeft);

                slot.amount += amountCanAdd;
                amountToAdd -= amountCanAdd;

                if (amountToAdd <= 0)
                {
                    RefreshUI();
                    return;
                }
            }
        }

        // PASS 2: Add to EMPTY slots following our custom order
        foreach (int index in searchOrder)
        {
            var slot = inventory[index];
            if (slot.item == null) // Found a blank slot!
            {
                slot.item = itemToAdd;

                int amountCanAdd = Mathf.Min(amountToAdd, itemToAdd.MaxStack);
                slot.amount = amountCanAdd;
                amountToAdd -= amountCanAdd;

                if (amountToAdd <= 0)
                {
                    RefreshUI();
                    return;
                }
            }
        }

        Debug.LogWarning("Inventory Full! No space in Backpack or Hotbar.");
        RefreshUI();
    }

    public void SwapItems(int indexA, int indexB)
    {
        // Simple data swap
        InventorySlotData temp = inventory[indexA];
        inventory[indexA] = inventory[indexB];
        inventory[indexB] = temp;

        RefreshUI(); // Update the screen to show the new positions
    }
    private void RefreshUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.RefreshInventoryUI(inventory);
        }
    }
    public void RemoveItem(int slotIndex)
    {
        // 1. Check if index is valid for our padded list
        if (slotIndex < 0 || slotIndex >= inventory.Count)
        {
            Debug.LogError($"InventoryManager: Invalid remove index {slotIndex}");
            return;
        }

        // 2. Clear the data in that slot (make it empty)
        // Setting item to null and amount to 0 effectively removes it from our padded list data structure
        inventory[slotIndex].item = null;
        inventory[slotIndex].amount = 0;

        Debug.Log($"InventoryManager: Item removed from slot {slotIndex}");

        // 3. Update the UI to show the empty slot
        RefreshUI(); // Or UIManager.Instance.RefreshInventoryUI(inventory);
    }
}