using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWalkController : MonoBehaviour {
	
	public float moveSpeed = 5f;

	// Perhaps more players will join the arena.
	// This enemy will choose at random which player to attack.
	private GameObject[] _players; 
	private GameObject _chosenPlayer; 
	private Vector3 endPosition;
	private List<Vector3> entrancePath = new List<Vector3>();


	void Start () {
		// Fill _players array with all object with Player tag
		_players = GameObject.FindGameObjectsWithTag("Player");
		// Choose a player from _players array to attack
		_chosenPlayer = _players[Random.Range(0, _players.Length-1)];
		ShieldManager sm = _chosenPlayer.GetComponent<ShieldManager>();

		// get entrance path from player
		foreach (Transform child in sm.entrancePath) {
			entrancePath.Add (child.position);
		}


		//endPosition = sm.getEntrancePathPosition();
	}
	
	void Update () {
			//Debug.Log (transform.GetComponent<LineOfSight>().CanSeePoint(entrancePath[0]));
	}
	
	void OnCollisionEnter (Collision col) {	
		//Debug.Log ("Enemy hits " + col.transform.name);
	}
	
}
