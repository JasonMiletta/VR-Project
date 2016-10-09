using UnityEngine;
using System.Collections;

public class selfDestruct : MonoBehaviour {
    public float lifetime = 5.0f;

	// Use this for initialization
	void Start () {
	}

    void awake()
    {
        var gameObj = this.gameObject;

        StartCoroutine(FadeTo(0.0f, lifetime));
        Debug.Log("Destroy");
        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
    }
 
     IEnumerator FadeTo(float aValue, float aTime)
    {
    float alpha = this.GetComponentInParent<Renderer>().material.color.a;
    for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
    {
        Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
        this.GetComponentInParent<Renderer>().material.color = newColor;
        yield return null;
    }
}
}
