using UnityEngine;
using System.Collections;

public class LineOfSight : MonoBehaviour {

	public bool CanSeePoint(Vector3 tempPoint) {
		return !Physics.Linecast(transform.position, tempPoint);
	}

//	public bool ObjectsOnPath(Vector3 tempPoint) {
//		RaycastHit[] hits;
//		Ray ray = new Ray(transform.position, (tempPoint-transform.position));
//		hits = Physics.RaycastAll(ray);
//		int i = 0;
//		while (i < hits.Length) {
//			RaycastHit hit = hits[i];
//			i++;
//			print ("hit");
//			Debug.Log (hit.transform.gameObject);
//		}
//		return true;
//	}
	
}
