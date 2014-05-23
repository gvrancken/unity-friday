using UnityEngine;
using System.Collections;

public class MeshTentacles : MonoBehaviour {

	private Vector3[] baseVertices;
	private GameObject[] tentacles;
	public int scatterEvery = 1;

	public GameObject tentacleObject;

	// Use this for initialization
	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		
		if (baseVertices == null) {
			baseVertices = mesh.vertices;
		}
		
		Vector3[] vertices = new Vector3[baseVertices.Length];
		tentacles = new GameObject[baseVertices.Length];

		for (int i=0; i < vertices.Length; i++) {

			if (i%scatterEvery == 0) {
			Vector3 vertex = baseVertices[i];

			Vector3 direction = transform.position - (transform.position + vertex);
			GameObject tentacle = Instantiate(tentacleObject, transform.position + vertex, Quaternion.LookRotation(direction)) as GameObject;
		//	Vector3 relativePos = transform.position - tentacleObject.transform.position;
		//	tentacle.transform.rotation = );
			tentacles[i] = tentacle;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;

		Vector3[] vertices = new Vector3[mesh.vertices.Length];

		for (int i=0; i < vertices.Length; i++) {

			if (i%scatterEvery == 0) {
				Vector3 vertex = mesh.vertices[i];
				
				GameObject tentacle = tentacles[i];
				tentacle.transform.position = transform.position + vertex;

			}
		}

	}
}
