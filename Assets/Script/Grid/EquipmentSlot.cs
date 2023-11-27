using UnityEngine;

public class EquipmentSlot : MonoBehaviour
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
        canvasGroup.alpha = 0.5f;
    }

}
