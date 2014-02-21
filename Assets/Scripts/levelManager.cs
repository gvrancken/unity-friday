using UnityEngine;
using System.Collections;

public class levelManager : MonoBehaviour {

	public GameObject enemy;

	public int spawnRadius = 20;
	public float spawnPitRadius = 1.0f;

	private int level=0;
	private SpawnerScript spawnerScript;
	private int numEnemies;
	private int enemyWaveDelay = 3;
	//private int enemyWaveMax;
	private Vector2 spawnCenterPoint;
	private float timeSinceLastSpawn = 0;
	
	void Start () {
		spawnerScript = GetComponent<SpawnerScript>();
		newLevel ();
	}

	void newLevel() {
		level++;
		//Difficulty Knobs
		//Knob Variable			Max Condition					Max		Calculation
		numEnemies 				=(numEnemies > 100) 			? 100 	:level * 4 ;
		enemyWaveDelay 			=(enemyWaveDelay < 1)			? 1		: 5 - (level-1)*2;	
		//enemyWaveMax   			=(enemyWaveMax > 8) 			? 8		:1 * level+1;
		Debug.Log (enemyWaveDelay);
	}

	void Update() {
		timeSinceLastSpawn += Time.deltaTime;
		Debug.Log (timeSinceLastSpawn);
		if (timeSinceLastSpawn >= enemyWaveDelay) {
			Clone ();
			timeSinceLastSpawn = 0;
		}

	}

	private static Vector2 PointOnCircle(float radius, float angleInDegrees, Vector2 origin)
	{
		// Convert from degrees to radians via multiplication by PI/180        
		float x = (radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180f)) + origin.x;
		float y = (radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180f)) + origin.y;
		
		return new Vector2(x, y);
	}
	
	private void Clone () {
		float randomAngle;
		
		spawnCenterPoint = PointOnCircle(spawnRadius, Random.Range (0,360), Vector2.zero);



		for (int i=0; i<numEnemies; i++) {
			randomAngle = Random.Range (0,360);
			Vector2 spawnPoint2D = PointOnCircle(spawnPitRadius, randomAngle, spawnCenterPoint);
			Vector3 spawnPoint = new Vector3(spawnPoint2D.x, 0, spawnPoint2D.y);
			GameObject instance = Instantiate(enemy, spawnPoint, Quaternion.identity) as GameObject;
			instance.rigidbody.AddTorque(new Vector3(randomAngle,randomAngle,randomAngle));
		}
	}
}
