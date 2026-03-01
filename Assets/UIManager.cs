using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    [SerializeField] private Transform slotParent; 

    private InventorySlot[] uiSlots;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        if (slotParent != null)
        {
            uiSlots = slotParent.GetComponentsInChildren<InventorySlot>();
        }
    }

    public void RefreshInventoryUI(List<InventorySlotData> inventoryList)
    {
        // 1. Clear existing UI slots
        foreach (var slot in uiSlots)
        {
            slot.ClearSlot();
        }

        // 2. Fill slots from left to right based on the new List
        for (int i = 0; i < inventoryList.Count; i++)
        {
            // Stop if we run out of physical UI slots
            if (i >= uiSlots.Length) break;

            uiSlots[i].UpdateSlot(inventoryList[i].item, inventoryList[i].amount);
        }
    }
}