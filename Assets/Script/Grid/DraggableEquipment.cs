using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableEquipment : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private EquipmentGrid equipmentGrid;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("InventoryGridUI").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        equipmentGrid = GetComponent<EquipmentGrid>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        InventorySlot overSlot = FindSlotUnderEquipment(eventData);

        if (overSlot != null)
        {
            bool available = GridManager.Instance.AreAllNodesAvailable(overSlot.node, equipmentGrid.gridWidth, equipmentGrid.gridHeight);
            Debug.Log("available " + available);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private InventorySlot FindSlotUnderEquipment(PointerEventData eventData)
    {
        InventoryGrid inventoryGrid = FindObjectOfType<InventoryGrid>(); // Cache this if performance is an issue
        foreach (InventorySlot slot in inventoryGrid.GetComponentsInChildren<InventorySlot>())
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(
                slot.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera))
            {
                return slot;
            }
        }
        return null;
    }

}
