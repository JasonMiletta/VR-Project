using UnityEngine;
using VRTK;
using System.Collections;

[RequireComponent(typeof(VRTK_ControllerEvents))]
public class CraftingManager : MonoBehaviour {

    public GameObject craftingPrefab;

    private VRTK_ControllerEvents controllerEvents;
    private bool craftingToggledOn = false;
    private Object craftingObject;

	// Use this for initialization
	void Start () {
        if(craftingPrefab == null)
        {
            Debug.LogError("CraftingManager needs a reference to the craftingPrefab to spawn!");
        }
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnEnable()
    {

        controllerEvents.AliasMenuOn += new ControllerInteractionEventHandler(doToggleCrafting);
    }

    private void OnDisable()
    {
        controllerEvents.AliasMenuOn -= new ControllerInteractionEventHandler(doToggleCrafting);
    }

    private void doToggleCrafting(object sender, ControllerInteractionEventArgs e)
    {
        if (craftingToggledOn)
        {
            closeCrafting();
        } else
        {
            openCrafting();
        }
    }

    private void openCrafting()
    {
        //TODO: Spawn crafting zone object
        craftingObject = Instantiate(craftingPrefab, this.transform.position, this.transform.rotation);
        craftingToggledOn = true;
    }

    private void closeCrafting()
    {
        //TODO: hide crafting zone object
        //TODO: Will need to handle dropping any inputted items
        Destroy(craftingObject);
        craftingToggledOn = false;
    }
}
