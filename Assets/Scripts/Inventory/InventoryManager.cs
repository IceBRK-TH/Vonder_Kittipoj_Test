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
    public Transform playerTransform;
    public List<InventorySlotData> inventory = new List<InventorySlotData>();

    [Header("Equipment State")]
    public Item equippedWeapon;
    public Item equippedArmor;
    public Item equippedLeggings;

    [Header("Equipment UI Reference")]
    public EquipSlot weaponSlotUI;
    public EquipSlot armorSlotUI;
    public EquipSlot armorSlotUI2;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < maxInventorySlots; i++)
        {
            inventory.Add(new InventorySlotData(null, 0));
        }
    }
    void Update()
    {
        // Detect keys 1 through 6
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseItem(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) UseItem(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) UseItem(5);
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
        // Check if both indices are within the valid 0-45 range
        if (indexA < 0 || indexA >= inventory.Count || indexB < 0 || indexB >= inventory.Count)
        {
            Debug.LogWarning("Swap aborted: One of the indices is outside the 46-slot inventory range.");
            return;
        }

        // Normal swap logic continues here...
        InventorySlotData temp = inventory[indexA];
        inventory[indexA] = inventory[indexB];
        inventory[indexB] = temp;

        RefreshUI();
    }
    public void RefreshUI()
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

    public void UseItem(int slotIndex)
    {
        InventorySlotData slot = inventory[slotIndex];
        if (slot.item == null) return;

        Item item = slot.item;

        switch (item.type)
        {
            case ItemType.Equipment:
            case ItemType.Tool:
                EquipItem(slotIndex,0);
                break;

            case ItemType.Placeable:
                PlaceItemAtPlayer(slotIndex);
                break;

            case ItemType.Consumable:
                UseConsumable(slotIndex);
                break;
        }
    }

    public void UseConsumable(int slotIndex)
    {
        InventorySlotData slot = inventory[slotIndex];
        if (slot.item == null || slot.item.type != ItemType.Consumable) return;

        // Apply Effect (e.g., PlayerHealth.Heal(slot.item.powerValue))
        Debug.Log($"Used {slot.item.itemName}. ");

        slot.amount--;
        if (slot.amount <= 0) slot.item = null;

        RefreshUI();
    }
    public void EquipItem(int invIndex, int equipType)
    {
        Item itemToEquip = inventory[invIndex].item;
        if (itemToEquip == null) return;
        if (equipType == 0) // Weapon
        {
            // 2. The Swap: Store the current weapon so we don't lose it
            Item oldWeapon = equippedWeapon;
            equippedWeapon = itemToEquip;

            // 3. Put the old weapon (or null) back into the backpack
            inventory[invIndex].item = oldWeapon;
            inventory[invIndex].amount = (oldWeapon != null) ? 1 : 0;

            // 4. Force the UI to update
            if (weaponSlotUI != null) weaponSlotUI.RefreshSlotUI();
        }
        else if (equipType == 1) // Armor
        {
            Item oldArmor = equippedArmor;
            equippedArmor = itemToEquip;
            inventory[invIndex].item = oldArmor;
            inventory[invIndex].amount = (oldArmor != null) ? 1 : 0;

            if (armorSlotUI != null) armorSlotUI.RefreshSlotUI();
        }
        else if (equipType == 2) // Leggings
        {
            Item oldLeggings = equippedLeggings;
            equippedLeggings = itemToEquip;
            inventory[invIndex].item = oldLeggings;
            inventory[invIndex].amount = (oldLeggings != null) ? 1 : 0;

            if (armorSlotUI2 != null) armorSlotUI2.RefreshSlotUI();
        }
       
        RefreshUI(); // Update the whole screen
    }

    private void PlaceItemAtPlayer(int slotIndex)
    {
        Item item = inventory[slotIndex].item;

        if (item.prefabToPlace != null)
        {
            // Instantiate the 'Visual Only' sprite at the player's current position
            Instantiate(item.prefabToPlace, playerTransform.position, Quaternion.identity);

            // Reduce the stack count in the backpack
            inventory[slotIndex].amount--;

            // If the stack hits zero, clear the item data
            if (inventory[slotIndex].amount <= 0)
            {
                inventory[slotIndex].item = null;
            }

            // Update the 46-slot UI (Hotbar and Backpack)
            RefreshUI();
        }
    }

    public void MoveOneToCrafting(int invIndex, int craftingSlotIndex)
    {
        // 1. Reference the item in your 40-slot backpack
        Item itemToMove = inventory[invIndex].item;

        if (itemToMove == null || inventory[invIndex].amount <= 0) return;

        // 2. Add only ONE to the CraftingManager
        // We create a new method in CraftingManager called 'AddOneToInput'
        bool success = CraftingManager.Instance.AddOneToInput(craftingSlotIndex, itemToMove);

        if (success)
        {
            // 3. Subtract only ONE from your backpack
            inventory[invIndex].amount--;

            // 4. If the stack is now empty, clear the item data
            if (inventory[invIndex].amount <= 0)
            {
                inventory[invIndex].item = null;
            }

            // 5. Refresh the 46-slot main UI
            RefreshUI();
        }
    }
}