using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public int damagePoints = 10;
	public ParticleSystem explosion;
	public Transform healthBar;
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

	}

	public void setShieldWall(Transform t) {
		shieldWall = t;
		growScaleStart = new Vector3(1f, 0.01f, 0.1f);
	}
	
	void OnCollisionEnter (Collision col) {	
		DamageController dc = col.gameObject.GetComponent<DamageController>();
		dc.takeDamage(damagePoints);
			foreach (Transform child in transform) {
				child.gameObject.renderer.material.SetColor("_RimColor", colorDamage);
			}
		damageEffect = 1;

		ParticleSystem explosionInst = (ParticleSystem)Instantiate(explosion, transform.position, transform.rotation);
		explosionInst.Play();
		Destroy(explosionInst.gameObject,1);
	}

	public void SetEnergized(bool state){
		isEnergized = state;
		if (isEnergized) {
			GetComponent<DamageController>().Heal(3);
			foreach (Transform child in transform) {
				child.gameObject.renderer.material.SetColor("_ColorTint", new Color(1,1,1,1));
				child.gameObject.renderer.material.SetColor("_RimColor", colorEnergized);
			}
		} else {
			foreach (Transform child in transform) {
				child.gameObject.renderer.material.SetColor("_ColorTint", colorEmpty);
				child.gameObject.renderer.material.SetColor("_RimColor", colorEmpty);
			}
		}
	}


	public void EnergyPulse(){
		pulseStartTime = Time.time;
		SetEnergized (true);
		puslePassedThrough = false;

	

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
		transform.GetComponent<BoxCollider> ().size = new Vector3(shieldWall.localScale.y*2, 1f, 0.5f);
		
		shieldWall.position = Vector3.Lerp (anchorPostition,transform.position,fracJourney);
		shieldJoint.position = Vector3.Lerp(anchorPostition,nextAnchorPostition,fracJourney);

		if (fracJourney>=1){
			isGrowing = false;
		}
	}

	public void SetDesitnationTransform(float length, Vector3 position) {
		growScaleEnd = new Vector3 (1f, length, 0.3f); 
		growLength = Vector3.Distance(growScaleEnd, growScaleStart);
		anchorPostition = position;
		NewShieldCost = Mathf.RoundToInt(NewShieldCostsPerUnit * length);
		print (transform.name + " - costs: " + NewShieldCost);
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
				transform.FindChild("ShieldJoint").gameObject.renderer.material.SetColor("_RimColor", _colorEnergizedJoint);
			}
		} else {
			if (_colorEnergizedJoint!=colorEnergized){
				_colorEnergizedJoint = colorEnergized;
				transform.FindChild("ShieldJoint").gameObject.renderer.material.SetColor("_RimColor", _colorEnergizedJoint);
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

		foreach (Transform child in transform) {
			child.gameObject.renderer.material.SetFloat ("_RimPower", pulseState);
		}


		if (damageEffect > 0) {
			damageEffect -= 0.1f;
			Color currentColor;
			//Debug.Log("damage effect = " + damageEffect);
			foreach (Transform child in transform) {
				if (child.name == "ShieldJoint"){
					print ("feuifwuiegfw");
					currentColor = ((1-damageEffect)*_colorEnergizedJoint)+(damageEffect*colorDamage);
				} else {
					currentColor = ((1-damageEffect)*colorEnergized)+(damageEffect*colorDamage);
				}
				child.gameObject.renderer.material.SetFloat ("_RimPower", ((1-damageEffect)*pulseState)+(0));
				child.gameObject.renderer.material.SetColor("_RimColor", currentColor);
			}
		}
	}



}