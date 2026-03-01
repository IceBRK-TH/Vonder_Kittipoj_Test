using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int slotIndex;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    // This updates the specific slot with item data
    public void UpdateSlot(Item item, int amount)
    {
        iconImage.sprite = item.icon;
        iconImage.enabled = true;

        if (item.IsStackable)
        {
            amountText.text = amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
    }

    public void ClearSlot()
    {
        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (amountText != null)
        {
            amountText.text = "";
        }
    }
}