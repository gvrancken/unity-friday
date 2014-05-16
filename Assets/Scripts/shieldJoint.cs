using UnityEngine;
using System.Collections;

public class ShieldJoint : MonoBehaviour {

	private Vector3 _basePosition;
	private int _seed;
	private float _noiseScale = 0.3f;
	private Shield shieldBack;
	private float _baseScale;
	private float _pulseScale;
	private float _targetScale;
	private int _shieldJointIndex;

	void Start(){
		_seed = Mathf.RoundToInt(Random.value * 9999);
		_basePosition = transform.position;
		_pulseScale = _baseScale * 1.3f;
	}

	public void SetShieldBack(Shield shield){
		print ("set shieldback " + shield.name);
		shieldBack = shield;
	}

	public void SetBaseScale(float scale){
		_baseScale = scale;
		print("Set base scale: " + _baseScale);

	}

	void Update(){
		float noiseX = Mathf.PerlinNoise (Time.time, Time.time)*_noiseScale*(_targetScale-_baseScale);
		float noiseZ = Mathf.PerlinNoise (Time.time+_seed, Time.time+_seed)*_noiseScale*(_targetScale-_baseScale);
		transform.position = _basePosition + new Vector3 (noiseX, 0, noiseZ);
		_targetScale -= 0.005f;
		_targetScale = Mathf.Max (_targetScale, _baseScale);
		transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(_targetScale,_targetScale,_targetScale), 0.3f);
		print("base scale: " + _baseScale);
	}

	public void OnClick() {
		shieldBack.CreateNewShield ();

	}

	public void OnMouseOver(){
		gameObject.renderer.material.color = Color.red;
	}

	public void EnergyPulse(){
		_pulseScale = _baseScale * 1.3f;
		_targetScale = _pulseScale;
	}

	public void SetShieldJointIndex(int index){
		_shieldJointIndex = index;
	}

	public void SetBasePosition(Vector3 position){
		_basePosition = position;
	}
}
