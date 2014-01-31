using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 0.5f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed;
		float moveVertical = Input.GetAxis("Vertical") * moveSpeed;
		transform.Translate(new Vector3(moveHorizontal, 0, moveVertical));
		
	}
}
