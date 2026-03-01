using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    [SerializeField] private Transform quickAccessParent; 
    [SerializeField] private Transform backpackParent;

    private List<InventorySlot> allUiSlots = new List<InventorySlot>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        if (quickAccessParent != null)
        {
            allUiSlots.AddRange(quickAccessParent.GetComponentsInChildren<InventorySlot>());
        }

        if (backpackParent != null)
        {
            allUiSlots.AddRange(backpackParent.GetComponentsInChildren<InventorySlot>());
        }
    }

    public void RefreshInventoryUI(List<InventorySlotData> inventoryList)
    {
        for (int i = 0; i < allUiSlots.Count; i++)
        {
            allUiSlots[i].slotIndex = i; 

            if (i < inventoryList.Count && inventoryList[i].item != null)
            {
                allUiSlots[i].UpdateSlot(inventoryList[i].item, inventoryList[i].amount);
            }
            else
            {
                allUiSlots[i].ClearSlot();
            }
        }
    }
}