using UnityEngine;
using System.Collections;

public class ShieldJoint : MonoBehaviour {
	


	public void OnClick() {
		transform.parent.GetComponent<Shield> ().CreateNewShield ();

	}

	public void OnMouseOver(){
		this.gameObject.renderer.material.color = Color.red;
	}
}
