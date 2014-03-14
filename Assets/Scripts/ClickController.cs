using UnityEngine;
using System.Collections;

public class ClickController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonUp(0)) {
			// create a ray going into the scene from the screen location the user clicked at
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			
			// the raycast hit info will be filled by the Physics.Raycast() call further
			RaycastHit hit;
			
			// perform a raycast using our new ray. 
			// If the ray collides with something solid in the scene, the "hit" structure will
			// be filled with collision information
			if( Physics.Raycast( ray, out hit ) )
			{
				// a collision occured. Check it.
				// Debug.Log ("clicked: " + hit.transform.gameObject);

				switch (hit.transform.tag) {
				case "Tower":
					hit.transform.GetComponent<LaserTowerScript>().isSelected = true;
					break;
				case "ShieldJoint" :
					hit.transform.GetComponent<ShieldJoint>().OnClick(); 
					break;
				case "Buildable":
					Debug.Log("build here:  " + hit.point);
					break;
				default:
					break;
				}


			}

		}

			
	}
}
