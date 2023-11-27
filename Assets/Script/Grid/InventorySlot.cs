using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Node node;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(Node node)
    {
        this.node = node;
        if (node.isAvailable)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0.5f;
        }
    }

    public void MakeAvailable()
    {
        if (!GridManager.Instance.HasAvailableNeighbor(node))
        {
            return;
        }

        node.SetAvailable(true);
        canvasGroup.alpha = 1f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ONDROP");
        if (eventData.pointerDrag != null)
        {
            Debug.Log($"Node Position: {node.position}");
        }
    }
}
