using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bin : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image binImage;


    // --- DROP LOGIC FOR BIN ---

    // This method is called whenever an item is dropped onto the bin image
    public void OnDrop(PointerEventData eventData)
    {
        // 1. Get the GameObject that was being dragged
        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject != null)
        {
            // 2. Try to get the InventorySlot script from the dragged GameObject
            InventorySlot draggedSlot = draggedObject.GetComponent<InventorySlot>();

            // 3. If it's a valid slot (meaning an item dropped from an inventory slot)...
            if (draggedSlot != null)
            {
                Debug.Log($"InventoryBin: Item dropped from slot {draggedSlot.slotIndex}, removing...");

                // 4. Tell the InventoryManager to remove the item at that index
                InventoryManager.Instance.RemoveItem(draggedSlot.slotIndex);
            }
        }
    }
}
