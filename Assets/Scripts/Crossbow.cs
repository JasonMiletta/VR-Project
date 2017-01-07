using UnityEngine;
using System.Collections;
using VRTK;

public class Crossbow : VRTK_InteractableObject {

    public GameObject boltLoadPoint;
    public float projectileSpeed = 10000.0f;
    public GameObject loadedBolt;

    bool _isBoltLoaded = true;
    Vector3 _boltLoadPointLocation;

	// Use this for initialization
	protected void Start () {

        if(boltLoadPoint != null)
        {
            _boltLoadPointLocation = boltLoadPoint.transform.position;
        }
        else 
        {
            Debug.Log("Bolt Load Point reference must be set!");
        }

        if (boltLoadPoint.transform.GetChild(0) != null)
        {
            loadedBolt = boltLoadPoint.transform.GetChild(0).gameObject;
            if(loadedBolt != null)
            {
                _isBoltLoaded = true;
            } else
            {
                _isBoltLoaded = false;
            }
        }
        
	}

    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        fireProjectile();
    }

    public override void OnInteractableObjectUsed(InteractableObjectEventArgs e)
    {
        base.OnInteractableObjectUsed(e);
        fireProjectile();
    }

    private void fireProjectile()
    {
        if (_isBoltLoaded)
        {
            GameObject boltCopy = Instantiate(loadedBolt, loadedBolt.transform.position, loadedBolt.transform.rotation) as GameObject;
            
            Rigidbody rb = boltCopy.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;

            boltCopy.SetActive(true);
            rb.AddForce(boltCopy.transform.forward * projectileSpeed);
            Destroy(boltCopy, 5.0f);

            //Destroy(loadedBolt);
            //_isBoltLoaded = false;
        }
    }
}
