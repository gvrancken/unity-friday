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
			if (IsTargetInSight(target)) {
				Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);
				transform.rotation = rotation;

				Vector3 targetDir = target.transform.position - transform.position;
				float angle = Vector3.Angle (transform.forward, targetDir);

				if (angle < 5f && canShoot) {
					ShootBullet();
			} else {
					target = null;
				}
			}
		} else {
			targetsInRange.Remove(null);

			if (targetsInRange.Count > 0) {
				target = targetsInRange[0];
			}
		}

	}

	bool IsTargetInSight(GameObject tempTarget) {
		Ray ray = new Ray(transform.position, (tempTarget.transform.position - transform.position));
		
		// the raycast hit info will be filled by the Physics.Raycast() call further
		RaycastHit hit;
		
		// perform a raycast using our new ray. 
		// If the ray collides with something solid in the scene, the "hit" structure will
		// be filled with collision information
		if( Physics.Raycast( ray, out hit ) )
		{
			// a collision occured. Check it.
			if (hit.transform.gameObject == tempTarget) {
				target = tempTarget;
				return true;
			}
		}
		
		return false;
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

	}

}
