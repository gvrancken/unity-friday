using UnityEngine;
using System.Collections;

public class ShieldManager : MonoBehaviour {

	public Transform shieldPiece;
	public int max = 30;
	public float tetha = 20;
	public float alpha = 5;


	// Use this for initialization
	void Start () {
		//float tetha = 6+ (max * 6);
		Debug.Log (6+ (max * 6));
		for (int i = 5; i <= max+5; i++) {

			float t = (tetha/max)*i;
			float a = (alpha/max)*i;
			Vector3 newPosition = new Vector3(transform.position.x + a*Mathf.Cos(t), 0, transform.position.y + a*Mathf.Sin(t));
			Transform instance = (Transform)Instantiate(shieldPiece, newPosition, transform.rotation);
			instance.LookAt(transform);
			float scaleFactor = (float)0.1*i;
			instance.localScale += new Vector3(scaleFactor, 0, 0	);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
