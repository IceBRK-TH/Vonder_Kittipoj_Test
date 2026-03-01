using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : InventorySlot, IDropHandler
{
    public ItemType acceptType; // Set this to 'Weapon' in the Inspector for the weapon slot
    public int equipmentIndex;  // 0 for Weapon, 1 for Armor, etc.

    public override void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot = eventData.pointerDrag.GetComponent<InventorySlot>();

        if (draggedSlot != null)
        {
            // 1. Equip the item in the manager
            InventoryManager.Instance.EquipItem(draggedSlot.slotIndex, this.equipmentIndex);

            // 2. IMMEDIATELY tell this slot to show the new item
            RefreshSlotUI();
        }
    }
    public void RefreshSlotUI()
    {
      
        if (equipmentIndex == 0) this.item = InventoryManager.Instance.equippedWeapon;
        else if (equipmentIndex == 1) this.item = InventoryManager.Instance.equippedArmor;
        else if (equipmentIndex == 2) this.item = InventoryManager.Instance.equippedLeggings;

        if (this.item != null)
        {
            UpdateSlot(this.item, 1);
        }
        else
        {
            ClearSlot();
        }
    }
}
