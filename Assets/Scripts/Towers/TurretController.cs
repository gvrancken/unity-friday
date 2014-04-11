using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TurretController : TowerController {

	public GameObject bullet;

	private bool canShoot = true;

	public TurretController()
	{
		//Debug.Log("TurretController Constructor Called");
	}

	override public void Shoot() 
	{
		if (timeSinceLastShot >= shootSpeed) canShoot = true;
		if (!canShoot) return;

		canShoot = false;
		timeSinceLastShot = 0;


		Transform spawnPoint = transform.FindChild("BulletEmitter");
		
		GameObject bulletInstance = Instantiate(bullet, spawnPoint.position, Quaternion.LookRotation (_target.transform.position)) as GameObject;

		BulletController bc = bulletInstance.GetComponent<BulletController>();
		bc.direction = transform.forward;

		bc.rigidbody.velocity = transform.forward * bc.bulletSpeed;
		bc.isFired = true;
	}

}
