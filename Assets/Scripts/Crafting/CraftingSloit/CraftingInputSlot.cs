using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingInputSlot : InventorySlot // Inherit to keep the UI visuals
{
    public override void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot = eventData.pointerDrag.GetComponent<InventorySlot>();

        if (draggedSlot != null)
        {
            // Move only one log per drag
            InventoryManager.Instance.MoveOneToCrafting(draggedSlot.slotIndex, this.slotIndex);

            // Check if the 10-log border pattern is complete
            CraftingManager.Instance.CheckRecipe();
        }
    }

    public void RefreshCraftingUI()
    {
        // Pull data from the Manager's crafting array
        InventorySlotData data = CraftingManager.Instance.GetInputSlotData(this.slotIndex);

        if (data.item != null)
        {
            UpdateSlot(data.item, data.amount);
        }
        else
        {
            ClearSlot();
        }
    }
}