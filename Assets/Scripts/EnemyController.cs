using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float damagePoints = 10;
	public bool pathFindToCrystal = false;

	// Perhaps more players will join the arena.
	// This enemy will choose at random which player to attack.
	private GameObject[] _players; 
	private GameObject _chosenPlayer; 
	private Vector3 endPosition;
	private ShieldManager sm;
	public float moveSpeed = 5f;
	private DebugText _dt;
	private int _lastSpiralPointChosen = -1;

	void Start () {
		// Fill _players array with all object with Player tag
		_players = GameObject.FindGameObjectsWithTag("Player");
		// Choose a player from _players array to attack
		_chosenPlayer = _players[Random.Range(0, _players.Length-1)];
		sm = _chosenPlayer.GetComponent<ShieldManager>();
		endPosition = sm.getEntrancePathPosition(1);
		transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
		_dt = GetComponent<DebugText> ();
		startAIFunctions (0.3f);
	}

	void startAIFunctions(float startDelay){
		if (pathFindToCrystal)
			InvokeRepeating ("PathFinderUpdate", startDelay, 2f);
		else
			endPosition = Vector3.zero;
			
				
	}

	void Update () {
		float step = moveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, endPosition, step);


		//pathfinder debug
//		transform.GetComponent<LineRenderer>().SetPosition(0, transform.position);
//		transform.GetComponent<LineRenderer>().SetPosition(1, endPosition);
	}
	
	void PathFinderUpdate(){
		bool foundPosition = false;	

		//check if core can be reached
		if (canReachCore()){
			_dt.setDebugText ("pathfind: core");
			foundPosition = true;
			endPosition = Vector3.zero;
		}

		if (_lastSpiralPointChosen != -1 && !foundPosition && sm.getSpiralPathMax()>=10) {
			//check if path can be reached
			int startCheckingPoint = 10;
			if (_lastSpiralPointChosen >=11)
				startCheckingPoint = _lastSpiralPointChosen-1;
			for (int i=startCheckingPoint; i<sm.getSpiralPathMax(); i++){
				Vector3 endPositionCheck = sm.getSpiralPathPosition(i);
				if (endPositionCheck!=null){
					if (CanSeePoint(endPositionCheck)){
						endPosition = endPositionCheck;
						foundPosition = true;
						_dt.setDebugText ("spiral: " + i);
						_lastSpiralPointChosen = i;
						break;
					}
				}
			}
		}

		if (!foundPosition) {
			//try to reach entrance path
			_lastSpiralPointChosen = -1;
			for (int i=1; i<=5;i++){
				Vector3 endPositionCheck = sm.getEntrancePathPosition(i);
				if (endPositionCheck!=null){
					if (CanSeePoint(endPositionCheck)){
						endPosition = endPositionCheck;
						foundPosition = true;
						_dt.setDebugText ("entrance: " + i);
						if (i==1)
							_lastSpiralPointChosen = sm.getSpiralPathMax();
						break;
					}
				}
			}
		}
		

		if (!foundPosition) {
			_dt.setDebugText ("pathfind: error");
			_lastSpiralPointChosen = 1;
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
