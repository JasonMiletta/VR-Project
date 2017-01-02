using UnityEngine;
using System.Collections;

public class CrossbowManager : MonoBehaviour {

    GameObject boltLoadPoint;
    float projectileSpeed = 10.0f;

    GameObject _loadedBolt;
    bool _isBoltLoaded;
    Vector3 _boltLoadPointLocation;

	// Use this for initialization
	void Start () {
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
            _loadedBolt = boltLoadPoint.transform.GetChild(0).gameObject;
            if(_loadedBolt != null)
            {
                _isBoltLoaded = true;
            } else
            {
                _isBoltLoaded = false;
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void fireProjectile()
    {
        if (_isBoltLoaded)
        {
            GameObject boltCopy = _loadedBolt;
            Destroy(_loadedBolt);

            GameObject firedProjectile = GameObject.Instantiate(boltCopy);
            firedProjectile.GetComponent<Rigidbody>().velocity = Vector3.forward * projectileSpeed;
        }
    }
}
