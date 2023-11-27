using UnityEngine;

public class InventorySlot : MonoBehaviour
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

    public void OnPointerClick()
    {
        if (!GridManager.Instance.HasAvailableNeighbor(node))
        {
            return;
        }

        node.SetAvailable(true);
        canvasGroup.alpha = 1f;
    }
}
