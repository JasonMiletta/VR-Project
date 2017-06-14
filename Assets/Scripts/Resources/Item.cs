using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public enum ItemType {Weapon, Tool, Rock, Wood, Stick};

    [SerializeField]
    public ItemType itemType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
