using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingInputSlot : InventorySlot // Inherit to keep the UI visuals
{
    public override void OnDrop(PointerEventData eventData)
    {
        // 1. Do the normal drag-and-drop swap
        base.OnDrop(eventData);

        // 2. Tell the CraftingManager to check if these items make a recipe
        CraftingManager.Instance.CheckRecipe();
    }
}