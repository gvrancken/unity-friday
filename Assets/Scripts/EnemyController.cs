using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float damagePoints = 10;

	// Perhaps more players will join the arena.
	// This enemy will choose at random which player to attack.
	private GameObject[] _players; 
	private GameObject _chosenPlayer; 
	private Vector3 endPosition;
	private ShieldManager sm;
	public float moveSpeed = 5f;
	private DebugText _dt;

	void Start () {
		// Fill _players array with all object with Player tag
		_players = GameObject.FindGameObjectsWithTag("Player");
		// Choose a player from _players array to attack
		_chosenPlayer = _players[Random.Range(0, _players.Length-1)];
		sm = _chosenPlayer.GetComponent<ShieldManager>();
		InvokeRepeating ("PathFinderUpdate", .01f, 0.5f);
		endPosition = sm.getEntrancePathPosition(1);
		transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
		_dt = GetComponent<DebugText> ();
	}

	void Update () {
		float step = moveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, endPosition, step);


		//pathfinder debug
		transform.GetComponent<LineRenderer>().SetPosition(0, transform.position);
		transform.GetComponent<LineRenderer>().SetPosition(1, endPosition);
	}
	
	void PathFinderUpdate(){
		bool foundPosition = false;	

		//check if core can be reached
		if (canReachCore()){
			_dt.setDebugText ("pathfind: core");
			foundPosition = true;
			endPosition = Vector3.zero;
		}

		if (!foundPosition) {
			//check if path can be reached
			for (int i=0; i<sm.getSpiralPathMax(); i++){
				print (i);
				Vector3 endPositionCheck = sm.getSpiralPathPosition(i);
				if (endPositionCheck!=null){
					if (CanSeePoint(endPositionCheck)){
						endPosition = endPositionCheck;
						foundPosition = true;
						_dt.setDebugText ("spiral: " + i + " " + endPosition);
						break;
					}
				}
			}
		}

		if (!foundPosition) {
			//try to reach entrance path
			for (int i=1; i<=5;i++){
				Vector3 endPositionCheck = sm.getEntrancePathPosition(i);
				if (endPositionCheck!=null){
					if (CanSeePoint(endPositionCheck)){
						endPosition = endPositionCheck;
						foundPosition = true;
						_dt.setDebugText ("entrance: " + i + " " + endPosition);
						break;
					}
				}
			}
		}
		

		if (!foundPosition) {
			_dt.setDebugText ("pathfind: error");
		}
	}

	void OnCollisionEnter (Collision col) {	
		//Debug.Log ("Enemy hits " + col.transform.name);
		if (col.gameObject.GetComponent<DamageController>() != null &&
		    col.transform.tag != "Enemy") {
			DamageController dc = col.gameObject.GetComponent<DamageController>();
			dc.takeDamage(damagePoints);
		}
	}

	bool CanSeePoint(Vector3 tempPoint) {
		int layerMask = 1 << 11;
		layerMask = ~layerMask;
		return !Physics.Linecast(transform.position, tempPoint,layerMask);
	}

	bool canReachCore(){
		Vector3 fwd = -transform.position;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, fwd, out hit, Vector3.Distance(transform.position, Vector3.zero))){
			if (hit.transform.name=="Core"){
				return true;
			} else {
				return false;
			}
		} else {
			return true;
		}
	}

	
}
