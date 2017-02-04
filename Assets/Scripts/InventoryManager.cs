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
            Debug.LogError("InventoryManager needs a reference to the inventoryPrefab to spawn!");
        }
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnEnable()
    {
        controllerEvents.AliasMenuOn += new ControllerInteractionEventHandler(doToggleInventory);
    }

    private void OnDisable()
    {
        controllerEvents.AliasMenuOn -= new ControllerInteractionEventHandler(doToggleInventory);
    }

    private void doToggleInventory(object sender, ControllerInteractionEventArgs e)
    {
        if (isInventoryOpen)
        {
            closeInventory();
        } else
        {
            openInventory();
        }
    }

    private void openInventory()
    {
        isInventoryOpen = true;
    }

    private void closeInventory()
    {
        isInventoryOpen = false;
    }
}
