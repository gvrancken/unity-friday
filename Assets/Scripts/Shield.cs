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
	private float growLength;
	private float growStartTime;
	private Vector3 anchorPostition;
	private Vector3 nextAnchorPostition;
	private Transform shieldJoint;
	private int shieldIndex;
	private float pulseStartTime;
	private bool puslePassedThrough;



	// Use this for initialization
	void Start () {
		shieldMesh.localScale = new Vector3(1, 0.01f, 0.3f );
		growScaleStart = new Vector3(1f, 0.01f, 0.1f);
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
			shieldJoint.gameObject.renderer.material.SetColor("_ColorTint", new Color(1,1,1,1));
			shieldJoint.gameObject.renderer.material.SetColor("_RimColor", colorEnergized);
			shieldMesh.gameObject.renderer.material.SetColor("_ColorTint", new Color(1,1,1,1));
			shieldMesh.gameObject.renderer.material.SetColor("_RimColor", colorEnergized);

		} else {
			shieldJoint.gameObject.renderer.material.SetColor("_ColorTint", colorEmpty);
			shieldJoint.gameObject.renderer.material.SetColor("_RimColor", colorEmpty);
			shieldMesh.gameObject.renderer.material.SetColor("_ColorTint", colorEmpty);
			shieldMesh.gameObject.renderer.material.SetColor("_RimColor", colorEmpty);

		}
	}


	public void EnergyPulse(){
		if (!isGrowing){
			pulseStartTime = Time.time;
			SetEnergized (true);
			puslePassedThrough = false;
		}
	}

	public void SetJoint(Transform joint) {
		shieldJoint = joint;
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
		float distCovered = (Time.time - growStartTime) * 1;
		float fracJourney = distCovered / growLength;
		shieldWall.localScale = Vector3.Lerp (growScaleStart, growScaleEnd, fracJourney);
		shieldJoint.localScale = new Vector3 (shieldWall.localScale.z*1.2f, 1f, shieldWall.localScale.z*1.2f);
		transform.GetComponent<BoxCollider> ().size = new Vector3(shieldWall.localScale.y*2, 1f, shieldWall.localScale.z);
		
		shieldWall.position = Vector3.Lerp (anchorPostition,transform.position,fracJourney);
		shieldJoint.position = Vector3.Lerp(anchorPostition,nextAnchorPostition,fracJourney);

		if (fracJourney>=1){
			isGrowing = false;
			enableBuildArea();
		}
	}

	public void SetDesitnationTransform(float length, Vector3 position) {
		print("index: " + shieldIndex);
		growScaleEnd = new Vector3 (1f, length, 0.3f+(shieldIndex*0.03f)); 
		growLength = Vector3.Distance(growScaleEnd, growScaleStart);
		anchorPostition = position;
		NewShieldCost = Mathf.RoundToInt(NewShieldCostsPerUnit * length);
	}

	public int GetNextShieldCosts(){
		return NewShieldCost;
	}

	public void StartGrowing() {
		growStartTime = Time.time;
		nextAnchorPostition = transform.parent.GetComponent<ShieldManager> ().GetJointPosition (shieldIndex+1);
		isGrowing = true;
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

		if (CanBuildNewShield()) {
			if (_colorEnergizedJoint!=colorEnergizedBuild){
				_colorEnergizedJoint = colorEnergizedBuild;
				shieldJoint.gameObject.renderer.material.SetColor("_RimColor", _colorEnergizedJoint);
			}
		} else {
			if (_colorEnergizedJoint!=colorEnergized){
				_colorEnergizedJoint = colorEnergized;
				shieldJoint.gameObject.renderer.material.SetColor("_RimColor", _colorEnergizedJoint);
			}
		}

		//update healthbar
		float hp = GetComponent<DamageController> ().GetHitPoints ();
		float scale = hp / 40;
		healthBar.localScale = new Vector3 (1, scale, 0.3f); 
		

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

				shieldJoint.gameObject.renderer.material.SetFloat ("_RimPower", ((1-damageEffect)*pulseState)+(0));
				currentColor = ((1-damageEffect)*_colorEnergizedJoint)+(damageEffect*colorDamage);
				shieldJoint.gameObject.renderer.material.SetColor("_RimColor", currentColor);

				shieldMesh.gameObject.renderer.material.SetFloat ("_RimPower", ((1-damageEffect)*pulseState)+(0));
				currentColor = ((1-damageEffect)*colorEnergized)+(damageEffect*colorDamage);
				shieldMesh.gameObject.renderer.material.SetColor("_RimColor", currentColor);

		}
	}

	void enableBuildArea() {
		float buildAreaScale = 0.7f * Vector3.Distance (Vector3.zero, shieldJoint.position);
		buildArea.localScale = new Vector3 (shieldMesh.localScale.y*2.9f,  buildAreaScale,1);
		buildArea.position += buildArea.TransformDirection (buildArea.forward) * 0.1f * buildAreaScale;
		buildArea.gameObject.SetActive (true);
	}



}