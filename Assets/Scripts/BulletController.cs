using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public bool isFired = false;
	public float bulletSpeed = 30f;
	public Vector3 direction; 
	public float damagePoints = 100f;

	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate() {
		if (isFired) {
			rigidbody.AddForce (direction * bulletSpeed);
		}
	}

	void OnCollisionEnter (Collision col) {
		//Debug.Log (col.gameObject.name);
		if (col.gameObject.tag != "Bullet") {
			DamageController dc = col.gameObject.GetComponent<DamageController>();
			dc.takeDamage(damagePoints);
			Destroy (gameObject);
		}

	}

}
