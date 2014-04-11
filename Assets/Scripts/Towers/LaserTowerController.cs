using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This is the derived class whis is
// also know as the Child class.
public class LaserTowerController : TowerController 
{

	public ParticleSystem smoke;

	private LineRenderer lineRenderer;
	private ParticleSystem currentSmoke;
	private Queue<ParticleSystem> smokeQueue = new Queue<ParticleSystem>();

	public LaserTowerController()
	{
		//Debug.Log("LaserTowerController Constructor Called");
	}

	void Start() {
		if (gameObject.GetComponent<LineRenderer>() == null) {
			lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
		}
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(2);
		lineRenderer.useWorldSpace = true;
		
		InitLaser();
	}

	public override void Shoot()
	{
		Debug.Log("Laser shoots."); 

		_loadTime += Time.deltaTime;
		
		// get the laser gun point and shoot from that point
		Transform laserGun = transform.FindChild("LaserGun");
		laserGun.localPosition = new Vector3(0, _loadTime/shootSpeed/2, 0);
		
		if (_loadTime >= shootSpeed) {
			if (!_isShooting) {
				
				currentSmoke = Instantiate(smoke, transform.position, Quaternion.Euler(-90,0,0)) as ParticleSystem;
				currentSmoke.loop = true;
				
				if (!currentSmoke.isPlaying) {
					currentSmoke.Play();
					smokeQueue.Enqueue(currentSmoke);
				}
			}
			
			_isShooting = true;
			
			Transform laserEmitter = transform.FindChild("LaserEmitter");
			
		    lineRenderer.SetPosition(0, laserEmitter.position);
			lineRenderer.SetPosition(1, _target.transform.position);

			DamageController dc = _target.GetComponent<DamageController>();
			dc.takeDamage(damagePoints * Time.deltaTime);
			currentSmoke.transform.position = _target.transform.position;
			
			_loadTime = shootSpeed;
		}
	}

	public override void CoolDown() {
		InitLaser();
	}

	public override void CleanUp() {
		Debug.Log ("cleanup smoke");
		if (currentSmoke != null && currentSmoke.isPlaying) {
			currentSmoke.Stop ();
			Destroy(currentSmoke.transform.gameObject, 1.0f);
		}
	}

	void InitLaser() {
		Transform pivot = transform.FindChild("LaserGun");
		pivot.localPosition = Vector3.Lerp (pivot.localPosition, new Vector3(0, _loadTime/shootSpeed/2, 0), 1f * Time.deltaTime);
		_loadTime = 0;
		
		// set begin and end vertex of line to laser zero.
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.zero);
	}



}