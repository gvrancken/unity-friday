using UnityEngine;
using System.Collections;

public class ShieldManager : MonoBehaviour {

	public Transform shieldPiece;
	public int max = 45;
	public float tetha = 30;
	public float alpha = 4;
	private GameObject[] shieldArray; 



	// Use this for initialization
	void Start () {
		//float tetha = 6+ (max * 6);
		Debug.Log (6+ (max * 6));
		for (int i = 0; i <= max; i++) {

			GameObject newShield = createShield(i);
			Debug.Log(newShield);

		}
	}

	public GameObject createShield(int i) {
		Debug.Log("ik ben hier");
		i += 6;
		float t = (tetha/max)*i;
		float a = (alpha/max)*i;
		Vector3 newPosition = new Vector3(transform.position.x + a*Mathf.Cos(t), 0, transform.position.y + a*Mathf.Sin(t));
		Transform instance = (Transform)Instantiate(shieldPiece, newPosition, transform.rotation);
		instance.LookAt(transform);
		float scaleFactor = (float)0.1*i;
		instance.localScale += new Vector3(scaleFactor, 0, 0	);
		instance.parent = transform;
		foreach (Transform child in instance) {
			child.gameObject.renderer.material.color = new Color (0f, 0f, 0.8f, 1f);
		}
		return instance.gameObject;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
