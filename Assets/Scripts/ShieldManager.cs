using UnityEngine;
using System.Collections;

public class ShieldManager : MonoBehaviour {

	public Transform shieldPiece;
	public Transform shieldJoint;
	public Transform core;
	public Transform path;
	[Range(1,500)]
	public int startCount = 15; 
	[Range(0,20)]
	public int startPos = 12;
	[Range(1,1000)]
	public int max = 1000;
	[Range(1f,2000f)]
	public float tetha = 477;
	[Range(-500f,1000f)]
	public float alpha = 131;
	[Range(0.1f,100f)]
	public float alphaScale = 9;
	[Range(0f,90f)]
	public float shieldRotate = 15f;
	[Range(0f,25f)]
	public float shieldThickness = 0.2f;
	[Range(0f,2f)]
	public float shieldLengthScale = 0.2f;
	[Range(0f,15f)]
	public float shieldLengthFactor = 0.2f;

	public Color colorEnergized = new Color (0.3f, 0.3f, 1f, 1f);	 
	public Color colorEmpty = new Color (0.4f, 0.4f, 0.4f, 0.8f);	
	public Color colorDamage = new Color (1.0f, 0, 0, 1);
	
	private Vector3[] shieldJoints; 
	private GameObject[] jointArray; 
	private GameObject[] shieldArray; 
	private GameObject[] pathPointArray; 
	private int totalShields = 0;
	private int firstBrokenPos = 0;
	private float pulseState = 0;
	private Color defaultColor = new Color(0.8f, 0.8f, 1, 1);
	private Color damageColor = new Color(1, 0, 0, 1);
	private float damageEffect = 0;
	private GameObject hudManager;


	// Use this for initialization
	void Start () {
		shieldJoints = new Vector3[max + 1];
		jointArray = new GameObject[max];
		shieldArray = new GameObject[max];
		pathPointArray = new GameObject[max];
		initializeSchieldJoints ();
		totalShields = startCount;
		rearrangeShields();
		SetEntrancePoints ();

		//Set core material
		core.gameObject.renderer.material.SetColor("_ColorTint", new Color(1,1,1,1));
		core.gameObject.renderer.material.SetColor("_RimColor", colorEnergized);

		//Get HUDManager
		GameObject[] x = GameObject.FindGameObjectsWithTag("HUDManager");
		hudManager = x [0];


	}


