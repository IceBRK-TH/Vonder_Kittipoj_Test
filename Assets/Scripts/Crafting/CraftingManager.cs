using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    [Header("UI Slots")]
    public CraftingInputSlot[] inputSlots; 
    public CraftingOutputSlot outputSlot;

    [Header("Recipes")]
    public Item logItem; 
    public Item chestItem;

    private Item currentResultItem;
    private int currentResultAmount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public InventorySlotData GetCurrentResult()
    {
        // Return the current recipe result as a data package
        return new InventorySlotData(currentResultItem, currentResultAmount);
    }
    public void OnCraftComplete()
    {
        int[] borderIndices = { 0, 1, 2, 3, 4, 7, 8, 9, 10, 11 };
        foreach (int i in borderIndices)
        {
            inputSlots[i].amount--;

            if (inputSlots[i].amount <= 0)
            {
                inputSlots[i].ClearSlot();
            }
            else
            {
                inputSlots[i].UpdateSlot(inputSlots[i].item, inputSlots[i].amount);
            }
        }

       // 1. Wipe the "Result" data so the Output Slot becomes empty
    currentResultItem = null;
    currentResultAmount = 0;
    
    // 2. Tell the Output UI to clear its image
    if (outputSlot != null) outputSlot.ClearSlot();

    // 3. Sync the 46-slot inventory UI
    InventoryManager.Instance.RefreshUI(); 

    // 4. Re-check the pattern
    CheckRecipe();
    }

    public void ClearCraftingInput()
    {
        foreach (var slot in inputSlots)
        {
            slot.ClearSlot(); // Clears variables and UI
        }
    }
    public void CheckRecipe()
    {

        bool isPatternCorrect = true;

        // 1. Check the Border (Slots 0,1,2,3, 4,7, 8,9,10,11)
        int[] borderIndices = { 0, 1, 2, 3, 4, 7, 8, 9, 10, 11 };
        foreach (int i in borderIndices)
        {
            if (inputSlots[i].item != logItem || inputSlots[i].amount < 1)
            {
                isPatternCorrect = false;
                break;
            }
        }

        // 2. Check the Middle 2 (Slots 5 and 6) must be empty
        if (inputSlots[5].item != null || inputSlots[6].item != null)
        {
            isPatternCorrect = false;
        }

        if (isPatternCorrect)
        {
            currentResultItem = chestItem;
            currentResultAmount = 1;
            outputSlot.UpdateSlot(currentResultItem, currentResultAmount);
        }
        else
        {
            currentResultItem = null;
            currentResultAmount = 0;
            outputSlot.ClearSlot();
        }
    }
    public void SetInputItem(int slotIndex, Item newItem, int newAmount)
    {
        if (slotIndex >= 0 && slotIndex < inputSlots.Length)
        {
            // If there was already an item here, you might want to return it to the bag
            if (inputSlots[slotIndex].item != null)
            {
                InventoryManager.Instance.AddItem(inputSlots[slotIndex].item, inputSlots[slotIndex].amount);
            }

            // Set the new item
            inputSlots[slotIndex].UpdateSlot(newItem, newAmount);
        }
    }

    // This allows the CraftingInputSlot to refresh its own UI
    public InventorySlotData GetInputSlotData(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inputSlots.Length)
        {
            return new InventorySlotData(inputSlots[slotIndex].item, inputSlots[slotIndex].amount);
        }
        return new InventorySlotData(null, 0);
    }

    public bool AddOneToInput(int slotIndex, Item newItem)
    {
        if (slotIndex < 0 || slotIndex >= inputSlots.Length) return false;

        CraftingInputSlot targetSlot = inputSlots[slotIndex];

        // Case A: The slot is empty
        if (targetSlot.item == null)
        {
            targetSlot.UpdateSlot(newItem, 1);
            return true;
        }
        // Case B: The slot already has the SAME item (increment stack)
        else if (targetSlot.item == newItem && newItem.IsStackable)
        {
            targetSlot.UpdateSlot(newItem, targetSlot.amount + 1);
            return true;
        }

        // Return false if trying to place a different item in a filled slot
        return false;
    }
}
