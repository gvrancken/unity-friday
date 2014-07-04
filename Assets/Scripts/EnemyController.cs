using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {


	public float damagePerSecond = 10;
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
	private Collider targetCollider;
	private Vector3 _moveTargetPosition;
	private Vector3 _staticMovePoition;
	private Vector3 _disiredPosition;
	private Quaternion _staticMoveRotation;

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
		_staticMovePoition = transform.position;


	}

	void startAIFunctions(float startDelay){
		if (pathFindToCrystal)
			InvokeRepeating ("PathFinderUpdate", startDelay, 2f);

		else
			endPosition = Vector3.zero;
			
				
	}


	void Update () {
		float step = moveSpeed * Time.deltaTime;
		float moveOffset = 0.2f*Mathf.Sin (2*Time.time);
		float rotation = (25/moveSpeed)*Mathf.Sin (2*Time.time+1.7f);
		if ((Vector3.Distance(_staticMovePoition,endPosition)>0.1f)&&(Vector3.Distance(_staticMovePoition,transform.position)<1f)){

			_moveTargetPosition = Vector3.MoveTowards (_moveTargetPosition, endPosition, step*5);

			_staticMovePoition = Vector3.MoveTowards(_staticMovePoition, _moveTargetPosition, step);



			//Vector3 c = transform.right;



		}
		_disiredPosition = _staticMovePoition + transform.right*moveOffset;
		transform.LookAt (_moveTargetPosition);
		transform.localEulerAngles += new Vector3(0,(rotation),0);
		MoveLegs ();


		//pathfinder debug
		transform.GetComponent<LineRenderer>().SetPosition(0, transform.position);
		transform.GetComponent<LineRenderer>().SetPosition(1, _moveTargetPosition);
	}

	void FixedUpdate(){
		transform.rigidbody.AddRelativeForce (Vector3.forward*moveSpeed*2, ForceMode.Force);
	}

	void MoveLegs(){
		int i=0;
		foreach (Transform leg in transform) {
			float moveOffset = 0.005f*Mathf.Sin (10*Time.time+i);
			leg.localPosition += new Vector3(0,0,moveOffset);
			leg.localEulerAngles+=new Vector3(0,moveOffset*1000,0);
			i++;
		}

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
			targetCollider = col.collider;
			print ("Hit target!");
			StartCoroutine(DealDamage());
			//endPosition = targetCollider.transform.position;
			//StopCoroutine("PathFinderUpdate");
		}
	}

	IEnumerator DealDamage(){
		if (targetCollider!=null){
			Vector3 targetPoint = targetCollider.ClosestPointOnBounds (transform.position);
			print ("Distance: " + Vector3.Distance (targetPoint, transform.position));
			if (Vector3.Distance(targetPoint, transform.position) <= 1){
				DamageController dc = targetCollider.gameObject.GetComponent<DamageController>();
				print ("Deal damage to " + targetCollider.gameObject.name );
				dc.takeDamage(damagePerSecond/5);
				yield return new WaitForSeconds(0.2f);
				StartCoroutine(DealDamage());
			} else {
				print ("no target");
				targetCollider = null;
			}
		}
	}

	bool CanSeePoint(Vector3 tempPoint) {
		int layerMask = 1 << 11;
		layerMask = ~layerMask;
		return !Physics.Linecast(_staticMovePoition, tempPoint,layerMask);
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
