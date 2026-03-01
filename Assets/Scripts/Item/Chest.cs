using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest Storage")]
    public int chestSize = 10; // How many slots this chest has
    public List<InventorySlotData> chestContents = new List<InventorySlotData>();

    [Header("Interaction")]
    private bool isPlayerInRange = false;

    private void Awake()
    {
        // Initialize the chest with empty slots when it spawns in the world
        for (int i = 0; i < chestSize; i++)
        {
            chestContents.Add(new InventorySlotData(null, 0));
        }
    }

    private void Update()
    {
        // Check if player is nearby AND presses the 'E' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleChest();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Make sure your player GameObject has the tag "Player"
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Press 'E' to open chest.");
            // You can trigger a small UI prompt here later!
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // Auto-close the chest if the player walks away
            CloseChest();
        }
    }

    private void ToggleChest()
    {
        // Logic to toggle the UI on and off
        Debug.Log("Chest toggled!");

       ChestUIManager.Instance.OpenChest(this);
    }

    public void CloseChest()
    {
        Debug.Log("Chest closed.");
       ChestUIManager.Instance.CloseChest();
    }
}