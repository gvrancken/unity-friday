using UnityEngine;
using System.Collections;

public class ShootController : MonoBehaviour {

	public float turnSpeed = 3.0f;
	public float shootSpeed = 1f;
	public GameObject bullet;

	private GameObject _target;
	private bool _isShooting = false;

	void Start () {
		// get range of sight
		SphereCollider sc = gameObject.GetComponent<SphereCollider>();
	}

	void OnTriggerEnter (Collider other) {
		if (_isShooting == false) {
			_target = other.gameObject;
			ShootEnemy(_target);
		}
	}

	void OnTriggerExit (Collider other) {
		_target = null;
		_isShooting = false;
	}
		
	// Update is called once per frame
	void Update () {
		if (_target != null) {
			//Vector3 lookPoint = Vector3.Lerp (transform.position, _target.transform.position, 10f);
			Quaternion rotation = Quaternion.LookRotation(_target.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
			//transform.LookAt(lookPoint);
		}
	}

	void ShootEnemy(GameObject target) {
		_isShooting = true;
	//	transform.LookAt(target.transform.position);
//		transform.rotation = Vector3.Lerp(transform.rotation, );
	}
}
