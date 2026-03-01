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
    public int maxInventorySlots = 46; // Check this in the Inspector!

    public List<InventorySlotData> inventory = new List<InventorySlotData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(Item itemToAdd, int amountToAdd)
    {
        if (itemToAdd == null) return;

        Debug.Log($"--- START ADDING: {amountToAdd}x {itemToAdd.itemName} ---");

        // PASS 1: Try to add to existing stacks first
        foreach (var slot in inventory)
        {
            if (slot.item == itemToAdd && slot.amount < slot.item.MaxStack)
            {
                int spaceLeft = slot.item.MaxStack - slot.amount;

                if (amountToAdd <= spaceLeft)
                {
                    slot.amount += amountToAdd;
                    Debug.Log($"Filled existing stack completely. Added {amountToAdd}.");
                    RefreshUI();
                    return;
                }
                else
                {
                    slot.amount += spaceLeft;
                    amountToAdd -= spaceLeft;
                    Debug.Log($"Maxed out an existing stack. Leftovers to place: {amountToAdd}");
                }
            }
        }

        Debug.Log($"Pass 1 Complete. Leftovers needed for new slots: {amountToAdd}");

        // PASS 2: Put leftovers into NEW empty slots
        while (amountToAdd > 0)
        {
            // Safety Check: Have we hit the max UI slots limit?
            if (inventory.Count >= maxInventorySlots)
            {
                Debug.LogWarning($"CRITICAL: Inventory Full! Dropping {amountToAdd} {itemToAdd.itemName}. Check 'maxInventorySlots' in Inspector.");
                break;
            }

            int amountForNewSlot = Mathf.Min(amountToAdd, itemToAdd.MaxStack);

            inventory.Add(new InventorySlotData(itemToAdd, amountForNewSlot));
            amountToAdd -= amountForNewSlot;

            Debug.Log($"Created new slot with {amountForNewSlot}x {itemToAdd.itemName}. Remaining leftovers: {amountToAdd}");
        }

        RefreshUI();
        Debug.Log($"--- FINISHED ADDING ---");
    }

    private void RefreshUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.RefreshInventoryUI(inventory);
        }
    }
}