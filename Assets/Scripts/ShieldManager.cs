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
	private float pulseState = 0;
	private Color defaultColor = new Color(0.8f, 0.8f, 1, 1);
	private Color damageColor = new Color(1, 0, 0, 1);
	private float damageEffect = 0;


	// Use this for initialization
	void Start () {
		//float tetha = 6+ (max * 6);
		shieldArray = new GameObject[max];
		for (int i = 0; i <= startCount-1; i++) {
			GameObject newShield = createShield(i);
			shieldArray[i] = newShield;
			newShield.GetComponent<Shield>().setEnergized(false);
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
		instance.transform.Rotate (new Vector3 (0, 2.5f, 0));
		float scaleFactor = (float)0.2*i;
		instance.localScale += new Vector3(scaleFactor, 0, 2f);
		instance.parent = transform;
		return instance.gameObject;
	}

	// Update is called once per frame
	void Update () {
		//Check if the shield is continues and de-energize all seperate parts
		for (int i = 0; i <=shieldArray.Length-1; i++) {
			if ((shieldArray[i]==null) && (firstBrokenPos > i)) {
				firstBrokenPos = i;
			} else if ((shieldArray[i]!=null) && (firstBrokenPos < i)) {
				shieldArray[i].GetComponent<Shield>().setEnergized(false);
			}
		}

		//Pulse the central crystal
		pulseState = Mathf.Sin (Time.time*5);
		pulseState = 1f + pulseState / 5;
		gameObject.renderer.material.SetFloat("_RimPower", pulseState);

		if (damageEffect > 0) {
			damageEffect -= 0.1f * Time.deltaTime;
			Debug.Log("damage effect = " + damageEffect);
			Color currentColor = ((1-damageEffect)*defaultColor)+(damageEffect*damageColor);
			gameObject.renderer.material.SetFloat ("_RimPower", ((1-damageEffect)*pulseState)+(0));
			gameObject.renderer.material.SetColor("_RimColor", currentColor);

			
		}


		//Pulse shield pieces with delay
		for (int i = 0; i <firstBrokenPos; i++) {
			if (shieldArray[i]!=null){
				pulseState = Mathf.Sin(((i*10) + Time.time) * 5);
				shieldArray[i].GetComponent<Shield>().setPulse(pulseState);
				shieldArray[i].GetComponent<Shield>().setEnergized(true);
			}
		}
	}
}
