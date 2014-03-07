using UnityEngine;
using System.Collections;

public class shieldJoint : MonoBehaviour {

	private bool mouseDown = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		RaycastHit hit;
//		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//		if (Physics.Raycast (ray, out hit, 100.0)){
//
//		}
	}

	void OnMouseDown() {
		print("Mousdown");

	}
}
