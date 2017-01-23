using UnityEngine;
using System.Collections;

public class Ignitable : MonoBehaviour
{
    public bool isOnFire = false;

    GameObject fireComponent;

    void Start()
    {
        Fire fireScript = GetComponentInChildren<Fire>(true);
        if (fireScript != null) {
            fireComponent = fireScript.gameObject;
        }
        else { 
            Debug.LogWarning("This Object is ignitable but missing a component with an attached fire script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnFire)
        {
            fireComponent.SetActive(true);
        } else
        {
            fireComponent.SetActive(false);
        }
    }

    public void ignite()
    {
        isOnFire = true;
    }
}