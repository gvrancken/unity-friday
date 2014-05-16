using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public bool isFired = false;
	public float bulletSpeed = 30f;
	public float bulletLifeTime = 1f;
	public Vector3 direction; 
	public float damagePoints = 100f;
	private float _lifeTimeCounter = 0;

	// Use this for initialization
	void Start () {
		Debug.Log(gameObject + " fired");
	}

	void Update() {
		_lifeTimeCounter += Time.deltaTime;
		if (_lifeTimeCounter >= bulletLifeTime) {
			Explode ();
		}
	}
	
	void FixedUpdate() {
		if (isFired) {
			rigidbody.AddForce (direction * bulletSpeed);
		}
	}

	void OnCollisionEnter (Collision col) {
		//Debug.Log (col.gameObject.name);
		if (col.gameObject.GetComponent<DamageController>() != null &&
		    col.gameObject.tag != "Bullet") {
			DamageController dc = col.gameObject.GetComponent<DamageController>();
			dc.takeDamage(damagePoints);
			Destroy (gameObject);
		}

	}

	void Explode () {
		Destroy (gameObject);
	}

}
