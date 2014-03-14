using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldManager : MonoBehaviour {

	public Transform shieldPiece;
	public Transform shieldJoint;
	public Transform core;
	public Transform path;
	public Transform entrancePath;
	public Camera mainCamera;
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
	private int lastShieldID = 0;
	private int firstBrokenPos = 0;
	private float pulseState = 0;
	private Color defaultColor = new Color(0.8f, 0.8f, 1, 1);
	private Color damageColor = new Color(1, 0, 0, 1);
	private float damageEffect = 0;
	private GameObject hudManager;
	private GameObject newJoint;
	private List<Vector3> EntrancePath = new List<Vector3>();
	private float playerRadius;


	// Use this for initialization
	void Start () {
		shieldJoints = new Vector3[max + 1];
		jointArray = new GameObject[max];
		shieldArray = new GameObject[max];
		pathPointArray = new GameObject[max];
		initializeSchieldJoints ();
		totalShields = startCount;
		InitializeShields();
		UpdateEntrancePoints ();
		shieldArray [0].GetComponent<Shield> ().InvokeRepeating ("EnergyPulse", .01f, 1f);

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
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(core.position, playerRadius);


		//Only draw entrancePosition
		Gizmos.color = Color.red;
		foreach (Transform child in entrancePath) {
			Gizmos.DrawWireSphere(child.position, 0.1f);
		}

	}

	void UpdateEntrancePoints() {

		if (pathPointArray [lastShieldID] != null) {
			entrancePath.position = pathPointArray [lastShieldID].transform.position;
			entrancePath.LookAt (core);
			entrancePath.Rotate(new Vector3(0,90,0));
			float scale = (playerRadius/7f);
			entrancePath.localScale = new Vector3(scale,scale,scale);
		}


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
	void InitializeShields() {
		newJoint = createJoint(-1);
		newJoint.transform.parent = core;
		for (int i = 0; i <= startCount; i++) {
			if (CreateNewShield(i)) {
				firstBrokenPos = i+1;
			}
		}
	}

	public bool CreateNewShield(int i) {
		if (shieldArray [i] == null) {
			GameObject newShield = createShield (i);
			newJoint = createJoint (i);
			newJoint.transform.parent = newShield.transform;
			shieldArray [i] = newShield;
			jointArray [i] = newJoint;
			newShield.GetComponent<Shield> ().SetJoint (newJoint.transform);
			newShield.gameObject.GetComponent<Shield> ().StartGrowing ();
			newShield.GetComponent<Shield> ().SetEnergized (false);
			newShield.name = "Shield" + i;
			totalShields++;
			if (i > lastShieldID) {
				lastShieldID = i;
				playerRadius = Vector3.Distance (core.position, newJoint.transform.position)*1.01f;
				Camera.main.orthographicSize = playerRadius*1.1f;
			}
			UpdateEntrancePoints();
			return true;
		} else {
			return false;
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
			shieldJoints[0] = new Vector3(0,0,0);
		}
	}

	public Vector3 GetJointPosition(int jointIndex) {
		return shieldJoints [jointIndex];
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
		instance.gameObject.GetComponent<Shield>().SetShieldIndex(i);
		foreach(Transform child in instance) {
			instance.gameObject.GetComponent<Shield>().setShieldWall(child);
			child.localScale = new Vector3(1, 0.01f, 0.3f );
		}
		instance.parent = transform;
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
			int y = i-10;
			if (y < 0) {y=0;}
			Vector3 pointPosition = newJoint.position/(1.6f-(y*0.008f));
			GameObject newPathPoint = new GameObject();
			newPathPoint.name = "PathPoints" + i;
			newPathPoint.transform.position = pointPosition;
			newPathPoint.tag = "PathPoint";
			newPathPoint.transform.parent = path;
			newPathPoint.transform.LookAt(core);
			pathPointArray[i] = newPathPoint;
			print ("PathPoint " + i + " created: " + pathPointArray[i].name);
		}
		return newJoint.gameObject;
	}

	public List<Vector3> getEntrancePathArray() {
		return EntrancePath;
	}

	void updateEnergy(){
		//Check if the shield is continues and de-energize all seperate parts
		for (int i = 0; i <=shieldArray.Length-1; i++) {
			if ((shieldArray[i]==null)) {
				firstBrokenPos = i;
				break;
			}
		}
		//Update Energy in HUD
		hudManager.GetComponent<HUDManager>().updateEnergy (firstBrokenPos);

	}

	// Update is called once per frame
	void Update () {
		updateEnergy ();

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

		//temp mouse scroll camera zoom
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
		{
			Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize-1, 3);
			
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
		{
			Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize+1, 10);
		}
	}
	//Pulse shield pieces with delay
	public void ShieldPulse(int shieldIndex) {
		if (shieldArray[shieldIndex]!=null){
			shieldArray[shieldIndex].GetComponent<Shield>().EnergyPulse();
		}
	}


}
