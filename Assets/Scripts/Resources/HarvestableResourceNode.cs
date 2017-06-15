using UnityEngine;

public class HarvestableResourceNode : MonoBehaviour {

    public GameObject resource;
    public float dropCooldown = 1.0f;

    public GameObject replacementResource;
    public Mesh[] replacementResources;

    private float _currentDropCooldown = 1.0f;
    private float currentHealth;

    public void Start()
    {
        currentHealth = replacementResources.Length - 1;
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Item>() != null && col.gameObject.GetComponent<Item>().itemType == Item.ItemType.Tool)
        {
            harvestedHit(col);
        }
    }

    private void harvestedHit(Collision col)
    {
        if (Time.time > _currentDropCooldown && currentHealth > 0)
        {
            GameObject newItem = GameObject.Instantiate(resource);
            newItem.transform.position = col.transform.position;
            _currentDropCooldown = Time.time + dropCooldown;
            --currentHealth;
            swapReplacementResource();
        }
    }

    private void swapReplacementResource()
    {
        GetComponent<MeshFilter>().mesh = replacementResources[(int)currentHealth];
        GetComponent<MeshCollider>().sharedMesh = replacementResources[(int)currentHealth];
        if (currentHealth == 0)
        {
            Debug.Log("0!!!!");
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
