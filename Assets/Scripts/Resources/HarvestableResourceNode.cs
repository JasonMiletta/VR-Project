using UnityEngine;

public class HarvestableResourceNode : MonoBehaviour {

    public GameObject resource;
    public float nodeHealth;
    public float dropCooldown = 1.0f;

    private float _currentDropCooldown = 0.0f;

    public void Start()
    {

    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Hammer")
        {
            harvestedHit(col);
        }
    }

    private void harvestedHit(Collision col)
    {
        if (Time.time > _currentDropCooldown)
        {
            GameObject newItem = GameObject.Instantiate(resource);
            newItem.transform.position = col.transform.position;
            _currentDropCooldown = Time.time + dropCooldown;
        }
    }
}
