using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public int damagePoints = 10;
	public ParticleSystem explosion;
	public Transform shieldMesh;
	public Transform healthBar;
	public Transform buildArea;
	public Color colorEnergized = new Color (0.3f, 0.3f, 1f, 1f);	 
	public Color colorEnergizedBuild = new Color (0.3f, 0.8f, 0.6f, 1f);
	public Color colorEmpty = new Color (0.4f, 0.4f, 0.4f, 0.8f);	
	public Color colorDamage = new Color (1.0f, 0, 0, 1);
	public int NewShieldCostsPerUnit = 2;


	private int NewShieldCost;
	private bool isEnergized = false;
	private float damageEffect = 0;
	private float pulseState = 0;
	private bool isGrowing;
	private Vector3 growScaleStart;
	private Vector3 growScaleEnd;
	private Transform shieldWall;
	private float growDistance;
	private float growStartTime;
	private Vector3 anchorPostition;
	private Vector3 nextAnchorPostition;
	private Transform shieldJointStart;
	private Transform shieldJointEnd;
	private int shieldIndex;
	private float pulseStartTime;
	private bool puslePassedThrough;



	// Use this for initialization
	void Start () {
		shieldMesh.localScale = new Vector3(0.01f, 0.01f, 0.3f );
		growScaleStart = new Vector3(0.01f, 0.01f, 0.1f);
		shieldWall = shieldMesh;
	}

	public void setShieldWall(Transform t) {
		shieldWall = t;

	}
	
	void OnCollisionEnter (Collision col) {	
		DamageController dc = col.gameObject.GetComponent<DamageController>();
		dc.takeDamage(damagePoints);
		shieldMesh.gameObject.renderer.material.SetColor("_RimColor", colorDamage);

		damageEffect = 1;

		ParticleSystem explosionInst = (ParticleSystem)Instantiate(explosion, transform.position, transform.rotation);
		explosionInst.Play();
		Destroy(explosionInst.gameObject,1);
	}

	public void SetEnergized(bool state){
		isEnergized = state;
		if (isEnergized) {
			GetComponent<DamageController>().Heal(3);
			shieldJointEnd.gameObject.renderer.material.SetColor("_ColorTint", new Color(1,1,1,1));
			shieldJointEnd.gameObject.renderer.material.SetColor("_RimColor", colorEnergized);
			shieldMesh.gameObject.renderer.material.SetColor("_ColorTint", new Color(1,1,1,1));
			shieldMesh.gameObject.renderer.material.SetColor("_RimColor", colorEnergized);

		} else {
			shieldJointEnd.gameObject.renderer.material.SetColor("_ColorTint", colorEmpty);
			shieldJointEnd.gameObject.renderer.material.SetColor("_RimColor", colorEmpty);
			shieldMesh.gameObject.renderer.material.SetColor("_ColorTint", colorEmpty);
			shieldMesh.gameObject.renderer.material.SetColor("_RimColor", colorEmpty);

		}
	}


	public void EnergyPulse(){
		shieldJointEnd.GetComponent<ShieldJoint>().EnergyPulse ();
		if (!isGrowing){
			pulseStartTime = Time.time;
			SetEnergized (true);
			puslePassedThrough = false;
		}
	}

	public void SetEndJoint(Transform joint) {
		Debug.Log ("SetEndJoint");
		shieldJointEnd = joint;
		joint.gameObject.GetComponent<ShieldJoint>().SetShieldBack (this);
	}

	public void SetStartJoint(Transform joint) {
		shieldJointStart = joint;
	}

	public void SetShieldIndex(int i) {
		shieldIndex = i;
	}

	public void CreateNewShield(){
		if (isEnergized) {
			int _energy = transform.parent.GetComponent<ShieldManager>().GetEnergy();
			if (_energy >= NewShieldCost) {
				transform.parent.GetComponent<ShieldManager> ().CreateNewShield (shieldIndex + 1);
				transform.parent.GetComponent<ShieldManager> ().AddEnergy(-NewShieldCost);
			}
		}
	}

	void Growing() {
		float distCovered = (Time.time - growStartTime) * 0.1f;
		float fracJourney = distCovered * 2f;	

		//shieldWall.position = Vector3.Lerp (anchorPostition,transform.position,fracJourney);
		shieldJointEnd.GetComponent<ShieldJoint>().SetBasePosition(Vector3.Lerp(anchorPostition,nextAnchorPostition,fracJourney));

		if (fracJourney>=1){
			isGrowing = false;
			enableBuildArea();
		}
	}

	void AttachToJoints() {

		transform.position = (shieldJointEnd.position + shieldJointStart.position)/2;
		float scaleFactor = Vector3.Distance (shieldJointEnd.position, shieldJointStart.position)/2;
		shieldWall.localScale = new Vector3 (0.3f+(shieldIndex*0.03f), scaleFactor, 0.3f+(shieldIndex*0.03f)); 
		transform.LookAt (shieldJointEnd);
		transform.Rotate(0, 90, 0);
		updateCollider ();
	}

	void updateCollider(){
		transform.GetComponent<BoxCollider> ().size = new Vector3(shieldWall.localScale.y, shieldWall.localScale.x, shieldWall.localScale.z);
	}

	public void SetEndJointPosition(Vector3 position) {
		growDistance = Vector3.Distance(position, shieldJointStart.position);
		anchorPostition = position;
		NewShieldCost = Mathf.RoundToInt(NewShieldCostsPerUnit * growDistance);
	}

	public int GetNextShieldCosts(){
		return NewShieldCost;
	}

	public void StartGrowing() {
		growStartTime = Time.time;
		nextAnchorPostition = transform.parent.GetComponent<ShieldManager> ().GetJointPosition (shieldIndex+1);
		isGrowing = true;
	}

	public void StopGrowing() {
		isGrowing = false;
	}

	bool CanBuildNewShield(){
		if (isEnergized) {
			if (transform.parent.GetComponent<ShieldManager>().NewShieldPossible(shieldIndex+1)){
				int _energy = transform.parent.GetComponent<ShieldManager>().GetEnergy();
					if (_energy >= NewShieldCost) {
						return true;
					}
			}
		}
		return false;
	}

	void Update(){
		Color _colorEnergizedJoint = colorEnergized;

		//Is this shield just created and still growing?
		if (isGrowing) {
			Growing();
		}

		AttachToJoints();


		if (CanBuildNewShield()) {
			if (_colorEnergizedJoint!=colorEnergizedBuild){
				_colorEnergizedJoint = colorEnergizedBuild;
				shieldJointEnd.gameObject.renderer.material.SetColor("_RimColor", _colorEnergizedJoint);
			}
		} else {
			if (_colorEnergizedJoint!=colorEnergized){
				_colorEnergizedJoint = colorEnergized;
				shieldJointEnd.gameObject.renderer.material.SetColor("_RimColor", _colorEnergizedJoint);
			}
		}

		//update healthbar
		float hp = GetComponent<DamageController> ().GetHitPoints ();
		float scale = hp / 40;
		healthBar.localScale = new Vector3 (1, scale, 0.6f); 
		

		float timeSincePulse = Time.time - pulseStartTime;
		pulseState = timeSincePulse;
		if ((timeSincePulse>0.05)&&!puslePassedThrough) {
			if (!isGrowing){
				transform.parent.GetComponent<ShieldManager> ().ShieldPulse (shieldIndex+1);
			}
			puslePassedThrough = true;
		}
		if (timeSincePulse>1.4){
			SetEnergized(false);
		}


			shieldMesh.gameObject.renderer.material.SetFloat ("_RimPower", pulseState);



		if (damageEffect > 0) {
			damageEffect -= 0.1f;
			Color currentColor;
			//Debug.Log("damage effect = " + damageEffect);

				shieldJointEnd.gameObject.renderer.material.SetFloat ("_RimPower", ((1-damageEffect)*pulseState)+(0));
				currentColor = ((1-damageEffect)*_colorEnergizedJoint)+(damageEffect*colorDamage);
				shieldJointEnd.gameObject.renderer.material.SetColor("_RimColor", currentColor);

				shieldMesh.gameObject.renderer.material.SetFloat ("_RimPower", ((1-damageEffect)*pulseState)+(0));
				currentColor = ((1-damageEffect)*colorEnergized)+(damageEffect*colorDamage);
				shieldMesh.gameObject.renderer.material.SetColor("_RimColor", currentColor);

		}
	}

	void enableBuildArea() {
		float buildAreaScale = 0.7f * Vector3.Distance (Vector3.zero, shieldJointEnd.position);
		buildArea.localScale = new Vector3 (shieldMesh.localScale.y*2.9f,  buildAreaScale,1);
		buildArea.position += buildArea.TransformDirection (buildArea.forward) * 0.1f * buildAreaScale;
		buildArea.gameObject.SetActive (true);
	}



}