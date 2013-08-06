using UnityEngine;
using System.Collections;

public class test1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	//asdfjasldfjlsdfjldsf
	}
	
	// Update is called once per frame
	void Update () {
	Vector3 rotationVelocity = new Vector3(45, 90, 1);
    transform.Rotate(rotationVelocity * Time.deltaTime);
	}
}
