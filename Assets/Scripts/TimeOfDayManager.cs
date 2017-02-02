using UnityEngine;
using System.Collections;

public class TimeOfDayManager : MonoBehaviour {

    public float dayTime = 60f;

    private float tickAngle;

	// Use this for initialization
	void Start () {
        tickAngle = dayTime / 360f;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(new Vector3(0, 1), tickAngle);
	}
}
