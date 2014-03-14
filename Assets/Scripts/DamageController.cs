using UnityEngine;
using System.Collections;

public class DamageController : MonoBehaviour {

	public float MaxHitPoints = 1;
	public float dieTime = 0.01f;

	private float _hitpoints;
	private bool _isDead = false;

	void Start () {
		_hitpoints = MaxHitPoints;
	}

	void Update() {
	
	}

	public void takeDamage(float damagePoints) {
		_hitpoints -= damagePoints;
		if (_isDead == false && _hitpoints <= 0) {
			_isDead = true;
			Destroy  (gameObject, dieTime);
		}

	}

	public void Heal(float healingPoints) {
		if (_hitpoints + healingPoints < MaxHitPoints){
			_hitpoints += healingPoints;
		} else {
			_hitpoints = MaxHitPoints;
		}
	}

	public float GetHitPoints() {
		return _hitpoints;
		
	}
}
