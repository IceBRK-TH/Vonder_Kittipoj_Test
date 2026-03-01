using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler , IDropHandler
{
    public int slotIndex;

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
    
    public void UpdateSlot(Item item, int amount)
    {
        if (iconImage == null || amountText == null) return;

        iconImage.sprite = item.icon;
        iconImage.enabled = true;
        amountText.text = item.IsStackable ? amount.ToString() : "";
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
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (iconImage.sprite == null) return;

        originalParent = iconImage.transform.parent;

        // 2. Safely use the locked-in Canvas
        if (canvas != null)
        {
            iconImage.transform.SetParent(canvas.transform);

            // 3. Force the dragged icon to the very front of the screen!
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
        if (iconImage.sprite == null) return;

        iconImage.transform.SetParent(originalParent);
        iconImage.transform.localPosition = Vector3.zero;
        iconImage.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject != null)
        {
            InventorySlot draggedSlot = draggedObject.GetComponent<InventorySlot>();

            if (draggedSlot != null && draggedSlot != this)
            {
                InventoryManager.Instance.SwapItems(draggedSlot.slotIndex, this.slotIndex);
            }
        }
    }
}