using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Testing Items")]
    [SerializeField] private List<Item> starterItem = new List<Item>();
    [SerializeField] private int starterAmount = 15;
    [SerializeField] private InventoryManager inventoryManager; 

    // GOOD: Waiting until everything is loaded
    void Start()
    {
        for (int i = 0; i < starterItem.Count; i++)
        {
            if (starterItem[i] != null)
            {
                inventoryManager.AddItem(starterItem[i], starterAmount);
                Debug.Log($"GameManager: Added {starterAmount} {starterItem[i].itemName} to inventory at start.");
            }
            else
            {
                Debug.LogWarning($"GameManager: Starter item at index {i} is not assigned in the Inspector!");
            }
        }
    }
    void Update()
    {
        // Manual trigger: Press 'G' to add the starter item to your inventory
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddTestItem();
        }
    }

    public void AddTestItem()
    {
        if (starterItem != null)
        {
            // Accesses the InventoryManager Singleton we created earlier
            inventoryManager.AddItem(starterItem[0], starterAmount);
            Debug.Log($"GameManager: Manually added {starterAmount} {starterItem[0].itemName}");
        }
        else
        {
            Debug.LogWarning("GameManager: No starter item assigned in the Inspector!");
        }
    }
}