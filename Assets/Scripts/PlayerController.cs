using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	[Range(0f,0.2f)]
	public float pulseScale = 0.03f;

	private GameObject _coreDie;
	private Vector3 _coreTargetScale;
	private float _corePulseScale;

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
		float hP = GetComponent<DamageController> ().GetHitPoints();
		_coreTargetScale = new Vector3(hP / 100f,hP / 100f,hP / 100f);
		_corePulseScale -= 0.005f;
		Mathf.Max (_corePulseScale, 0f);
		_coreTargetScale += new Vector3(_corePulseScale,_corePulseScale,_corePulseScale);
		transform.localScale = Vector3.Lerp(transform.localScale, _coreTargetScale, 0.3f);



	}

	public void corePulse(){
		_corePulseScale = pulseScale;
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
