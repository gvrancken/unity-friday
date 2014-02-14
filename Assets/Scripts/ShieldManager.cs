using UnityEngine;
using System.Collections;

public class ShieldManager : MonoBehaviour {

	public Transform shieldPiece;
	public int startCount = 25;
	public int max = 45;
	public float tetha = 30;
	public float alpha = 4;

	private GameObject[] shieldArray; 
	private int totalShields = 0;
	private int firstBrokenPos = 0;


	// Use this for initialization
	void Start () {
		//float tetha = 6+ (max * 6);
		shieldArray = new GameObject[max];
		for (int i = 0; i <= startCount-1; i++) {
			GameObject newShield = createShield(i);
			shieldArray[i] = newShield;
			//newShield.GetComponent<Shield>().setEnergized(true);
			firstBrokenPos = i+1;
		}
	}

	public GameObject createShield(int i) {
		i += 6;
		float t = (tetha/max)*i;
		float a = (alpha/max)*i;
		Vector3 newPosition = new Vector3(transform.position.x + a*Mathf.Cos(t), 0, transform.position.y + a*Mathf.Sin(t));
		Transform instance = (Transform)Instantiate(shieldPiece, newPosition, transform.rotation);
		instance.LookAt(transform);
		float scaleFactor = (float)0.2*i;
		instance.localScale += new Vector3(scaleFactor, 0, 0);
		instance.parent = transform;
		return instance.gameObject;
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i <=shieldArray.Length-1; i++) {
			if ((shieldArray[i]==null) && (firstBrokenPos > i)) {
				firstBrokenPos = i;
			} else if ((shieldArray[i]!=null) && (firstBrokenPos < i)) {
				shieldArray[i].GetComponent<Shield>().setEnergized(false);
			}
		}
	}
}
