using UnityEngine;
using System.Collections;
using TreeEditor;

/**
 * 	Based on CrumpleMesh.js
 */
public class PerlinDeform : MonoBehaviour {


	public float scale = 0.3f;
	public float speed = 0.3f;
	public bool recalculateNormals = false;
	
	private Vector3[] baseVertices;
	
	private Perlin noise = new Perlin ();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		
		if (baseVertices == null) {
			baseVertices = mesh.vertices;
		}
		
		Vector3[] vertices = new Vector3[baseVertices.Length];
		
		float time_x = Time.time * speed + 0.1365143f;
		float time_y = Time.time * speed + 1.21688f;
		float time_z = Time.time * speed + 2.5564f;
		
		for (int i=0; i<vertices.Length; i++) {
			
			Vector3 vertex = baseVertices[i];

			vertex.x += noise.Noise(time_x + vertex.x, time_x + vertex.y, time_x + vertex.z) * scale;
			vertex.y += noise.Noise(time_y + vertex.x, time_y + vertex.y, time_y + vertex.z) * scale;
			vertex.z += noise.Noise(time_z + vertex.x, time_z + vertex.y, time_z + vertex.z) * scale;
			
			vertices[i] = vertex;

		}
		
		mesh.vertices = vertices;
		
		if (recalculateNormals) {
			mesh.RecalculateNormals();
		}
		
		mesh.RecalculateBounds();
		
	}
}
