using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemy;
    public bool activated;
    public float spawnTime = 5.0f;
    private float currentTime;

	// Use this for initialization
	void Start () {
        currentTime = spawnTime;
    }

    void Awake()
    {
        spawnObject();
    }
	
	// Update is called once per frame
	void Update () {
        currentTime = currentTime - Time.deltaTime;       
        if(activated && currentTime < 0)
        {
            spawnObject();
            currentTime = spawnTime;
        }
	}

    private void spawnObject()
    {
        Debug.Log("Spawning Object");
        Instantiate(enemy, this.transform.position, this.transform.rotation);
    }
}
