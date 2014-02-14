using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float damagePoints = 10;

	// Perhaps more players will join the arena.
	// This enemy will choose at random which player to attack.
	private GameObject[] _players; 
	private GameObject _chosenPlayer; 

	public float moveSpeed = 5f;

	void Start () {
		// Fill _players array with all object with Player tag
		_players = GameObject.FindGameObjectsWithTag("Player");
		// Choose a player from _players array to attack
		_chosenPlayer = _players[Random.Range(0, _players.Length-1)];
	}

	void Update () {
		float step = moveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, _chosenPlayer.transform.position, step);
	}
	
	void OnCollisionEnter (Collision col) {	
		if (col.gameObject.tag != "Enemy" &&
		    col.gameObject.tag != "Bullet") {
			DamageController dc = col.gameObject.GetComponent<DamageController>();
			dc.takeDamage(damagePoints);
		}
	}
	
}
