using UnityEngine;
using System.Collections;

public class BuildManager : MonoBehaviour {

	public GameObject tower;

	private bool _canBuild = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;

		if( Physics.Raycast( ray, out hit, 100 )) {
			_canBuild = true;
		}

		if (_canBuild) {
		
			// TODO: show what is going to be build at cursor position
			if (Input.GetButtonUp("Fire1")) {
				if (hit.transform.gameObject.tag == "Buildable") {
					Instantiate (tower, new Vector3(hit.point.x, 0, hit.point.z), Quaternion.Euler(0, Random.Range (0,360), 0));
				}
			}
		}

	}
	
}
