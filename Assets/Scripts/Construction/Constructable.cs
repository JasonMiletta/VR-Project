using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Constructable : VRTK_InteractableObject{

    public GameObject constructObject;

    private ConstructionPointer constructionPointer;

    void Start()
    {
        constructionPointer = GetComponent<ConstructionPointer>();
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        if (constructionPointer.IsActive())
        {
            if (constructObject != null)
            {
                GameObject constructedObject = Instantiate(constructObject, constructionPointer.pointerTip.transform.position, constructionPointer.pointerTip.transform.rotation);
                
            }
        }
        base.Ungrabbed(previousGrabbingObject);
        
    }

    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        //TODO: if pointer is on, place object
        constructionPointer.ToggleBeam(true);
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);
        constructionPointer.ToggleBeam(false);
    }

}
