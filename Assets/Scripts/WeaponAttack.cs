using UnityEngine;
using System.Collections;

public class WeaponAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Hit");
        var obj = col.gameObject.GetComponent<DestructibleCube>();
        if(obj != null)
        {
            obj.killCube();
        }
    }
}
