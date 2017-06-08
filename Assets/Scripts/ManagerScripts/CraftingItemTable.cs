using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingItemTable : MonoBehaviour {

    [SerializeField]
    public GameObject Sword;
    [SerializeField]
    public GameObject Hammer;
    [SerializeField]
    public GameObject Shield;
    [SerializeField]
    public GameObject Axe;
    [SerializeField]
    public GameObject Wood;

    public void FixedUpdate()
    {
        
    }

    public void craftItem(Item item1, Item item2, Transform spawnPoint)
    {
        if(item1.itemType == Item.ItemType.Rock && item2.itemType == Item.ItemType.Rock)
        {
            spawnObject(Shield, spawnPoint);
        }

        if (item1.itemType == Item.ItemType.Stick && item2.itemType == Item.ItemType.Stick)
        {
            spawnObject(Wood, spawnPoint);
        }

        if (item1.itemType == Item.ItemType.Wood && item2.itemType == Item.ItemType.Wood)
        {
            spawnObject(Axe, spawnPoint);
        }

        if (item1.itemType == Item.ItemType.Rock && item2.itemType == Item.ItemType.Stick || item1.itemType == Item.ItemType.Stick && item2.itemType == Item.ItemType.Rock)
        {
            spawnObject(Sword, spawnPoint);
        }

        if (item1.itemType == Item.ItemType.Rock && item2.itemType == Item.ItemType.Wood || item1.itemType == Item.ItemType.Wood && item2.itemType == Item.ItemType.Rock)
        {
            spawnObject(Hammer, spawnPoint);
        }

        Destroy(item1);
        Destroy(item2);
    }

    private void spawnObject(GameObject objectToSpawn, Transform spawnPoint)
    {
        Debug.Log("Spawning: " + objectToSpawn.name);
        Instantiate(objectToSpawn, spawnPoint);
    }
}
