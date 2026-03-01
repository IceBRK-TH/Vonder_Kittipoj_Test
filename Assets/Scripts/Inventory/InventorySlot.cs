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
        }
        if (amountText != null) amountText.text = "";
    }
    public void OnBeginDrag(PointerEventData eventData)
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
        if (iconImage.sprite == null) return;

        iconImage.transform.SetParent(originalParent);
        iconImage.transform.localPosition = Vector3.zero;
        iconImage.raycastTarget = true;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"Mouse just dropped an item onto: {gameObject.name}");
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