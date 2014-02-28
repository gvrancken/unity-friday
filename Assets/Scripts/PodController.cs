using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PodController : MonoBehaviour {

	public List<Transform> unitList = new List<Transform>();
	public float timeToUnload = 2f;

	private bool hasLanded;
	private bool hasUnloaded;
	private float timeSinceLanded = 0;
	private float podSize;

	// Use this for initialization
	void Start () {
		podSize = transform.localScale.sqrMagnitude;
		hasLanded = true;
		hasUnloaded = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (hasLanded) {
			if (!hasUnloaded) {
				timeSinceLanded += Time.deltaTime;
				if (timeSinceLanded >= timeToUnload) {
					UnloadPod();
				}
			} else {
				Destroy(gameObject);
			}
		}


	}

	void UnloadPod() {

		Transform unit = unitList[0];

		Vector3 heading = Vector3.zero - transform.position;
		Vector3 direction = heading / (heading.magnitude);

		Debug.Log (direction);

		Vector3 spawnPoint = transform.position + (direction * podSize);

		GameObject instance = Instantiate(unit, spawnPoint, Quaternion.identity) as GameObject;

		hasUnloaded = true;


	}
}
