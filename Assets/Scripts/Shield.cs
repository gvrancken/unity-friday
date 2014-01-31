using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public int MaxHitPoints;
	private int hitPoints;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "Enemy")
		{
			transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			col.gameObject.GetComponent<destructibleObject>.damage(10);
		}
	}


}