	void OnDrawGizmosSelected() {
		//Debug code to render the vector-list used to create the shields.
//		for (int i = 0; i<=max; i++) {
//				Gizmos.color = Color.white;
//				Gizmos.DrawWireSphere (shieldJoints[i], 1);
//		}


		//Only draw entrancePosition
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(pathPointArray[totalShields].transform.position, 0.1f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(pathPointArray[1].transform.position, 0.1f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(pathPointArray[2].transform.position, 0.1f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(pathPointArray[3].transform.position, 0.1f);
	}

	void SetEntrancePoints() {
		GameObject newEntrancePoint1 = new GameObject();
		newEntrancePoint1.name = "EntrancePoint" + 1;
		Vector3 newPos =  pathPointArray [totalShields].transform.position+RotateY(pathPointArray [totalShields].transform.position, -Mathf.PI/2);
		newEntrancePoint1.transform.position = newPos;
		newEntrancePoint1.tag = "PathPoint";
		newEntrancePoint1.transform.parent = path;
		pathPointArray[1] = newEntrancePoint1;

		GameObject newEntrancePoint2 = new GameObject();
		newEntrancePoint2.name = "EntrancePoint" + 2;
		newPos =  (newEntrancePoint1.transform.position)+RotateY(newEntrancePoint1.transform.position, -Mathf.PI/1.6f);
		newEntrancePoint2.transform.position = newPos;
		newEntrancePoint2.tag = "PathPoint";
		newEntrancePoint2.transform.parent = path;
		pathPointArray[2] = newEntrancePoint2;

		GameObject newEntrancePoint3 = new GameObject();
		newEntrancePoint3.name = "EntrancePoint" + 2;
		newPos =  jointArray[totalShields].transform.position*1.3f;
		newEntrancePoint3.transform.position = newPos;
		newEntrancePoint3.tag = "PathPoint";
		newEntrancePoint3.transform.parent = path;
		pathPointArray[3] = newEntrancePoint3;
	}

	private Vector3 RotateY(Vector3 v, float angle )
		
	{
		float sin = Mathf.Sin( angle );
		float cos = Mathf.Cos( angle );

		float tx = v.x;
		float tz = v.z;
		
		v.x = (cos * tx) + (sin * tz);
		v.z = (cos * tz) - (sin * tx);
		return v;
	}

	//Create all shieds
	void rearrangeShields() {
		GameObject newJoint = createJoint(-1);
		newJoint.transform.parent = core;
		for (int i = 0; i <= totalShields; i++) {
			GameObject newShield = createShield(i);
			newJoint = createJoint(i);
			newJoint.transform.parent = newShield.transform;
			shieldArray[i] = newShield;
			jointArray[i] = newJoint;
			newShield.GetComponent<Shield>().setEnergized(false);
			firstBrokenPos = i+1;
		}
	}

	//Initialize all position for the shields.
	void initializeSchieldJoints() {
		for (int i = 0; i<shieldJoints.Length; i++) {
			float localAlpha = alpha;
			//The alpha for the first 20 shields is a bit different to create the perfect spiral
			if (i>10) {
				if (i< 20){
					localAlpha = alpha + (((i-10f)/10f)*(alphaScale *(i-(10-startPos))));
				} else {
					localAlpha = alpha + ((alphaScale *(i-(10-startPos))));
				}
			}
			float t = (tetha / max) * (i+startPos);
			float a = (localAlpha / max) * (i+startPos);
			Vector3 newPosition = new Vector3 (transform.position.x + a * Mathf.Cos (t), 0, transform.position.y + a * Mathf.Sin (t));
			float noiseFactor = 0.01f * i;
			newPosition += new Vector3(Random.Range(-noiseFactor, noiseFactor),0,Random.Range(-noiseFactor, noiseFactor));
			shieldJoints[i] = newPosition;
		}
	}

	//Generates a shield at the given position
	public GameObject createShield(int i) {
		//Take position inbetween 2 shieldJoints
		Vector3 shieldPosition = (shieldJoints [i] + shieldJoints [i + 1])/2;
		Transform instance = (Transform)Instantiate(shieldPiece, shieldPosition, transform.rotation);
		instance.LookAt(shieldJoints [i]);
		instance.transform.Rotate(0, 90, 0);
		float scaleFactor = Vector3.Distance (shieldJoints [i], shieldJoints [i+1])/2;
		instance.gameObject.GetComponent<Shield>().SetDesitnationTransform(scaleFactor, shieldJoints[i]);
		foreach(Transform child in instance) {
			instance.gameObject.GetComponent<Shield>().setShieldWall(child);
			child.localScale = new Vector3(1, 0.01f, 0.3f );
		}
		instance.parent = transform;
		instance.gameObject.GetComponent<Shield> ().StartGrowing ();
		return instance.gameObject;
	}

	//Generates a shield at the given position
	public GameObject createJoint(int i) {
		//Take position inbetween 2 shieldJoints
		Vector3 jointPosition = shieldJoints [i+1];
		Transform newJoint = (Transform)Instantiate(shieldJoint, jointPosition, transform.rotation);
		newJoint.name = "ShieldJoint" + i;

		//Create pathPoint
		if (i >= 10) {
			float y = i-10;
			if (y < 0) {y=0;}
			Vector3 pointPosition = newJoint.position/(1.6f-(y*0.008f));
			GameObject newPathPoint = new GameObject();
			newPathPoint.name = "PathPoints" + i;
			newPathPoint.transform.position = pointPosition;
			newPathPoint.tag = "PathPoint";
			newPathPoint.transform.parent = path;
			newPathPoint.transform.LookAt(core);
			pathPointArray[i] = newPathPoint;
		}
		return newJoint.gameObject;
	}

	public Vector3 getEntrancePathPosition() {
		return pathPointArray[totalShields].transform.position;
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
		hudManager.GetComponent<HUDManager>().updateEnergy (firstBrokenPos);
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
//		Debug.Log(firstBrokenPos);
		for (int i = 0; i <firstBrokenPos; i++) {
			if (shieldArray[i]!=null){
				pulseState = Mathf.Sin(((i*10) + Time.time) * 5);
				shieldArray[i].GetComponent<Shield>().setPulse(pulseState);
				shieldArray[i].GetComponent<Shield>().setEnergized(true);
			}
		}
	}
}
