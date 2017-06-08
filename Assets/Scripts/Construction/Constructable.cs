using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Constructable : VRTK_InteractableObject{

    [Header("Constructible Settings", order = 4)]
    public GameObject constructObject;
    public List<GameObject> Anchors;
    

    void Start()
    {
    }

    public override void Grabbed(GameObject currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        currentGrabbingObject.GetComponent<ConstructionPointer>().showConstructHighlight = true;
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        previousGrabbingObject.GetComponent<ConstructionPointer>().showConstructHighlight = false;

    }
    /*
    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        //TODO: if pointer is on, place object
        usingObject.GetComponent<ConstructionPointer>().ToggleBeam(true);
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);
        previousUsingObject.GetComponent<ConstructionPointer>().ToggleBeam(false);
    }*/

}
