using UnityEngine;
using System.Collections;

public class DamageController : MonoBehaviour {

	public float MaxHitPoints = 1;
	public float dieTime = 0.01f;

	private float _hitpoints;
	private bool _isDead = false;

	public float barDisplay; //current progress
	public Vector2 pos = new Vector2(120,40);
	public Vector2 size = new Vector2(60,10);
	public Texture2D emptyTex;
	public Texture2D fullTex;

	void Start () {
		_hitpoints = MaxHitPoints;
	}

	void OnGUI() {
		//draw the background:
		//levelManager.GUIDrawRect(new Rect(pos.x, pos.y, size.x, size.y), , 
	}

	void Update() {
		//for this example, the bar display is linked to the current time,
		//however you would set this value based on your desired display
		//eg, the loading progress, the player's health, or whatever.
		barDisplay = 100;
		//   barDisplay = MyControlScript.staticHealth;
	}

	public void takeDamage(float damagePoints) {
		_hitpoints -= damagePoints;


		if (_isDead == false && _hitpoints <= 0) {
			_isDead = true;
			Destroy  (gameObject, dieTime);
		}

	}


}
