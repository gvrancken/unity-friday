using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter (Collision col) {
		col.gameObject.tag == "Enemy")
		{
			transform.localScale = Vector3(0.5, 0.5, 0.5);
			col.gameObject;
		}
	}
