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

	public Color coreColorDefault = new Color(0.05f, 0.3f, 1, 1);
	public Color coreColorDamage = new Color(1, 0, 0, 1);
	public Color colorEnergized = new Color (0.05f, 0.3f, 1f, 1f);	 
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

	private float damageEffect = 0;
	private GameObject _hudManager;
	private GameObject newJoint;
	private float playerRadius;
	private float outOfViewRadius;
	private int _energy;
	private GameObject _levelManager;


	// Use this for initialization
	void Start () {
		_levelManager = GameObject.Find ("LevelManager");
		shieldJoints = new Vector3[max + 1];
		jointArray = new GameObject[max];
		shieldArray = new GameObject[max];
		pathPointArray = new GameObject[max];
		initializeSchieldJoints ();
		totalShields = startCount;
		InitializeShields();
		InvokeRepeating ("CorePulse", .01f, 1f);

		//Set core material
		core.gameObject.renderer.material.SetColor("_ColorTint", coreColorDefault);
		//core.gameObject.renderer.material.SetColor("_RimColor", colorEnergized);

		//Get HUDManager
		GameObject[] x = GameObject.FindGameObjectsWithTag("HUDManager");
		_hudManager = x [0];

	}



	/* updates the 5 entrance points for pathfindig around the spiral
	 * the 5 points have a fixed position but get a new position, rotation & scale
	 * every time a new shield is added to the spiral.*/
	void UpdateEntrancePoints() {
		if (pathPointArray [lastShieldID] != null) {

			//reposition to the last spiralPathPoint
			entrancePath.position = pathPointArray [lastShieldID].transform.position;
			entrancePath.LookAt (core);
			entrancePath.Rotate(new Vector3(0,90,0));

			//Scale according to the playerRadius
			float scale = (playerRadius/7f);
			entrancePath.localScale = new Vector3(scale,scale,scale);
		}
	}


	/* Vector calculation: rotate a vector with a specified angle
	 * Should be moved to a seperate vector-calculation class */
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

	/* Create all shieds on game-start according to the public startCount */
	void InitializeShields() {
		for (int i = 0; i <= startCount; i++) {
			if (CreateNewShield(i)) {
				firstBrokenPos = i+1;
			}
		}
	}

	/* Create a new shield */
	public bool CreateNewShield(int i) {
		if (shieldArray [i] == null) {
			GameObject newShield = createShield (i);
			newJoint = createJoint (i);
			createSpiralPathPoint(i);
			shieldArray [i] = newShield;
			jointArray [i] = newJoint;

			Transform startJoint;
			if (i==0) {
				//Player core is 'first' joint
				startJoint = core;
			} else {
				startJoint = jointArray[i-1].transform;
			}
			newShield.GetComponent<Shield> ().SetStartJoint (startJoint);
			newShield.GetComponent<Shield> ().SetEndJoint (newJoint.transform);
			newShield.GetComponent<Shield>().SetEndJointPosition(shieldJoints[i]);
			newShield.gameObject.GetComponent<Shield> ().StartGrowing ();
			newShield.GetComponent<Shield> ().SetEnergized (false);
			newShield.name = "Shield" + i;
			totalShields++;
			if (i > lastShieldID) {
				lastShieldID = i;
				playerRadius = Vector3.Distance (core.position, shieldJoints[i])*1.01f;
				outOfViewRadius = playerRadius*2;
				_levelManager.GetComponent<LevelManager>().setSpawnSphereRadius(playerRadius+5);
				Camera.main.GetComponent<CameraController>().setMaxViewSize(playerRadius*1.5f);
				core.GetComponent<DamageController>().SetMaxHitPoints(100+(6*i));
			}
			UpdateEntrancePoints();
			return true;
		} else {
			return false;
		}

	}

	/* Returns a bool based on whether the given shieldID already contains a shield or not
	 * When a shield is destroyed, a new shield can be build */ 
	public bool NewShieldPossible(int i){
		if (shieldArray [i] == null) {
			return true;
		} else {
			return false;
		}
	}

	/* Runs once at the start of the game to create al vectors for the spiral shape.
	 * These vectors are used to create the new shields. */
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
	/* returns the vector3 position of spiral joint position of the given index */
	public Vector3 GetJointPosition(int jointIndex) {
		return shieldJoints [jointIndex];
	}

	/* Generates and returns the shield object at the given id position. */
	public GameObject createShield(int i) {
		//Take position inbetween 2 shieldJoints
		Vector3 shieldPosition = (shieldJoints [i] + shieldJoints [i + 1])/2;
		Transform instance = (Transform)Instantiate(shieldPiece, shieldPosition, transform.rotation);
		instance.LookAt(shieldJoints [i]);
		instance.transform.Rotate(0, 90, 0);
		float scaleFactor = Vector3.Distance (shieldJoints [i], shieldJoints [i+1])/2;
		instance.gameObject.GetComponent<Shield>().SetShieldIndex(i);

		instance.parent = transform;
		return instance.gameObject;
	}

	/* Generates and returns the joint object at the given id position. */
	public GameObject createJoint(int i) {
		//Take position inbetween 2 shieldJoints
		Vector3 jointPosition;
		if (i == 0) {
			jointPosition = core.position;
		} else {
			jointPosition = jointArray[i-1].transform.position;
		}
		Transform newJoint = (Transform)Instantiate(shieldJoint, jointPosition, transform.rotation);
		newJoint.GetComponent<ShieldJoint> ().SetShieldJointIndex (i);
		newJoint.GetComponent<ShieldJoint> ().SetBaseScale(1.1f*(0.4f+(i*0.03f)));
		newJoint.name = "ShieldJoint" + i;
		return newJoint.gameObject;
	}

	/* Generates and returns the spiral pathfinding point next to the shieldjoint with the given Index */
	public GameObject createSpiralPathPoint(int i) {
		Vector3 pointPosition = shieldJoints[i+1]/(1.6f-(i*0.008f));
		GameObject newPathPoint = new GameObject();
		newPathPoint.name = "PathPoints" + i;
		newPathPoint.transform.position = pointPosition;
		newPathPoint.tag = "PathPoint";
		newPathPoint.transform.parent = path;
		newPathPoint.transform.LookAt(core);
		pathPointArray[i] = newPathPoint;
		return newPathPoint;
	}

	public int getSpiralPathMax() {
		if (lastShieldID>=1) {
			return lastShieldID-1;
		} else {
			return -1;
		}
	}
	
	public Vector3 getSpiralPathPosition(int index) {

		if (pathPointArray[index].transform!=null){
			return pathPointArray[index].transform.position;
		} else {
			return entrancePath.FindChild("EntrancePathPoint1").position;
		}

	}

	public Vector3 getEntrancePathPosition(int index){
		if ((index >= 1) && (index<=5)) {
			//print("Ask for position " + index + ", " + entrancePath.FindChild("EntrancePathPoint" + index).position);
			return entrancePath.FindChild("EntrancePathPoint" + index).position;
		} else {
			return Vector3.zero;
		}
	}

	void updateLevel(){
		//Check if the shield is continues and de-energize all seperate parts
		for (int i = 0; i <=shieldArray.Length-1; i++) {
			if ((shieldArray[i]==null)) {
				firstBrokenPos = i;
				break;
			}
		}
		//Update Level in HUD
		_hudManager.GetComponent<HUDManager>().updateLevel (firstBrokenPos);
	}

	public int AddEnergy(int amount){
		_energy += amount;
		_hudManager.GetComponent<HUDManager>().updateEnergy(_energy);
		return _energy;
	}

	public int GetEnergy(){
		return _energy;
	}

	// Update is called once per frame
	void Update () {
		updateLevel ();

		if (isCoreAlive()){

			//Pulse the central crystal
			pulseState = Mathf.Sin (Time.time*5);
			pulseState = 1f + pulseState / 5;
			//core.renderer.material.SetFloat("_RimPower", pulseState);


			if (damageEffect > 0) {
				damageEffect -= 0.1f * Time.deltaTime;
				Color currentColor = ((1-damageEffect)*coreColorDefault)+(damageEffect*coreColorDamage);
				core.renderer.material.SetFloat ("_RimPower", ((1-damageEffect)*pulseState)+(5));
				core.renderer.material.SetColor("_RimColor", Color.white);
			}
		}


	}

	void CorePulse(){
		ShieldPulse (0);
		core.GetComponent<PlayerController> ().corePulse ();
		HealCore ();
	}

	//Pulse shield pieces with delay
	public void ShieldPulse(int shieldIndex) {
		if (shieldArray[shieldIndex]!=null){
			shieldArray[shieldIndex].GetComponent<Shield>().EnergyPulse();
		}
	}

	void HealCore(){
		core.GetComponent<DamageController> ().Heal (4);
	}

	public bool isCoreAlive(){
		if (core != null) {
			return true;
		} else {
			return false;
		}
	}

	public Vector3 isInProximityOfShield(Vector3 p){
		int closestShieldIndex=0;
		float closestDistance=0;

		for (int i=0; i<totalShields-1; i++){
			if (shieldArray[i].transform!=null){
				float d = Vector3.Distance (shieldArray[i].transform.position, p);
				if (closestDistance==0) {
					closestDistance=d;
					closestShieldIndex = i;
				} else if (d < closestDistance){
					closestDistance = d;
					closestShieldIndex = i;
				}
			}
		}

		if (closestShieldIndex == 0) {
						return new Vector3 ();
				}

		Vector3 v = jointArray[closestShieldIndex].transform.position;
		Vector3 w = jointArray[closestShieldIndex-1].transform.position;

		Vector2 v2 = new Vector2 (v.x, v.z);
		Vector2 w2 = new Vector2 (w.x, w.z);
		Vector2 p2 = new Vector2 (p.x, p.z);


		Vector2 pointOnShield = pointOnSegmentClosestBy (p2, v2, w2);
		return new Vector3(pointOnShield.x, 0, pointOnShield.y);

	}

	public Transform GetShieldJoint(int index){
		return jointArray [index].transform;
	}

	float sqr(float x) {
		return x * x;
	}
	

	Vector2 pointOnSegmentClosestBy(Vector2 p, Vector2 v, Vector2 w) {
		float l2 = Vector2.Distance(v, w);
		if (l2 == 0) return v;
		float t = ((((p.x - v.x) * (w.x - v.x)) + ((p.y - v.y) * (w.y - v.y)))/l2) / l2;
		if (t < 0) return v;
		if (t > 1) return w;
		return new Vector2(v.x + t * (w.x - v.x), v.y + t * (w.y - v.y));
	}


}
