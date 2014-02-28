using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PodController : MonoBehaviour
{
		public Queue<Transform> unitQueue = new Queue<Transform> ();
		public float timeBeforeUnload = 1f;
		private bool hasLanded;
		private bool hasUnloaded;
		private bool isUnloading;
		private float timeSinceLanded = 0f;
		private float timeSinceLastUnload = 0f;
		private float podSize;
		private float unitUnloadDelay = 0.05f;

		// Use this for initialization
		void Start ()
		{
				podSize = transform.localScale.magnitude;
				hasLanded = true;
				hasUnloaded = false;
				isUnloading = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (hasLanded) {
						if (isUnloading) {
							
						} else if (!hasUnloaded && !isUnloading) {
								timeSinceLanded += Time.deltaTime;
				if (timeSinceLanded >= timeBeforeUnload) {
										UnloadPod ();
								}
						} else {
								Destroy (gameObject);
						}
				}
		}

		void UnloadPod ()
		{
				isUnloading = true;
				InvokeRepeating ("UnloadUnit", 0.01f, unitUnloadDelay);
		}

		void UnloadUnit ()
		{
;
				if (unitQueue.Count > 0) {
						timeSinceLastUnload += Time.deltaTime;
						if (timeSinceLastUnload >= unitUnloadDelay) {

								Transform unit = unitQueue.Dequeue ();

								float unitSize = unit.localScale.sqrMagnitude;
								Vector3 heading = Vector3.zero - transform.position;
								Vector3 direction = heading / (heading.magnitude);
		
								Vector3 spawnPoint = transform.position + (direction * (podSize + unitSize));
		
								GameObject instance = Instantiate (unit, spawnPoint, Quaternion.identity) as GameObject;
								timeSinceLastUnload = 0;
						}
				} else {
					Debug.Log (timeSinceLastUnload);
						isUnloading = false;
						CancelInvoke ();
						hasUnloaded = true;
				}
				
		}
}
