using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretController : MonoBehaviour {

	public float turnSpeed = 3.0f;
	public float shootSpeed = 1.0f;
	public GameObject bullet;

	private List<GameObject> _targetsInRange = new List<GameObject>();
	private GameObject _target;
	private bool _isShooting = false;
	private float _reloadTime = 0;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Enemy") {
			Debug.Log(other.gameObject + " enters");

			_targetsInRange.Add(other.gameObject);

			if (_target == null) {
				_target = other.gameObject;
			}

			Debug.Log (_targetsInRange.Count);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Enemy") {
			Debug.Log(other);

			_targetsInRange.Remove(other.gameObject);
			if (other.gameObject == _target) {
				_target = null;
			}
			Debug.Log (_targetsInRange.Count);
		}
	}

	// Update is called once per frame
	void Update () {
		if (_target != null) {
			Quaternion rotation = Quaternion.LookRotation(_target.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);

			Vector3 targetDir = _target.transform.position - transform.position;
			float angle = Vector3.Angle (transform.forward, targetDir);
			if (!_isShooting && angle < 5f) {
				_isShooting = true;
				InvokeRepeating("ShootBullet", 0, shootSpeed);
			}
		} else {
			_targetsInRange.Remove(null);
			Debug.Log (_targetsInRange.Count);
			if (_targetsInRange.Count == 0) {
				Debug.Log("nothing to do...");
			} else {
				_target = _targetsInRange[0];
			}
		}
	}

	void ShootBullet() {

		if (_target == null) {
			_isShooting = false;
			CancelInvoke();
			return;
		}

		// TODO Better way in stead of hardcoded gameObject BulletEmitter
		Transform spawnPoint = transform.FindChild("BulletEmitter");
		
		GameObject bulletInstance = Instantiate(bullet, spawnPoint.position, Quaternion.LookRotation (_target.transform.position)) as GameObject;

		BulletController bc = bulletInstance.GetComponent<BulletController>();
		bc.direction = transform.forward;

		bc.rigidbody.velocity = transform.forward * bc.bulletSpeed;
		bc.isFired = true;

		// TODO Add recoil to gun
		// TODO Better way in stead of hardcoded 
		Transform gun = transform.FindChild ("Gun");

	}

}
