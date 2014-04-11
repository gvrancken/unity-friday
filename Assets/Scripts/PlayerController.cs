using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private GameObject _coreDie;

	// Use this for initialization
	void Start () {
		_coreDie = GameObject.Find ("CoreDie");
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Transform child in _coreDie.transform){
			if (child.gameObject.renderer.material.color.a>0f){
				child.gameObject.renderer.material.color -= new Color (0f, 0f, 0f, 0.02f);
			}
		}
	}

	public void GetDamage(){
		foreach (Transform child in _coreDie.transform){
			child.gameObject.renderer.material.color += new Color (0f, 0f, 0f, 1f);
		}
	}
	public void Die(){
		foreach (Transform child in _coreDie.transform){
			Camera.main.GetComponent<CameraController>().cameraZoomSpeed = 0.03f;
			Camera.main.GetComponent<CameraController>().setViewSizeTarget(100);
		}
	}

	void OnCollisionEnter (Collision col) {	
		DamageController dc = col.gameObject.GetComponent<DamageController>();
		dc.takeDamage(10);
	}
}
