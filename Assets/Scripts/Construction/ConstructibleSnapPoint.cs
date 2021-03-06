﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ConstructibleSnapPoint : MonoBehaviour {

    private Collider snapCollider;
    private GameObject highlight;
    private VRTK_ControllerEvents controllerEvents;

	// Use this for initialization
	void Start () {
        snapCollider = GetComponent<BoxCollider>();
        highlight = transform.GetChild(0).gameObject;
        controllerEvents = GetComponentInParent<VRTK_ControllerEvents>();
	}

    private void OnEnable()
    {
        if (controllerEvents)
        {
            controllerEvents.AliasUseOn += new ControllerInteractionEventHandler(doCreateWall);
        }
    }

    private void OnDisable()
    {
        if (controllerEvents)
        {
            controllerEvents.AliasUseOn -= new ControllerInteractionEventHandler(doCreateWall);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<VRTK_PlayerObject>() && other.GetComponent<VRTK_PlayerObject>().objectType == VRTK_PlayerObject.ObjectTypes.Pointer)
        {

            GameObject pointerObject = other.transform.parent.gameObject;
            highlight.SetActive(true);
            if (pointerObject.GetComponentInChildren<ConstructableHighlight>() != null) { 
                GameObject pointerHighlight = pointerObject.GetComponentInChildren<ConstructableHighlight>().gameObject;
                pointerHighlight.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<VRTK_PlayerObject>() && other.GetComponent<VRTK_PlayerObject>().objectType == VRTK_PlayerObject.ObjectTypes.Pointer)
        {
            GameObject pointerObject = other.transform.parent.gameObject;
            highlight.SetActive(false);
            if (pointerObject.GetComponentInChildren<ConstructableHighlight>() != null)
            {
                GameObject pointerHighlight = pointerObject.GetComponentInChildren<ConstructableHighlight>().gameObject;
                
                pointerHighlight.SetActive(true);
            }
        }
    }

    void doCreateWall(object sender, ControllerInteractionEventArgs e)
    {
        if (highlight.activeSelf)
        {
            GameObject newConstruct = GetComponentInParent<Constructable>().constructObject;
            Instantiate(newConstruct, transform.position, transform.rotation, transform.parent);
        }
    }
}
