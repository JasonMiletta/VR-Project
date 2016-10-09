using UnityEngine;
using System.Collections;

public class EnemyChase : MonoBehaviour {

    public float speed = 5.0f;

    private Vector3 target;

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.DrawRay(this.transform.position, target - this.transform.position);
        this.transform.LookAt(target);
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);     
	}

    void onTriggerEnter(Collider col)
    {
            Debug.Log("Enemy chase collided");
            Destroy(this.gameObject);
        
    }
}
