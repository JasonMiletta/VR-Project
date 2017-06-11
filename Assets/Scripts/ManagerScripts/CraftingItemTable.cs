using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingItemTable : MonoBehaviour {

    public Item item1 = null;
    public Item item2 = null;

    public GameObject CraftingSpawnPoint;

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

    private void OnEnable()
    {
        LatchClosedTrigger.OnClosed += craftItem;
    }
    private void OnDisable()
    {
        LatchClosedTrigger.OnClosed -= craftItem;
    }

    public void Start()
    {

    }

    public void FixedUpdate()
    {
        
    }

    public void craftItem()
    {
        Debug.Log("Craft Item!");
        if (item1 != null && item2 != null)
        {
            craftItem(item1, item2, CraftingSpawnPoint.transform);
        }
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

    }

    private void spawnObject(GameObject objectToSpawn, Transform spawnPoint)
    {
        Debug.Log("Spawning: " + objectToSpawn.name);
        Destroy(item1.gameObject);
        Destroy(item2.gameObject);

        item1 = null;
        item2 = null;

        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
    }

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        Item collidingItem = other.gameObject.GetComponent<Item>();
        if (collidingItem != null)
        {
            Debug.Log("Item entered");
            if (item1 == null)
            {
                Debug.Log("item 1");
                item1 = collidingItem;
            }
            else if (item2 == null)
            {
                Debug.Log("item 2");
                item2 = collidingItem;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Item otherItem = other.gameObject.GetComponent<Item>();
        Debug.Log("Item exited");
        if (item1 == otherItem)
        {
            Debug.Log("Item 1 exited");
            item1 = null;
        } else if(item2 == otherItem)
        {
            item2 = null;
            Debug.Log("Item 2 exited");
        }
    }
    #endregion
}
