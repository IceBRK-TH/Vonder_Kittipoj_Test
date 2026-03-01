using UnityEngine;

public class ChestUIManager : MonoBehaviour
{
    public static ChestUIManager Instance;

    [Header("UI References")]
    public GameObject statusPanel;
    public GameObject chestPanel;
    public GameObject equipmentPanel;
    public GameObject craftingPanel;
    public ChestSlot[] chestUISlots; // Drag your 10 UI slots here in the Inspector

    private Chest currentOpenChest; // Remembers which chest in the world we are looking at

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Called by Chest.cs when you press 'E'
    public void OpenChest(Chest chest)
    {
        currentOpenChest = chest;
        statusPanel.SetActive(true);
        chestPanel.SetActive(true);
        equipmentPanel.SetActive(false);
        craftingPanel.SetActive(false);
        RefreshChestUI();
    }

    public void CloseChest()
    {
        currentOpenChest = null;
        statusPanel.SetActive(false);
        chestPanel.SetActive(false);
        equipmentPanel.SetActive(true);
        craftingPanel.SetActive(true);
    }

    public void RefreshChestUI()
    {
        if (currentOpenChest == null) return;

        // Loop through the UI slots and match them with the Chest's data
        for (int i = 0; i < chestUISlots.Length; i++)
        {
            if (i < currentOpenChest.chestContents.Count)
            {
                InventorySlotData data = currentOpenChest.chestContents[i];
                if (data.item != null)
                {
                    chestUISlots[i].UpdateSlot(data.item, data.amount);
                }
                else
                {
                    chestUISlots[i].ClearSlot();
                }
            }
        }
    }

    public void SwapPlayerAndChest(int playerIndex, int chestIndex)
    {
        // TRAP 1: Is a chest actually linked?
        if (currentOpenChest == null)
        {
            Debug.LogError("🚨 SWAP FAILED: 'currentOpenChest' is null! You must open the chest by colliding with it and pressing 'E' in the game, not just leaving the UI panel on.");
            return;
        }

        // TRAP 2: Are the indices out of bounds?
        if (playerIndex < 0 || playerIndex >= InventoryManager.Instance.inventory.Count)
        {
            Debug.LogError($"🚨 SWAP FAILED: Player Index {playerIndex} is out of bounds!");
            return;
        }
        if (chestIndex < 0 || chestIndex >= currentOpenChest.chestContents.Count)
        {
            Debug.LogError($"🚨 SWAP FAILED: Chest Index {chestIndex} is out of bounds! Does the chest have {currentOpenChest.chestSize} slots?");
            return;
        }

        // Grab the data from both lists
        InventorySlotData playerSlot = InventoryManager.Instance.inventory[playerIndex];
        InventorySlotData chestDataSlot = currentOpenChest.chestContents[chestIndex];

        // Swap the items and amounts
        Item tempItem = playerSlot.item;
        int tempAmount = playerSlot.amount;

        playerSlot.item = chestDataSlot.item;
        playerSlot.amount = chestDataSlot.amount;

        chestDataSlot.item = tempItem;
        chestDataSlot.amount = tempAmount;

        // Refresh the UI to show the new items
        InventoryManager.Instance.RefreshUI();
        RefreshChestUI();

        Debug.Log("✅ SWAP SUCCESSFUL!");
    }
    public void SwapChestToChest(int indexA, int indexB)
    {
        if (currentOpenChest == null)
        {
            Debug.LogError("No chest is currently open!");
            return;
        }

        // Grab the data from the chest's memory
        InventorySlotData dataA = currentOpenChest.chestContents[indexA];
        InventorySlotData dataB = currentOpenChest.chestContents[indexB];

        // Swap the items and amounts
        Item tempItem = dataA.item;
        int tempAmount = dataA.amount;

        dataA.item = dataB.item;
        dataA.amount = dataB.amount;

        dataB.item = tempItem;
        dataB.amount = tempAmount;

        // Refresh the Chest UI visually
        RefreshChestUI();
    }
}