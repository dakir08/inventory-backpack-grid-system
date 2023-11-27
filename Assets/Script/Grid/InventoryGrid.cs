using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour
{
    public GameObject slotPrefab;
    [Tooltip("If enable, gridmanager will override gridWidth, gridHeight, and cellSize")]
    public bool useInventoryGrid;
    public int gridWidth = 5;
    public int gridHeight = 4;
    public int cellSize = 50;
    public float slotSpacing = 5f;

    public Vector2Int availableSpaceStart = new Vector2Int(0, 0); // Start position
    public int availableSpaceWidth = 2;
    public int availableSpaceHeight = 2;


    private void Start()
    {
        GenerateGrid();
        // InitAvailableSpace();
    }

    void GenerateGrid()
    {
        float width = gridWidth;
        float height = gridHeight;
        float size = cellSize;

        if (useInventoryGrid)
        {
            width = GridManager.Instance.gridWidth;
            height = GridManager.Instance.gridHeight;
            size = GridManager.Instance.cellSize;
        }

        GridLayoutGroup gridLayout = gameObject.GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(size, size); // Adjust cell size as needed
        gridLayout.spacing = new Vector2(slotSpacing, slotSpacing);



        Debug.Log(width + " " + height);


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject slotObj = Instantiate(slotPrefab, transform);
                // InventorySlot slot = slotObj.GetComponent<InventorySlot>();
                // slot.Initialize(new Vector2Int(x, y));
            }
        }




        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(
            width * size + slotSpacing * (width - 1),
            height * size + slotSpacing * (height - 1)
            ); // Adjust the panel size
    }
}
