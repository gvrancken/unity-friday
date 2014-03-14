using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TurretController : MonoBehaviour {

	public float turnSpeed = 3.0f;
	public float shootSpeed = 1.0f;
	public GameObject bullet;

	private List<GameObject> targetsInRange = new List<GameObject>();
	private GameObject target;
	private bool canShoot = false;
	private bool isShooting = false;
	private float reloadTime = 0;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Enemy") {

			targetsInRange.Add(other.gameObject);

			if (target == null) {
				target = other.gameObject;
			}

		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Enemy") {

			targetsInRange.Remove(other.gameObject);
			if (other.gameObject == target) {
				target = null;
			}
		}
	}

	// Update is called once per frame
	void Update () {

		reloadTime += Time.deltaTime;
		if (reloadTime >= shootSpeed) {
			canShoot = true;
		}

		if (target != null) {

			Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);
			transform.rotation = rotation;

			Vector3 targetDir = target.transform.position - transform.position;
			float angle = Vector3.Angle (transform.forward, targetDir);

			if (angle < 5f && canShoot) {
				ShootBullet();
			}
		} else {
			targetsInRange.Remove(null);

			if (targetsInRange.Count > 0) {
				target = targetsInRange[0];
			}
		}

	}

	void ShootBullet() {
		canShoot = false;
		reloadTime = 0;

		Transform spawnPoint = transform.FindChild("BulletEmitter");
		
		GameObject bulletInstance = Instantiate(bullet, spawnPoint.position, Quaternion.LookRotation (target.transform.position)) as GameObject;

		BulletController bc = bulletInstance.GetComponent<BulletController>();
		bc.direction = transform.forward;

		bc.rigidbody.velocity = transform.forward * bc.bulletSpeed;
		bc.isFired = true;

		// TODO Add recoil to gun
		// TODO Better way in stead of hardcoded 
		Transform gun = transform.FindChild ("Gun");

	}

}
