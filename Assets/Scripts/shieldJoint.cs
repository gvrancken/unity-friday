using UnityEngine;
using System.Collections;

public class shieldJoint : MonoBehaviour {

	private bool mouseDown = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var hit : RaycastHit;
		var ray : Ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, hit, 100.0)){

		}
	}
}
