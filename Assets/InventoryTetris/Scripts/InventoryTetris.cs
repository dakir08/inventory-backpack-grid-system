using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Analytics;
using UnityEngine.UIElements;
using System.Data.Common;

public class InventoryTetris : MonoBehaviour {

    public static InventoryTetris Instance { get; private set; }

    [SerializeField] private InventoryTetrisBackground inventoryTetrisBackground;

    public event EventHandler<PlacedObject> OnObjectPlaced;

    private Grid<GridObject> grid;
    private RectTransform itemContainer;
    internal object BackgroundSingleTransformListStatic;

    private readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };


    private void Awake() {
        Instance = this;

        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 50f;
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        for (int y = 0; y < grid.GetHeight(); y++) {
            for (int x = 0; x < grid.GetWidth(); x++) {
                grid.GetGridObject(x, y).SetIsPlaceable(false);
            }
        }

        itemContainer = transform.Find("ItemContainer").GetComponent<RectTransform>();

        transform.Find("BackgroundTempVisual").gameObject.SetActive(false);
    }

    private void Start() {
        StartUpInventory();
    }

    private void StartUpInventory()
    {
        for (int y = 0; y < grid.GetHeight(); y++) {
            for (int x = 0; x < grid.GetWidth(); x++) {
                if (x >= 3 && x <= 6 && y >= 4 && y <= 6) {
                    grid.GetGridObject(x, y).SetIsPlaceable(true);
                }
            }
        }

        // create potential grid object visual
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                PotentialGridVisual(x, y);
            }
        }
    }

    internal void PotentialGridVisual(int x, int y) {
        
        if (grid.GetGridObject(y, x).GetIsPlaceable()) return;

        bool isValid = false;

        foreach (Vector2Int direction in directions) {
            Vector2Int neighbourCoordinates = new Vector2Int(x, y) + direction;

            if (grid.IsValidGridPosition(neighbourCoordinates) && grid.GetGridObject(neighbourCoordinates.y, neighbourCoordinates.x).GetIsPlaceable()) {
                isValid = true;
                break;
            }
        }
    
        if (!isValid) return;
        
        // Debug.Log("Potential Block: " + x + " " + y);
        inventoryTetrisBackground.PotentialGridVisual(x, y);
    }

    public class GridObject {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        public PlacedObject placedObject;
        private bool isPlaceable, isTriggeredObject;

        public GridObject(Grid<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
            isTriggeredObject = false;
            isPlaceable = true;
        }

        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(PlacedObject placedObject) {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearPlacedObject() {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetIsPlaceable(bool isPlaceable) {
            this.isPlaceable = isPlaceable;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetTriggeredObject(bool isTriggeredObject) {
            this.isTriggeredObject = isTriggeredObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public bool GetIsTriggeredObject() {
            return isTriggeredObject;
        }

        public PlacedObject GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {
            return (placedObject == null && isPlaceable) || isTriggeredObject;
        }

        public bool HasPlacedObject() {
            return (placedObject != null || !isPlaceable) && !isTriggeredObject;
        }

        public bool GetIsPlaceable()
        {
            return isPlaceable;
        }
    }

    public Grid<GridObject> GetGrid() {
        return grid;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXY(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    // public Vector2Int GetTilePosition(Vector3 worldPosition) {
    //     Vector2Int gridPosition = GetGridPosition(worldPosition);
    //     return new Vector2Int(gridPosition.x, gridPosition.y);
    // }

    public bool IsValidGridPosition(Vector2Int gridPosition) {
        return grid.IsValidGridPosition(gridPosition);
    }

    // public void Extend(Vector2Int gridPosition) {
    //     if (grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild();
    // }

    public void UpdateValidVisual (Vector2Int gridPosition)
    {
        inventoryTetrisBackground.GridVisual(gridPosition.y, gridPosition.x);
        // Debug.Log(indexString);
        
    }
    
    public void ResetVisual()
    {
        for (int y = 0; y < grid.GetHeight(); y++) {
            for (int x = 0; x < grid.GetWidth(); x++) {
                if (!grid.GetGridObject(x, y).GetIsPlaceable()) continue;
                inventoryTetrisBackground.UpdateVisual(y, x);
            }
        }
    }

    public void UpdateUnValidVisual(Vector2Int gridPosition)
    {
        if (!grid.IsValidGridPosition(gridPosition)) return;
        if (!grid.GetGridObject(gridPosition.x, gridPosition.y).GetIsPlaceable()) return;
        inventoryTetrisBackground.UnValidVisual(gridPosition.y, gridPosition.x);
    }

    public bool CheckValidPlacement(ItemTetrisSO itemTetrisSO, Vector2Int placedObjectOrigin, PlacedObjectTypeSO.Dir dir) {
        List<Vector2Int> gridPositionList = itemTetrisSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canPlace = true;
        foreach (Vector2Int gridPosition in gridPositionList) {

            // Debug.Log(gridPosition);

            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition) {
                // Not valid
                canPlace = false;
                break;
            }
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                canPlace = false;
                break;
            }
        }

        if (canPlace) {
            foreach (Vector2Int gridPosition in gridPositionList) {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                    canPlace = false;
                    break;
                }
            }
        }

        return canPlace;
    }

    public bool TryPlaceItem(ItemTetrisSO itemTetrisSO, Vector2Int placedObjectOrigin, PlacedObjectTypeSO.Dir dir) {
        // Test Can Build
        List<Vector2Int> gridPositionList = itemTetrisSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canPlace = true;
        foreach (Vector2Int gridPosition in gridPositionList) {

            Debug.Log(gridPosition);

            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition) {
                // Not valid
                canPlace = false;
                break;
            }
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                canPlace = false;
                break;
            }
        }

        if (canPlace) {
            foreach (Vector2Int gridPosition in gridPositionList) {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                    canPlace = false;
                    break;
                }
            }
        }

        if (canPlace) {
            Vector2Int rotationOffset = itemTetrisSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

            PlacedObject placedObject = PlacedObject.CreateCanvas(itemContainer, placedObjectWorldPosition, placedObjectOrigin, dir, itemTetrisSO);
            placedObject.transform.rotation = Quaternion.Euler(0, 0, -itemTetrisSO.GetRotationAngle(dir));

            placedObject.GetComponent<InventoryTetrisDragDrop>().Setup(this);

            foreach (Vector2Int gridPosition in gridPositionList) {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }

            OnObjectPlaced?.Invoke(this, placedObject);
        
            // Object Placed!
            return true;
        } else {
            // Object CANNOT be placed!
            return false;
        }
    }

    public void RemoveItemAt(Vector2Int removeGridPosition) {
        PlacedObject placedObject = grid.GetGridObject(removeGridPosition.x, removeGridPosition.y).GetPlacedObject();

        if (placedObject != null) {
            // Demolish
            placedObject.DestroySelf();

            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList) {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }
        }
    }

    public RectTransform GetItemContainer() {
        return itemContainer;
    }



    [Serializable]
    public struct AddItemTetris {
        public string itemTetrisSOName;
        public Vector2Int gridPosition;
        public PlacedObjectTypeSO.Dir dir;
    }

    [Serializable]
    public struct ListAddItemTetris {
        public List<AddItemTetris> addItemTetrisList;
    }

    public string Save() {
        List<PlacedObject> placedObjectList = new List<PlacedObject>();
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                if (grid.GetGridObject(x, y).HasPlacedObject()) {
                    placedObjectList.Remove(grid.GetGridObject(x, y).GetPlacedObject());
                    placedObjectList.Add(grid.GetGridObject(x, y).GetPlacedObject());
                }
            }
        }

        List<AddItemTetris> addItemTetrisList = new List<AddItemTetris>();
        foreach (PlacedObject placedObject in placedObjectList) {
            addItemTetrisList.Add(new AddItemTetris {
                dir = placedObject.GetDir(),
                gridPosition = placedObject.GetGridPosition(),
                itemTetrisSOName = (placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO).name,
            });

        }

        return JsonUtility.ToJson(new ListAddItemTetris { addItemTetrisList = addItemTetrisList });
    }

    public void Load(string loadString) {
        ListAddItemTetris listAddItemTetris = JsonUtility.FromJson<ListAddItemTetris>(loadString);

        foreach (AddItemTetris addItemTetris in listAddItemTetris.addItemTetrisList) {
            TryPlaceItem(InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(addItemTetris.itemTetrisSOName), addItemTetris.gridPosition, addItemTetris.dir);
        }
    }
}
