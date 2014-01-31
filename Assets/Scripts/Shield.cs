using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public int damagePoints;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter (Collision col) {	
		Debug.Log ("hit!");
		if (col.gameObject.tag != "Enemy") {
			DamageController dc = col.gameObject.GetComponent<DamageController>();
			dc.takeDamage(damagePoints);
		}
	}


}