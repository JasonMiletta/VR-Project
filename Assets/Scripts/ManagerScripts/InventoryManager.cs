using UnityEngine;
using VRTK;
using System.Collections;

[RequireComponent(typeof(VRTK_ControllerEvents))]
public class InventoryManager : MonoBehaviour {

    public GameObject inventoryPrefab;

    private VRTK_ControllerEvents controllerEvents;
    private bool isInventoryOpen = false;

	// Use this for initialization
	void Start ()
    {
        if (inventoryPrefab == null)
        {
            inventoryPrefab = GetComponentInChildren<Inventory>().gameObject;
            if(inventoryPrefab == null)
            {
                Debug.LogError("InventoryManager needs a reference to the inventoryPrefab to spawn!");
            }
        }
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void OnEnable()
    {
        if (controllerEvents)
        {
            controllerEvents.AliasMenuOn += new ControllerInteractionEventHandler(doToggleInventory);
            controllerEvents.AliasMenuOff += new ControllerInteractionEventHandler(stopTrackingController);
        }
    }

    private void OnDisable()
    {
        if (controllerEvents)
        {
            controllerEvents.AliasMenuOn -= new ControllerInteractionEventHandler(doToggleInventory);
            controllerEvents.AliasMenuOff -= new ControllerInteractionEventHandler(stopTrackingController);
        }
    }

    private void doToggleInventory(object sender, ControllerInteractionEventArgs e)
    {
        if (isInventoryOpen)
        {
            closeInventory();
        } else
        {
            openInventory();
            startTrackingController();
        }
    }

    private void startTrackingController()
    {
        inventoryPrefab.transform.parent = this.transform;
        inventoryPrefab.transform.position = this.transform.position;
        inventoryPrefab.transform.rotation = this.transform.rotation;
    }

    private void stopTrackingController(object sender, ControllerInteractionEventArgs e)
    {
        inventoryPrefab.transform.parent = null;
    }

    private void openInventory()
    {
        inventoryPrefab.SetActive(true);
        isInventoryOpen = true;
    }

    private void closeInventory()
    {
        inventoryPrefab.SetActive(false);
        isInventoryOpen = false;
    }
}
