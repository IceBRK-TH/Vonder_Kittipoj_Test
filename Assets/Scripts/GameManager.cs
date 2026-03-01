using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Testing Items")]
    [SerializeField] private Item starterItem; // Drag your 'Resistor' or 'Wire' asset here
    [SerializeField] private int starterAmount = 5;
    [SerializeField] private InventoryManager inventoryManager; 

    // GOOD: Waiting until everything is loaded
    void Start()
    {
        inventoryManager.AddItem(starterItem, 15);
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
            inventoryManager.AddItem(starterItem, starterAmount);
            Debug.Log($"GameManager: Manually added {starterAmount} {starterItem.itemName}");
        }
        else
        {
            Debug.LogWarning("GameManager: No starter item assigned in the Inspector!");
        }
    }
}