using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingOutputSlot : InventorySlot
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        // 1. Check if there is actually a crafted item to drag
        if (this.item == null) return;

        // 2. Call the parent drag logic so the icon follows the mouse
        base.OnBeginDrag(eventData);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        // Keep this empty so players can't drop things INTO the result
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