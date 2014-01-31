using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

	public GameObject prefab;
	public GameObject spawnCore;

	public float startSpawnTime = 2f;
	public float spawnTime = 2f;


	// Use this for initialization
	void Start () {
		InvokeRepeating ("Clone", startSpawnTime, spawnTime);

	}
	
	// Update is called once per frame
	void Update () {

	}

	static Vector2 PointOnCircle(float radius, float angleInDegrees, Vector2 origin)
	{
		// Convert from degrees to radians via multiplication by PI/180        
		float x = (radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180f)) + origin.x;
		float y = (radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180f)) + origin.y;
		
		return new Vector2(x, y);
	}

	void Clone () {
		float randomAngle = Random.Range (0,360);
	
		Vector2 pointOnCircle = PointOnCircle(spawnCore.transform.localScale.x / 2, randomAngle, new Vector2(spawnCore.transform.position.x, spawnCore.transform.position.z));
		Vector3 pointInSpace = new Vector3(pointOnCircle.x, 0, pointOnCircle.y);

		GameObject instance = Instantiate(prefab, pointInSpace, Quaternion.identity) as GameObject;
		instance.rigidbody.AddTorque(new Vector3(randomAngle,randomAngle,randomAngle));
	}
	
}
