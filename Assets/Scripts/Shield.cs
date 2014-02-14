using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public int damagePoints = 10;
	public ParticleSystem explosion;
	public Color colorEnergized = new Color (0.3f, 0.3f, 1f, 1f);	
	public Color colorEmpty = new Color (0.4f, 0.4f, 0.4f, 0.8f);	

	private bool energized = false;

	// Use this for initialization
	void Start () {

	}
	
	void OnCollisionEnter (Collision col) {	
		DamageController dc = col.gameObject.GetComponent<DamageController>();
		dc.takeDamage(damagePoints);
		foreach (Transform child in transform) {
			//Debug.Log ("hit!");
			//child.gameObject.renderer.material.color -= new Color (0.01f, 0.2f, 0.2f, 0.2f);
		}
		ParticleSystem explosionInst = (ParticleSystem)Instantiate(explosion, transform.position, transform.rotation);
		explosionInst.Play();
		Destroy(explosionInst.gameObject,1);
	}

	public void setEnergized(bool state){
		energized = state;
		if (energized) {
			foreach (Transform child in transform) {
				child.gameObject.renderer.material.color = new Color (0.3f, 0.3f, 1f, 1f);	
			}
		} else {
			foreach (Transform child in transform) {
				child.gameObject.renderer.material.color = new Color (0.4f, 0.4f, 0.4f, 0.8f);	
			}
		}
	}


}