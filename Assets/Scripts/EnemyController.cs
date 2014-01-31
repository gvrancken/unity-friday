using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	private GameObject[] _players; 
	private GameObject _chosenPlayer; 

	public float moveSpeed = 5f;
	
	// Use this for initialization
	void Start () {
		_players = GameObject.FindGameObjectsWithTag("Player");
		_chosenPlayer = _players[Random.Range(0, _players.Length-1)];
		Debug.Log ("Will attack: " + _chosenPlayer.name);
		
	}
	
	// Update is called once per frame
	void Update () {
		float step = moveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, _chosenPlayer.transform.position, step);
	}

	void OnCollisionEnter (Collision col) {

	

	}
}
