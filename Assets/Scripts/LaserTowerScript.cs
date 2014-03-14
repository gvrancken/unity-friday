using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserTowerScript : MonoBehaviour {

	public float turnSpeed = 30.0f;
	public float shootSpeed = 1.0f;
	public float damagePoints = 10;
	public ParticleSystem smoke;

	public bool isSelected = false;
	
	private List<GameObject> _targetsInRange = new List<GameObject>();
	private GameObject _target;
	private bool _isShooting = false;
	private float _loadTime = 0;
	private LineRenderer lineRenderer;
	private ParticleSystem currentSmoke;
	private Queue<ParticleSystem> smokeQueue = new Queue<ParticleSystem>();

	void Start() {
		lineRenderer = transform.GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(2);
		lineRenderer.useWorldSpace = true;

		InitLaser();
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Enemy") {
			_targetsInRange.Add(other.gameObject);
		}
	}
	
	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Enemy") {
			_targetsInRange.Remove(other.gameObject);
			if (other.gameObject == _target) {
				_target = null;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		// if there is a target, rotate to it and shoot.
		if (_target != null) {
			if (IsTargetInSight(_target)) {
				Quaternion rotation = Quaternion.LookRotation(_target.transform.position - transform.position);
				// laserTower immediately rotates to desired point
				transform.rotation = rotation;

				FireLaser();
			} else {
				_target = null;
			}
		} else {

			if (currentSmoke != null && currentSmoke.isPlaying) {
				currentSmoke.Stop ();
				Destroy(currentSmoke.transform.gameObject, 1f);
			}

			_isShooting = false;
			_targetsInRange.Remove(null);

			// if there are units in sight, pick the first in line.
			if (_targetsInRange.Count > 0) {

				// raycast
				for (int i=0; i<_targetsInRange.Count; i++) {
					GameObject tempTarget = _targetsInRange[i];
					if (IsTargetInSight(tempTarget)) {
						_target = tempTarget;
					} 
				}
			} 
			if (_target == null) {
				InitLaser();
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
				_target = tempTarget;
				return true;
			}
		}

		return false;
	}

	void InitLaser() {
		Transform pivot = transform.FindChild("LaserGun");
		pivot.localPosition = Vector3.Lerp (pivot.localPosition, new Vector3(0, _loadTime/shootSpeed/2, 0), 1f * Time.deltaTime);
		_loadTime = 0;

		// set begin and end vertex of line to laser zero.
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.zero);
	}



	
	void FireLaser() {
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

}
