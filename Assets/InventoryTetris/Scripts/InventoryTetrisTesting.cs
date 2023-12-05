using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Data.Common;
using UnityEditor;
using System.Net;
using UnityEngine.EventSystems;
using System.Drawing;
using UnityEngine.UI;

public class InventoryTetrisTesting : MonoBehaviour {

    [SerializeField] private Transform outerInventoryTetrisBackground;
    // [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private InventoryTetris outerInventoryTetris;
    [SerializeField] private List<string> addItemTetrisSaveList;

    private PlacedObject placedObject;
    private InventoryTetris inventoryTetris;
    private InventoryTetrisBackground inventoryTetrisBackground;

    private int addItemTetrisSaveListIndex;


    Vector2Int origin;

    private void Awake() {
        placedObject = GetComponent<PlacedObject>();
    }

    private void Start() {
        outerInventoryTetrisBackground.gameObject.SetActive(false);

        origin = new Vector2Int((int) Camera.main.transform.position.x - 45, (int) Camera.main.transform.position.y - 45);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            outerInventoryTetrisBackground.gameObject.SetActive(true);
            outerInventoryTetris.Load(addItemTetrisSaveList[addItemTetrisSaveListIndex]);

            addItemTetrisSaveListIndex = (addItemTetrisSaveListIndex + 1) % addItemTetrisSaveList.Count;
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Log(inventoryTetris.Save());
        }

        // if (Input.GetMouseButtonDown(1)) {
            

        //     // inventoryTetrisBackground.UpdateVisual();

        // }
    }
}
