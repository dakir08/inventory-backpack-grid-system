using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 50f;
    public float slotSpacing = 5f;
    public Node[,] inventoryGrid;
    public Node[,] debugInventoryGrid;
    public Vector3 originPosition = Vector3.zero; // Default to (0, 0, 0)

    public Vector2Int availableSpaceStart = new Vector2Int(0, 0); // Start position
    public int availableSpaceWidth = 2;
    public int availableSpaceHeight = 2;

    // New

    protected override void Awake()
    {
        base.Awake();

        CreateGrid();
    }

    private void Start()
    {
        InitAvailableSpace();
    }

    private void CreateGrid()
    {
        inventoryGrid = new Node[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                inventoryGrid[x, y] = new Node(new Vector2Int(x, y));
            }
        }
    }

    // Method to access a node at a particular position
    public Node GetNode(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return inventoryGrid[x, y];
        }
        return null;
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int y = Mathf.FloorToInt(worldPosition.y / cellSize);

        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return inventoryGrid[x, y];
        }
        Debug.Log("No node from this world position");
        return null; // Return null if the position is outside the grid
    }

    public bool HasAvailableNeighbor(Node node)
    {
        Vector2Int[] neighborOffsets = new Vector2Int[]
        {
            new Vector2Int(0, 1),  // North
            new Vector2Int(1, 0),  // East
            new Vector2Int(0, -1), // South
            new Vector2Int(-1, 0)  // West
        };

        foreach (var offset in neighborOffsets)
        {
            Vector2Int neighborPosition = node.position + offset;
            if (IsWithinGrid(neighborPosition) && inventoryGrid[neighborPosition.x, neighborPosition.y].isAvailable)
            {
                return true;
            }
        }

        return false;
    }

    // Helper method to check if a position is within the grid bounds
    private bool IsWithinGrid(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridWidth && position.y >= 0 && position.y < gridHeight;
    }

    private void InitAvailableSpace()
    {
        for (int x = 0; x < availableSpaceWidth; x++)
        {
            for (int y = 0; y < availableSpaceHeight; y++)
            {
                int posX = availableSpaceStart.x + x;
                int posY = availableSpaceStart.y + y;

                if (posX < gridWidth && posY < gridHeight)
                {
                    inventoryGrid[posX, posY].SetAvailable(true);
                }
            }
        }
    }


    public bool AreAllNodesAvailable(Node startNode, int equipmentWidth, int equipmentHeight)
    {
        if (startNode == null) return false;

        int startX = startNode.position.x;
        int startY = startNode.position.y;

        if (startX + equipmentWidth + 1 > gridWidth || startY + equipmentHeight + 1 > gridHeight)
        {
            return false;
        }

        for (int x = startX; x < startX + equipmentWidth; x++)
        {
            for (int y = startY; y < startY + equipmentHeight; y++)
            {
                if (!IsWithinGrid(new Vector2Int(x, y)) || !inventoryGrid[x, y].isAvailable)
                {
                    return false; // If the node is outside the grid or not available
                }
            }
        }

        return true; // All nodes in the specified area are available
    }


    private void CreateDebugGrid()
    {
        debugInventoryGrid = new Node[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                debugInventoryGrid[x, y] = new Node(new Vector2Int(x, y));
            }
        }
    }

    // Debug
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        float originSize = 20f; // Size of the origin marker
        Gizmos.DrawCube(originPosition, new Vector3(originSize, originSize, originSize));

        // CreateDebugGrid();

        // Gizmos.color = Color.gray;
        // for (int x = 0; x < gridWidth; x++)
        // {
        //     for (int y = 0; y < gridHeight; y++)
        //     {
        //         var node = debugInventoryGrid[x, y];
        //         var position = new Vector3(x * cellSize, y * cellSize, 0);

        //         // Change color if node is occupied
        //         Gizmos.color = node != null && node.isAvailable ? Color.red : Color.gray;

        //         // Draw a small cube at each node position
        //         Gizmos.DrawCube(position, new Vector3(cellSize, cellSize, 0.1f));
        //     }
        // }
    }
}
