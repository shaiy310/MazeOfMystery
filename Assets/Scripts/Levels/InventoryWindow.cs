using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    public GameObject[] inventoryImages;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in inventoryImages) {
            item.SetActive(Inventory.collectedItems[item.name]);
        }
    }
}
