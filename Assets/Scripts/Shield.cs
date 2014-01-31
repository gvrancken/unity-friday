using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public int damagePoints = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter (Collision col) {	


			DamageController dc = col.gameObject.GetComponent<DamageController>();
			dc.takeDamage(damagePoints);
		foreach (Transform child in transform) {
			//Debug.Log ("hit!");
			child.gameObject.renderer.material.color -= new Color (0.01f, 0.2f, 0.2f, 0.2f);
		}
	}

	void Update () {
		//DamageController dc = GetComponent<DamageController>();
		//float HP = dc.getHP();

	}


}