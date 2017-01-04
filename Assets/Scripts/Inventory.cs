using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public List<Item> inventoryList = new List<Item>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addItemToInventory(Item newItem)
    {
        inventoryList.Add(newItem);
    }

    public Item getItemFromInventoryByIndex(int index)
    {
        Item retrievedItem = inventoryList[index];
        inventoryList.RemoveAt(index);

        return retrievedItem;
    }
}
