using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float damagePoints = 10;

	// Perhaps more players will join the arena.
	// This enemy will choose at random which player to attack.
	private GameObject[] _players; 
	private GameObject _chosenPlayer; 
	private Vector3 endPosition;
	public float moveSpeed = 5f;

	void Start () {
		// Fill _players array with all object with Player tag
		_players = GameObject.FindGameObjectsWithTag("Player");
		// Choose a player from _players array to attack
		_chosenPlayer = _players[Random.Range(0, _players.Length-1)];
		ShieldManager sm = _chosenPlayer.GetComponent<ShieldManager>();

		endPosition = sm.getEntrancePathPosition();
	}

	void Update () {
		float step = moveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
	}

	void OnCollisionEnter (Collision col) {	
		Debug.Log ("Enemy hits " + col.transform.name);
		if (col.gameObject.GetComponent<DamageController>() != null &&
		    col.transform.tag != "Enemy") {
			DamageController dc = col.gameObject.GetComponent<DamageController>();
			dc.takeDamage(damagePoints);
		}
	}
	
}
