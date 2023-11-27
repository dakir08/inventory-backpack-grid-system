using UnityEngine;
using UnityEngine.UI;


public class InventoryGrid : MonoBehaviour
{
    public GameObject slotPrefab;


    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float width = GridManager.Instance.gridWidth;
        float height = GridManager.Instance.gridHeight;
        float size = GridManager.Instance.cellSize;
        float slotSpacing = GridManager.Instance.slotSpacing;

        float rectWidth = width * size + slotSpacing * (width - 1);
        float rectHeight = height * size + slotSpacing * (height - 1);

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectWidth, rectHeight); // Adjust the panel size

        rectTransform.position = new Vector3(
            GridManager.Instance.originPosition.x,
            GridManager.Instance.originPosition.y + rectHeight,
            GridManager.Instance.originPosition.z
        );

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject slotObj = Instantiate(slotPrefab, transform);
                InventorySlot slot = slotObj.GetComponent<InventorySlot>();
                slot.Initialize(GridManager.Instance.GetNode(x, y));

                RectTransform slotRectTransform = slotObj.GetComponent<RectTransform>();

                // Calculate the position for each slot
                float slotPosX = x * size;
                float slotPosY = y * size;

                // Calculate cellSpacing
                if (x != 0)
                {
                    slotPosX += slotSpacing * x;
                }

                if (y != 0)
                {
                    slotPosY += slotSpacing * y;
                }


                // Set the position and size of the slot
                slotRectTransform.anchoredPosition = new Vector2(slotPosX, slotPosY);
            }
        }

    }
}
