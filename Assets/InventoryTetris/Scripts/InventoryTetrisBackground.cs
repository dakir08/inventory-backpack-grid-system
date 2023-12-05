using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTetrisBackground : MonoBehaviour {

    [SerializeField] private InventoryTetris inventoryTetris;
    private List<Transform> backgroundTransformList = new List<Transform>();
    private void Start() {
        // Create background
        for (int y = 0; y < inventoryTetris.GetGrid().GetHeight(); y++) {
            for (int x = 0; x < inventoryTetris.GetGrid().GetWidth(); x++) {
            
                string index = (y * inventoryTetris.GetGrid().GetHeight() + x).ToString();
                string indexString = "";
                if (index != "0") {
                    indexString = " ("+index+")";
                }

                Transform backgroundSingleTransform = gameObject.transform.Find("Template" + indexString);

                if (backgroundSingleTransform == null) continue;
                Debug.Log(backgroundSingleTransform.gameObject.name);

                backgroundSingleTransform.gameObject.SetActive(true);

                if (x >= 3 && x <= 6 && y >= 4 && y <= 6) continue;
                backgroundSingleTransform.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            }
        }

        GetComponent<GridLayoutGroup>().cellSize = new Vector2(inventoryTetris.GetGrid().GetCellSize(), inventoryTetris.GetGrid().GetCellSize());

        GetComponent<RectTransform>().sizeDelta = new Vector2(inventoryTetris.GetGrid().GetWidth(), inventoryTetris.GetGrid().GetHeight()) * inventoryTetris.GetGrid().GetCellSize();

        GetComponent<RectTransform>().anchoredPosition = inventoryTetris.GetComponent<RectTransform>().anchoredPosition;
    
        InventoryTetrisTesting inventoryTerisTesting = GameObject.Find("InventoryTetrisTesting").GetComponent<InventoryTetrisTesting>();
    }

    public void UpdateVisual(int y, int x) {
        string index = (y * inventoryTetris.GetGrid().GetWidth() + x).ToString();
        string indexString = "";
        if (index != "0") {
            indexString = " ("+index+")";
        }
        Transform backgroundSingleTransform = gameObject.transform.Find("Template" + indexString);
        backgroundSingleTransform.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
    }

    public void GridVisual(int y, int x)
    {
        string index = (y * inventoryTetris.GetGrid().GetWidth() + x).ToString();
        string indexString = "";
        if (index != "0") {
            indexString = " ("+index+")";
        }
        Debug.Log(indexString);
        Transform backgroundSingleTransform = gameObject.transform.Find("Template" + indexString);
        backgroundSingleTransform.gameObject.GetComponent<Image>().color = new Color(0, 255, 0, 1);
    }

    public void UnValidVisual(int y, int x)
    {
        string index = (y * inventoryTetris.GetGrid().GetWidth() + x).ToString();
        string indexString = "";
        if (index != "0") {
            indexString = " ("+index+")";
        }
        Debug.Log(indexString);
        Transform backgroundSingleTransform = gameObject.transform.Find("Template" + indexString);
        backgroundSingleTransform.gameObject.GetComponent<Image>().color = new Color(255, 0, 0, 1);
    }

    public void PotentialGridVisual(int y, int x)
    {
        string index = (y * inventoryTetris.GetGrid().GetWidth() + x).ToString();
        string indexString = "";
        if (index != "0") {
            indexString = " ("+index+")";
        }
        Debug.Log(indexString);
        Transform backgroundSingleTransform = gameObject.transform.Find("Template" + indexString);
        backgroundSingleTransform.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
    }
}