using UnityEngine.EventSystems;

public class ChestSlot : InventorySlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (draggedSlot == null) return;

        // 1. Are we dragging FROM one Chest Slot to ANOTHER Chest Slot?
        if (draggedSlot is ChestSlot)
        {
            ChestUIManager.Instance.SwapChestToChest(draggedSlot.slotIndex, this.slotIndex);
            return; // STOP!
        }

        // 2. Are we dragging FROM the Backpack INTO the Chest?
        if (draggedSlot.slotIndex >= 0 && draggedSlot.slotIndex < 46)
        {
            ChestUIManager.Instance.SwapPlayerAndChest(draggedSlot.slotIndex, this.slotIndex);
            return; // STOP!
        }
    }
}