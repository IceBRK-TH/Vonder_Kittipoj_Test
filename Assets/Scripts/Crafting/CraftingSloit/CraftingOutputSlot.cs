using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingOutputSlot : InventorySlot
{
    // Block players from dropping items INTO the output
    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Cannot drop items into the output slot!");
    }

    // When the player clicks/drags the result, move it to the real inventory
    public void OnClickResult()
    {
        InventorySlotData outputData = CraftingManager.Instance.GetCurrentResult();

        if (outputData.item != null)
        {
            // Use your existing AddItem logic to handle stacking
            InventoryManager.Instance.AddItem(outputData.item, outputData.amount);

            // Clear the crafting table
            CraftingManager.Instance.ClearCraftingInput();
        }
    }
}