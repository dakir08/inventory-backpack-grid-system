using UnityEngine;
using UnityEngine.UI;

public class EquipmentGrid : MonoBehaviour
{
    public GameObject slotPrefab;
    public int gridWidth = 5;
    public int gridHeight = 4;


    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float size = GridManager.Instance.cellSize;
        float slotSpacing = GridManager.Instance.slotSpacing;

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject slotObj = Instantiate(slotPrefab, transform);
                EquipmentSlot slot = slotObj.GetComponent<EquipmentSlot>();
                slot.Initialize(new Node(new Vector2Int(x, y)));
                slot.node.SetAvailable(true);
            }
        }


        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(
            gridWidth * size + slotSpacing * (gridWidth - 1),
            gridHeight * size + slotSpacing * (gridHeight - 1)
            ); // Adjust the panel size
    }
}
