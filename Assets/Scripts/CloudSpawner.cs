using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour {

    public GameObject cloudPrefab;
    public float cloudSpawnRate = 1f;
    public float cloudSpeed = 10f;
    public float cloudLifeTime = 10f;
    public float cloudSpawnDistanceRange = 200; 

    private float cooldown = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(cooldown <= 0f)
        {
            float xCoord = Random.Range(-cloudSpawnDistanceRange, cloudSpawnDistanceRange);
            GameObject newCloud = (GameObject) Instantiate(cloudPrefab, this.transform, false);

            newCloud.transform.position += new Vector3(xCoord, 0, 0);
            Rigidbody body = newCloud.GetComponent<Rigidbody>();
            body.velocity = new Vector3(0, 0, cloudSpeed);
            Destroy(newCloud, cloudLifeTime);

            cooldown = cloudSpawnRate;
        } else
        {
            cooldown = cooldown - Time.deltaTime;
        }
	}
}
