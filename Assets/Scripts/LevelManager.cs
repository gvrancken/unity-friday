using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

		public GameObject spawnObject;
		public List<Transform> enemyList = new List<Transform> ();
		public List<Transform> constructionsList = new List<Transform> ();
		public Transform selectedBuildConstruct;

		private Transform spawnSphere;
		private int level = 0;
		private int numEnemies;
		private int enemyWaveDelay = 1;
		private int enemyWaveMax = 10;
		private int numWaves = 0;
		private float timeSinceLastSpawn = 0;

		private static Texture2D _staticRectTexture;
		private static GUIStyle _staticRectStyle;

		private GameObject _hudManager;
	
		void Start ()
		{
				_hudManager = GameObject.Find ("HUDManager");
				
				spawnSphere = transform.FindChild ("SpawnSphere");
				newLevel ();
		}

		void newLevel ()
		{
				level++;

				//Difficulty Knobs
				//Knob Variable			Max Condition					Max		Calculation
				numEnemies = 			(numEnemies > 100) ? 			100 : 	level * 4;
		print ("numEnemies: " + numEnemies);
				enemyWaveDelay =		Mathf.RoundToInt(10f + (0.4f * numEnemies));
				enemyWaveMax = 			(enemyWaveMax > 100) ? 			100 : 	3 * level;
		}

		void Update ()
		{
				timeSinceLastSpawn += Time.deltaTime;

				if (timeSinceLastSpawn >= enemyWaveDelay) {
						newWave ();
						timeSinceLastSpawn = 0;
				}
				if (numWaves >= enemyWaveMax) {
						newLevel ();
				}

		_hudManager.GetComponent<HUDManager>().updateWave("Wave: " + numWaves + " (time to next wave: " + Mathf.Round(enemyWaveDelay-timeSinceLastSpawn)+")");
		}

		private static Vector2 PointOnCircle (float radius, float angleInDegrees, Vector2 origin)
		{
				// Convert from degrees to radians via multiplication by PI/180        
				float x = (radius * Mathf.Cos (angleInDegrees * Mathf.PI / 180f)) + origin.x;
				float y = (radius * Mathf.Sin (angleInDegrees * Mathf.PI / 180f)) + origin.y;

				return new Vector2 (x, y);
		}
	

	
		private void newWave ()
		{
				float randomAngle = Random.Range (0, 360);

				float spawnRadius = spawnSphere.GetComponent<SphereCollider> ().radius;
				Vector2 spawnPoint2D = PointOnCircle (spawnRadius, randomAngle, Vector2.zero);
				Vector3 spawnPoint = new Vector3 (spawnPoint2D.x, 0, spawnPoint2D.y);

				GameObject instance = Instantiate (spawnObject, spawnPoint, Quaternion.identity) as GameObject;
				PodController pc = instance.GetComponent<PodController> ();

				for (int i=0; i<numEnemies; i++) {
						if (enemyList.Count > 0) {
								pc.unitQueue.Enqueue (enemyList [Mathf.RoundToInt(Random.Range (0,enemyList.Count-0.5f))]);
						}
				}
				
				numWaves++;
				
		}

		public void setSpawnSphereRadius(float radius){
			spawnSphere = transform.FindChild ("SpawnSphere");
			spawnSphere.GetComponent<SphereCollider> ().radius = radius;
		}


}
