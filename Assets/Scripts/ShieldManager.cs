using UnityEngine;
using System.Collections;

public class ShieldManager : MonoBehaviour {

	public Transform shieldPiece;
	public Transform core;
	[Range(1,500)]
	public int startCount = 15; 
	[Range(1,1000)]
	public int max = 1000;
	[Range(1f,2000f)]
	public float tetha = 715;
	[Range(-500f,1000f)]
	public float alpha = 220;
	[Range(0.1f,100f)]
	public float alphaScale = 30;
	[Range(0f,90f)]
	public float shieldRotate = 15f;
	[Range(0f,25f)]
	public float shieldThickness = 2f;
	[Range(0f,2f)]
	public float shieldLengthScale = 0.2f;
	[Range(0f,15f)]
	public float shieldLengthFactor = 0.2f;


	private Vector3[] shieldJoints; 
	private GameObject[] shieldArray; 
	private int totalShields = 0;
	private int firstBrokenPos = 0;
	private float pulseState = 0;
	private Color defaultColor = new Color(0.8f, 0.8f, 1, 1);
	private Color damageColor = new Color(1, 0, 0, 1);
	private float damageEffect = 0;


	// Use this for initialization
	void Start () {
			shieldJoints = new Vector3[max + 1];
			shieldArray = new GameObject[max];
			initializeSchieldJoints ();
			rearrangeShields();
	}

	void OnDrawGizmosSelected() {
		for (int i = 0; i<=max; i++) {
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere (shieldJoints[i], 1);

		}
	}

	void rearrangeShields() {

		for (int i = 0; i <= startCount; i++) {
			GameObject newShield = createShield(i);
			shieldArray[i] = newShield;
			newShield.GetComponent<Shield>().setEnergized(false);
			firstBrokenPos = i+1;
		}
	}

	void initializeSchieldJoints() {
		for (int i = 0; i<shieldJoints.Length; i++) {
			float localAlpha = alpha;
			if (i>10) {
				if (i< 20){
					Debug.Log(((i-10f)/10f));
					localAlpha = alpha + (((i-10f)/10f)*(alphaScale *(i-4f)));
				} else {
					localAlpha = alpha + ((alphaScale *(i-4)));
				}
			}
			float t = (tetha / max) * (i+6);
			float a = (localAlpha / max) * (i+6);
			Vector3 newPosition = new Vector3 (transform.position.x + a * Mathf.Cos (t), 0, transform.position.y + a * Mathf.Sin (t));
			shieldJoints[i] = newPosition;
		}
	}

	public GameObject createShield(int i) {
		//Take position inbetween 2 shieldJoints
		Vector3 shieldPosition = (shieldJoints [i] + shieldJoints [i + 1])/2;
		Transform instance = (Transform)Instantiate(shieldPiece, shieldPosition, transform.rotation);
		instance.LookAt(transform);
		instance.transform.Rotate(0, shieldRotate, 0);
		float scaleFactor = Vector3.Distance (shieldJoints [i], shieldJoints [i+1]);
		instance.localScale = new Vector3(scaleFactor/2, instance.localScale.y, (float)instance.localScale.z*(1f+(i/20f)) );
		instance.parent = transform;
		return instance.gameObject;
	}

	// Update is called once per frame
	void Update () {
		initializeSchieldJoints ();
		//rearrangeShields ();
		//Check if the shield is continues and de-energize all seperate parts
		for (int i = 0; i <=shieldArray.Length-1; i++) {
			if ((shieldArray[i]==null) && (firstBrokenPos > i)) {
				firstBrokenPos = i;
			} else if ((shieldArray[i]!=null) && (firstBrokenPos < i)) {
				shieldArray[i].GetComponent<Shield>().setEnergized(false);
			}
		}
//			
		//Pulse the central crystal
		pulseState = Mathf.Sin (Time.time*5);
		pulseState = 1f + pulseState / 5;
		core.renderer.material.SetFloat("_RimPower", pulseState);

		if (damageEffect > 0) {
			damageEffect -= 0.1f * Time.deltaTime;
			Color currentColor = ((1-damageEffect)*defaultColor)+(damageEffect*damageColor);
			core.renderer.material.SetFloat ("_RimPower", ((1-damageEffect)*pulseState)+(0));
			core.renderer.material.SetColor("_RimColor", currentColor);

			
		}

		//Pulse shield pieces with delay
		Debug.Log(firstBrokenPos);
		for (int i = 0; i <firstBrokenPos; i++) {
			if (shieldArray[i]!=null){
				pulseState = Mathf.Sin(((i*10) + Time.time) * 5);
				shieldArray[i].GetComponent<Shield>().setPulse(pulseState);
				shieldArray[i].GetComponent<Shield>().setEnergized(true);
			}
		}
	}
}
