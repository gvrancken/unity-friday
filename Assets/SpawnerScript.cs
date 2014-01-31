using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

	public Transform prefab;
	public int spawnTime; 

	// Use this for initialization
	void Start () {
		Invoke ("Clone", spawnTime);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void Clone () {
		Instantiate(prefab, Vector3.zero, Quaternion.identity);
	}
}
