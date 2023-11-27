using UnityEngine;

public class Node
{
    public Vector2Int position { get; private set; }
    public bool isAvailable;
    public bool isPlaced;

    public Node(Vector2Int position)
    {
        this.position = position;
        isAvailable = false;
        isPlaced = false;
    }


    public void SetAvailable(bool value)
    {
        isAvailable = value;
    }

}
