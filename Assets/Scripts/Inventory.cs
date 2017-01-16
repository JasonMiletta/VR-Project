using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public Item[,] inventoryList = new Item[2, 2];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addItemToInventory(Item newItem)
    {
        return;
    }

    public Item getItemFromInventoryByIndex(int rowX, int colY)
    {
        Item retrievedItem = inventoryList[rowX, colY];
        inventoryList[rowX, colY] = null;

        return retrievedItem;
    }
}
