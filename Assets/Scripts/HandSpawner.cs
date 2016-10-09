using UnityEngine;
using System.Collections;
using VRTK;
using UnityEngine.Events;

public class HandSpawner : MonoBehaviour {
    public float cooldown = 1.0f;

    public GameObject[] spawnObjects;
    public VRTK_ControllerEvents hand;
    

    private void Awake()
    {
        hand = this.GetComponent<VRTK_ControllerEvents>();
    }

    private void Update()
    {

        if (hand.touchpadPressed && Time.deltaTime >= cooldown)
        {
            cooldown = 1.0f;
        } else
        {
            cooldown -= Time.deltaTime;
        }
    }
    
    public void spawnObject(int index)
    {
        GameObject newObject = GameObject.Instantiate(spawnObjects[index]);
        newObject.transform.position = this.transform.position;
        newObject.transform.rotation = this.transform.rotation;

        Rigidbody rb = newObject.GetComponent<Rigidbody>();

        //newObject.transform.localScale = obj.transform.lossyScale;
    }

}