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
		private float unitUnloadDelay = 0.1f;

		// Use this for initialization
		void Start ()
		{
				podSize = transform.localScale.magnitude;
				hasLanded = true;
				hasUnloaded = false;
				isUnloading = false;

				//transform.Translate(Vector3.up * 1);

				
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

	private static Vector2 PointOnCircle(float radius, float angleInDegrees, Vector2 origin)
	{
		// Convert from degrees to radians via multiplication by PI/180        
		float x = (radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180f)) + origin.x;
		float y = (radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180f)) + origin.y;
		
		return new Vector2(x, y);
	}

		void UnloadPod ()
		{
				isUnloading = true;
				InvokeRepeating ("UnloadUnit", 0.01f, unitUnloadDelay);
		}

		void UnloadUnit ()
		{

		float randomAngle = Random.Range(0,360);

		Vector2 spawnPoint2D = PointOnCircle(podSize, randomAngle, Vector2.zero);
		Vector3 spawnPoint = new Vector3(spawnPoint2D.x, 0, spawnPoint2D.y);

				if (unitQueue.Count > 0) {
						timeSinceLastUnload += Time.deltaTime;
						if (timeSinceLastUnload >= unitUnloadDelay) {

								Transform unit = unitQueue.Dequeue ();
		
								GameObject instance = Instantiate (unit, spawnPoint+transform.position, Quaternion.identity) as GameObject;
								
								timeSinceLastUnload = 0;
						}
				} else {
					
						isUnloading = false;
						CancelInvoke ();
						hasUnloaded = true;
				}
				
		}
}
