using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public int damagePoints = 10;
	public ParticleSystem explosion;
	// Use this for initialization
	void Start () {


	}
	
	void OnCollisionEnter (Collision col) {	


			DamageController dc = col.gameObject.GetComponent<DamageController>();
			dc.takeDamage(damagePoints);
		foreach (Transform child in transform) {
			//Debug.Log ("hit!");
			child.gameObject.renderer.material.color -= new Color (0.01f, 0.2f, 0.2f, 0.2f);

			ParticleSystem explosionInst = (ParticleSystem)Instantiate(explosion, transform.position, transform.rotation);
			explosionInst.Play();
			Destroy(explosionInst.gameObject,1);
		}
	}

	void Update () {
		//DamageController dc = GetComponent<DamageController>();
		//float HP = dc.getHP();

	}


}