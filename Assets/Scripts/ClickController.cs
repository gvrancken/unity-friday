using UnityEngine;
using System.Collections;

public class ClickController : MonoBehaviour {
	public Transform _constructionGhost; 

	private GameObject _player;
	private ShieldManager _shieldManager;
	private GameObject _lm;
	private LevelManager _lmscript;
	private bool ghostBuildableState;
	private Vector3 _v = new Vector3();


	// Use this for initialization
	void Start () {
		_player = GameObject.Find ("Player");
		_shieldManager = _player.GetComponent<ShieldManager> ();
		_lm = GameObject.Find ("LevelManager");
		_lmscript = _lm.GetComponent<LevelManager>();
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere (_v, 2);
		//print (_v);
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
			Rect rect = new Rect(Screen.width-250, Screen.height-100, 250, 100);	
			if (!rect.Contains(Input.mousePosition)){
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
				} else if (Input.GetMouseButtonUp(1)) {
					//right-click = cancel build-mode, deselect construction unit, disable construction ghost
					_lmscript.selectedBuildConstruct = null;
					_constructionGhost.GetComponent<ConstructionGhost>().SetConstructionType(ConstructionType.Empty);

				} else {

					//MouseOver, checks every frame if a construction can be build and updates the construction ghost accordingly.
					if ((hit.transform.tag == "Buildable") && (!ghostBuildableState)) {
						_constructionGhost.GetComponent<ConstructionGhost>().setGhostBuildableEnabled(true);
						ghostBuildableState = true;
						print ("canbuild");

					} else if ((hit.transform.tag != "Buildable") && (ghostBuildableState)){
						_constructionGhost.GetComponent<ConstructionGhost>().setGhostBuildableEnabled(false);
						ghostBuildableState = false;
					}

					switch (hit.transform.tag) {
					case "ShieldJoint":
						hit.transform.gameObject.GetComponent<ShieldJoint>().OnMouseOver();
						break;
					default:
						break;
					}

					//Update position of the construction ghost to where the mousepointer hits the 'floor'.
					if (GetComponent<LevelManager>().selectedBuildConstruct != null){
						_constructionGhost.position = new Vector3(hit.point.x, 0, hit.point.z);
					}
				}
			} else {
				if (GetComponent<LevelManager>().selectedBuildConstruct != null){
					_constructionGhost.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width-250, 50,0));
				}
			}

		}
		  
			
	}
}
