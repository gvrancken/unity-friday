using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public int damagePoints = 10;
	public ParticleSystem explosion;
	public Color colorEnergized = new Color (0.3f, 0.3f, 1f, 1f);	 
	public Color colorEmpty = new Color (0.4f, 0.4f, 0.4f, 0.8f);	
	public Color colorDamage = new Color (1.0f, 0, 0, 1);

	private bool energized = false;
	private float damageEffect = 0;
	private float pulseState = 0;

	// Use this for initialization
	void Start () {

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

	public void setEnergized(bool state){
		energized = state;
		if (energized) {
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

	public void setPulse(float p){
		pulseState = 0.6f + p / 2;
	}

	void Update(){
		foreach (Transform child in transform) {
			child.gameObject.renderer.material.SetFloat ("_RimPower", pulseState);
		}

		if (damageEffect > 0) {
			damageEffect -= 0.1f;
			//Debug.Log("damage effect = " + damageEffect);
			Color currentColor = ((1-damageEffect)*colorEnergized)+(damageEffect*colorDamage);
			foreach (Transform child in transform) {
				child.gameObject.renderer.material.SetFloat ("_RimPower", ((1-damageEffect)*pulseState)+(0));
				child.gameObject.renderer.material.SetColor("_RimColor", currentColor);
			}

		}
	}



}