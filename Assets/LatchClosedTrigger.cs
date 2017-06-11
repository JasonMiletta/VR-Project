using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatchClosedTrigger : MonoBehaviour {

    public delegate void BoxClosedAction();
    public static event BoxClosedAction OnClosed;

    public ParticleSystem dustEffect;

    //This is the latch we watch for to detect if the latch has been *closed*
    public GameObject Latch;
    public bool isClosed = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Latch)
        {
            Debug.Log("Box Closed!");
            isClosed = true;
            if(dustEffect != null)
            {
                dustEffect.Play();
            }
            OnClosed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Latch)
        {
            Debug.Log("Box Opened!");
            isClosed = false;
        }
    }
}
