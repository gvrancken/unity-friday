using UnityEngine;
using System.Collections;

public class ConstructionGhost : MonoBehaviour {
	public Transform[] constructionTypeList;

	private ConstructionType _selectedConstructionType = ConstructionType.Empty;
	private bool _constructionBuildableEnabled;
	private bool _constructionCostsEnabled;
	private bool _constructionEnabled;
	private GameObject _player;
	private ShieldManager _shieldManager;
	private bool _lineRenderEnabled;

	// Use this for initialization
	void Start () {
		_player = GameObject.Find ("Player");
		_shieldManager = _player.GetComponent<ShieldManager> ();
		ClearGhost ();
	}

	void Update(){
		if (_lineRenderEnabled) {
			Vector3 _v = _shieldManager.isInProximityOfShield(transform.position);
			transform.GetComponent<LineRenderer>().SetPosition(0, transform.position);
			transform.GetComponent<LineRenderer>().SetPosition(1, _v);
		}
	}
	
	public void SetConstructionType(ConstructionType newType){
		if (_selectedConstructionType != newType) {
			_selectedConstructionType = newType;
			UpdateConstructionType();
		}
	}

	//Set whether the construction can be build or not.
	public void setGhostBuildableEnabled(bool enabled){
		if (_constructionBuildableEnabled != enabled){
			_constructionBuildableEnabled = enabled;
			updateConstructionEnabled();

		}

	}

	//Set whether the construction can be build or not.
	public void setGhostCostsEnabled(bool enabled){
		if (_constructionCostsEnabled != enabled){
			_constructionCostsEnabled = enabled;
			updateConstructionEnabled();
		}
	}

	void updateConstructionEnabled(){

		_constructionEnabled = _constructionBuildableEnabled && _constructionCostsEnabled;
		if (_selectedConstructionType>=0){
			updateGhostColor();
		}

			_lineRenderEnabled = _constructionEnabled && (_selectedConstructionType>=0);
		transform.GetComponent<LineRenderer> ().enabled = _lineRenderEnabled;
	
	}

	void updateGhostColor(){
		Color ghostColor = Color.green;
		if (!_constructionEnabled) {
			ghostColor = Color.red;
		}
		if (_selectedConstructionType>=0){
			foreach (Transform child in constructionTypeList [(int)_selectedConstructionType]){
				child.gameObject.renderer.material.color = ghostColor;
				print ("wordt groen!");
			}
		}
	}

	//Disable/hide all ghost transforms
	void UpdateConstructionType(){
		ClearGhost ();
		updateGhostColor ();
		if (_selectedConstructionType!=ConstructionType.Empty){
			constructionTypeList [(int)_selectedConstructionType].gameObject.SetActive (true);
			
		}
	}

	void ClearGhost(){
		foreach (Transform child in transform) {
			child.gameObject.SetActive(false);	
		}
	}


}
