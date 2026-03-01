using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public  abstract class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler , IDropHandler
{
    public int slotIndex;


    public Item item;
    public int amount;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    private Transform originalParent;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError($"CRITICAL: Slot {gameObject.name} cannot find a Canvas!");
        }
    }

    public void UpdateSlot(Item newItem, int newAmount)
    {
        if (iconImage == null || amountText == null) return;

        this.item = newItem;
        this.amount = newAmount;

        iconImage.sprite = item.icon;
        iconImage.enabled = true;
        amountText.text = item.IsStackable ? amount.ToString() : "";
    }

    public void ClearSlot()
    {
        this.item = null;
        this.amount = 0;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
            iconImage.transform.localPosition = Vector3.zero;
        }
        if (amountText != null) amountText.text = "";
    }
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (iconImage.sprite == null) return;

        originalParent = iconImage.transform.parent;

        if (canvas != null)
        {
            iconImage.transform.SetParent(canvas.transform);

            iconImage.transform.SetAsLastSibling();
        }

        iconImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (iconImage.sprite != null)
        {
            iconImage.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (iconImage != null && originalParent != null)
        {
            iconImage.transform.SetParent(originalParent);
            iconImage.transform.localPosition = Vector3.zero;
            iconImage.raycastTarget = true; // This un-freezes the slot!
        }

    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (draggedSlot == null) return;

        // 1. Identify the Source
        bool isFromOutput = draggedSlot is CraftingOutputSlot;
        bool isFromBackpack = !isFromOutput && (draggedSlot.slotIndex >= 0 && draggedSlot.slotIndex <= 45);

        // 2. Handle Crafting Result (The "Claim" Logic)
        if (isFromOutput)
        {
            // Add item to the real 46-slot data list
            InventoryManager.Instance.AddItem(draggedSlot.item, draggedSlot.amount);

            // Clean up the 10 logs in the grid
            CraftingManager.Instance.OnCraftComplete();

            return; // STOP HERE! This prevents the split/double-spawn
        }

        // 3. Handle Normal Swap (Only for 0-45)
        if (isFromBackpack && this.slotIndex >= 0 && this.slotIndex <= 45)
        {
            InventoryManager.Instance.SwapItems(draggedSlot.slotIndex, this.slotIndex);
        }
    }
}