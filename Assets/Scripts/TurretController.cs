using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour {

	public float turnSpeed = 3.0f;
	public float shootSpeed = 30.0f;
	public GameObject bullet;

	private GameObject _target;
	private bool _isShooting = false;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Enemy") {
			if (_isShooting == false) {
				_target = other.gameObject;
			}
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
				InvokeRepeating("ShootBullet", 0, 1);
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
