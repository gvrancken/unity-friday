using UnityEngine;
using System.Collections;

public class ClickController : MonoBehaviour {
	private GameObject _player;
	private ShieldManager _shieldManager;
	private Transform 	_constructionGhost; 

	// Use this for initialization
	void Start () {
		_player = GameObject.Find ("Player");
		_shieldManager = _player.GetComponent<ShieldManager> ();
	}

	public void SetConstructionGhost(Transform ghost){
		_constructionGhost = ghost;
	}
	
	// Update is called once per frame
	void Update () {
		// create a ray going into the scene from the screen location the user clicked at
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		
		// the raycast hit info will be filled by the Physics.Raycast() call further
		RaycastHit hit;
		
		// perform a raycast using our new ray. 
		// If the ray collides with something solid in the scene, the "hit" structure will
		// be filled with collision information
	
		if( Physics.Raycast( ray, out hit ) )
		// a collision occured. Check it.
		{
			if (Input.GetMouseButtonUp(0)) {

				switch (hit.transform.tag) {
				case "Tower":
					break;
				case "ShieldJoint" :
					hit.transform.GetComponent<ShieldJoint>().OnClick(); 
					break;
				case "Buildable":
					Transform unit = GetComponent<LevelManager>().selectedBuildConstruct;
					if (unit != null) {
						if (_shieldManager.GetEnergy() >= 50){
							Vector3 buildPoint = new Vector3(hit.point.x, 0, hit.point.z);
							Instantiate(unit, buildPoint, Quaternion.identity);
							_shieldManager.AddEnergy (-50);
						}
					}
					break;
				case "GUIButton":
					hit.transform.GetComponent<Button>().OnClick(); 
					break;
				default:
					break;
				}
			} else {
				//MouseOver
				switch (hit.transform.tag) {
				case "Buildable":
					if (GetComponent<LevelManager>().selectedBuildConstruct != null){
						_constructionGhost.position = hit.point;
					}
					break;
				default:
					break;
				}
			}

		}

			
	}
}
