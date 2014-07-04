using UnityEngine;
using System.Collections;

public class DamageController : MonoBehaviour {

	public float MaxHitPoints = 1;
	public float dieTime = 0.01f;
	public int energySpawnOnDeath = 0;
	public Transform energyNode;
	public float SpawnRange = 0.5f;

	private float _hitpoints;
	private bool _isDead = false;


	void Start () {
		_hitpoints = MaxHitPoints;
	}

	void Update() {
	
	}

	public void takeDamage(float damagePoints) {
		_hitpoints -= damagePoints;
		if (gameObject.name == "Core") {
			gameObject.GetComponent<PlayerController>().GetDamage();
		}
		if (gameObject.GetComponent<DebugText> ()!=null)
			gameObject.GetComponent<DebugText> ().setDebugText ("HP: " + _hitpoints);
		
		if (_isDead == false && _hitpoints <= 0) {
			Die();
		}

	}

	public void Die(){
		_isDead = true;
		if (energyNode!=null)
			SpawnEnergy();
		Destroy  (gameObject, dieTime);
		if (gameObject.name == "Core") {
			gameObject.GetComponent<PlayerController>().Die();
		}
	}

	public void Heal(float healingPoints) {
		if (_hitpoints + healingPoints < MaxHitPoints){
			_hitpoints += healingPoints;
		} else {
			_hitpoints = MaxHitPoints;
		}
		if (gameObject.GetComponent<DebugText> ()!=null)
			gameObject.GetComponent<DebugText> ().setDebugText ("HP: " + _hitpoints);
	}

	public float GetHitPoints() {
		return _hitpoints;
		
	}

	public float GetMaxHitPoints(){
		return MaxHitPoints;
	}

	public void SetMaxHitPoints(float maxHP){
		MaxHitPoints = maxHP;
	}

	private void SpawnEnergy(){
		if (energySpawnOnDeath > 0) {
			for (int i = 1; i<=energySpawnOnDeath; i++) {
				float spawnX = transform.position.x + (SpawnRange - (Random.value*2*SpawnRange));
				float spawnZ = transform.position.z + (SpawnRange - (Random.value*2*SpawnRange));
				Vector3 spawnPosition = new Vector3(spawnX, 0, spawnZ);
				Transform instance = (Transform)Instantiate(energyNode, spawnPosition, transform.rotation);
				instance.GetComponent<EnergyNode>().setOriginPoint(transform.position);
			}
		}
	}
}
