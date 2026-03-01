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
        InventoryManager.Instance.AddItem(chestItem, 1);

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
}
