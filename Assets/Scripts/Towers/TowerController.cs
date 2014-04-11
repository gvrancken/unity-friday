using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This is the base class which is
//also known as the Parent class.
public class TowerController : MonoBehaviour
{

	public float turnSpeed = 30.0f;
	public float shootSpeed = 1.0f;
	public float damagePoints = 1.0f;


	protected float _loadTime = 0;
	protected bool _isShooting = false;
	protected GameObject _target;
	protected List<GameObject> _targetsInRange = new List<GameObject>();
	
	// This is the first constructor for the Fruit class
	// and is not inherited by any derived classes.
	public TowerController()
	{
		Debug.Log("TowerController Constructor Called");
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
				Shoot ();
			} else {
				_target = null;
			}
		} else {
			_isShooting = false;
			_targetsInRange.Remove(null);
			CleanUp();
			// if there are units in sight, pick the first in line.
			if (_targetsInRange.Count > 0) {
				
				// raycast
				for (int i=0; i<_targetsInRange.Count; i++) {
					GameObject tempTarget = _targetsInRange[i];
					if (tempTarget != null) {
						if (IsTargetInSight(tempTarget)) {
							_target = tempTarget;
						} 
					}
				}
			} 
			if (_target == null) {
				CoolDown();
			}
		} 
		
	}

	public virtual void CleanUp() {
		Debug.Log ("base: cleanup");
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

	public virtual void Shoot()
	{
		Debug.Log("Base: Shoot.");     
	}

	public virtual void CoolDown() 
	{
		Debug.Log("Base: Cooling Down.");  
	}
	
	public void Rotate()
	{
		Debug.Log("Hello, I am a fruit.");
	}




}