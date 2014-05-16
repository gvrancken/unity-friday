using UnityEngine;
using System.Collections;

public class EnergyNode : MonoBehaviour {
	private Vector3 _endPosition;
	private float _moveSpeed = 1.7f;
	private GameObject _hudManager;
	private Transform _absorbTarget;
	private GameObject _player;

	private Vector3 _forceDirection;
	private float _initializationTime;
	private float _lifeTime;

	private void SetAbsorbTarget(Transform target){
		_absorbTarget = target;
		_endPosition = _absorbTarget.position;
	}

	void Start(){
		GameObject[] x = GameObject.FindGameObjectsWithTag("Player");
		SetAbsorbTarget(x[0].transform);
		_player = x [0];

		_initializationTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (transform.position, _endPosition);
		float speedModifier = Mathf.Clamp (1f / distance, 0.2f, 2.5f);
		float step = _moveSpeed * speedModifier * Time.deltaTime;
		Vector3 randomPosition = new Vector3(0,0,0);
		if (distance > 5) {
			 randomPosition = new Vector3 (3-Random.value*6, 0, 3-Random.value*6);
		}
		_lifeTime = Time.timeSinceLevelLoad - _initializationTime;
		if (_lifeTime<0.6f){
			transform.position += (_forceDirection * Random.value* 2 * (0.6f-_lifeTime)*(0.6f-_lifeTime));
		}
		transform.position = Vector3.MoveTowards(transform.position, _endPosition+randomPosition, step);

		if (Vector3.Distance(transform.position,_endPosition) < 0.2) {
			absorbEnergy();
		}
	}

	private void absorbEnergy(){
		if (_player.GetComponent<ShieldManager>().isCoreAlive()){
			_player.GetComponent<ShieldManager>().AddEnergy(1);
			Destroy(this.gameObject);
		}
	}

	public void setOriginPoint(Vector3 origin){
		_forceDirection = transform.position - origin;
		_forceDirection.Normalize();
	}
}
