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

	public void takeDamage(float damagePoints) {
		_hitpoints -= damagePoints;

		Debug.Log (gameObject.name + " has " + _hitpoints + " left!");

		if (_isDead == false && _hitpoints <= 0) {
			_isDead = true;
			Invoke ("DestroyMe", dieTime);
		}

	}

	void DestroyMe() {
		Destroy (gameObject);
	}

}
