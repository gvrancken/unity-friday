using UnityEngine;
using System.Collections;

public class ShieldJoint : MonoBehaviour {

	private bool mouseDown = false;


	public void OnClick() {
		transform.parent.GetComponent<Shield> ().CreateNewShield ();

	}

	public void OnMouseOver(){
		this.gameObject.renderer.material.color = Color.red;
	}
}
