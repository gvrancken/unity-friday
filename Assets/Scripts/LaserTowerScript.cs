﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserTowerScript : MonoBehaviour {

	public float turnSpeed = 30.0f;
	public float shootSpeed = 1.0f;
	public float damagePoints = 10;
	public GameObject bullet;
	
	private List<GameObject> _targetsInRange = new List<GameObject>();
	private GameObject _target;
	private bool _isShooting = false;
	private float _loadTime = 0;
	private LineRenderer lineRenderer;

	void Start() {
		lineRenderer = transform.GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(2);
		InitLaser();
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Enemy") {
			
			_targetsInRange.Add(other.gameObject);
			
			if (_target == null) {
				_target = other.gameObject;
			}
			
			Debug.Log (_targetsInRange.Count);
		}
	}
	
	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Enemy") {
			
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
			transform.rotation = rotation;

			FireLaser();

		} else {

			_targetsInRange.Remove(null);
			
			if (_targetsInRange.Count > 0) {
				_target = _targetsInRange[0];
			}

			InitLaser();
		} 

	}

	void InitLaser() {
		_loadTime = 0;
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.zero);
	}
	
	void FireLaser() {
		_loadTime += Time.deltaTime;

		if (_loadTime >= shootSpeed) {
			Transform laserEmitter = transform.FindChild("LaserEmitter");
			lineRenderer.useWorldSpace = true;
			lineRenderer.SetPosition(0, laserEmitter.position);
			lineRenderer.SetPosition(1, _target.transform.position);
			DamageController dc = _target.GetComponent<DamageController>();
			dc.takeDamage(damagePoints * Time.deltaTime);

		}

	}

}