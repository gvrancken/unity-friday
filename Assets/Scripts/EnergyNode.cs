using UnityEngine;
using System.Collections;

public class EnergyNode : MonoBehaviour {
	private Vector3 _endPosition;
	private float _moveSpeed = 1.7f;
	private GameObject _hudManager;
	private Transform _absorbTarget;
	private GameObject _player;

	private void SetAbsorbTarget(Transform target){
		_absorbTarget = target;
		_endPosition = _absorbTarget.position;
	}

	void Start(){
		GameObject[] x = GameObject.FindGameObjectsWithTag("Player");
		SetAbsorbTarget(x[0].transform);
		_player = x [0];
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (transform.position, _endPosition);
		float speedModifier = Mathf.Clamp (1f / distance, 0.2f, 2.5f);
		float step = _moveSpeed * speedModifier * Time.deltaTime;
		Vector3 randomPosition = new Vector3(0,0,0);
		if (distance > 5) {
			 randomPosition = new Vector3 (2-Random.value*4, 0, 2-Random.value*4);
		}
		transform.position = Vector3.MoveTowards(transform.position, _endPosition+randomPosition, step);

		if (Vector3.Distance(transform.position,_endPosition) < 0.2) {
			absorbEnergy();
		}
	}

	private void absorbEnergy(){
		_player.GetComponent<ShieldManager>().AddEnergy(1);
		Destroy(this.gameObject);
	}
}
