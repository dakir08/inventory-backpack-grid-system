using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InventoryExpand : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private InventoryTetris inventoryTetris;
    private InventoryTetrisBackground inventoryTetrisBackground;
   
    private readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
   
    private void Awake()
    {
        inventoryTetrisBackground = GetComponent<InventoryTetrisBackground>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        GameObject currentObject = eventData.pointerCurrentRaycast.gameObject;
        if (currentObject == null) return;
        if (currentObject.name[..1] != "T") return;
        Debug.Log(currentObject.name);
        string objectValue = currentObject.name;
        int x = 0, y = 0;
        if (objectValue.IndexOf("(") != -1) {
            objectValue = objectValue.Substring(objectValue.IndexOf("("));
            objectValue = objectValue.Replace("(", "");
            objectValue = objectValue.Replace(")", "");
            int index = int.Parse(objectValue);
            x = index / 10;
            y = index % 10;
        }

        bool isValid = false;

        foreach (Vector2Int direction in directions) {
            Vector2Int neighbourCoordinates = new Vector2Int(x, y) + direction;

            if (inventoryTetris.GetGrid().IsValidGridPosition(neighbourCoordinates) && inventoryTetris.GetGrid().GetGridObject(neighbourCoordinates.y, neighbourCoordinates.x).GetIsPlaceable()) {
                isValid = true;
                break;
            }
        }

        if (!isValid) return;

        Debug.Log(x + " " + y);
        inventoryTetris.GetGrid().GetGridObject(y, x).SetIsPlaceable(true);
        inventoryTetrisBackground.UpdateVisual(x, y);

        foreach (Vector2Int direction in directions) {
            Vector2Int neighbourCoordinates = new Vector2Int(x, y) + direction;

            inventoryTetris.PotentialGridVisual(neighbourCoordinates.x, neighbourCoordinates.y);
        } 
    }
}