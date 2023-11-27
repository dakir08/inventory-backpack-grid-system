using UnityEngine;

public class Node
{
    public Vector2Int Position { get; private set; }
    public bool isAvailable;
    public bool isPlaced;

    public Node(Vector2Int position)
    {
        Position = position;
        isAvailable = false;
        isPlaced = false;
    }


    public void SetAvailable(bool value)
    {
        isAvailable = value;
    }

}
