using UnityEngine;
using System.Collections;

public class DestructibleCube : MonoBehaviour {
    public GameObject fragmentObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void killCube()
    {
        Destroy(Instantiate(fragmentObject, this.transform.position, this.transform.rotation), 5.0f);
        Destroy(this.gameObject);
    }
}
