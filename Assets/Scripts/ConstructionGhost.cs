using UnityEngine;
using System.Collections;

public class ConstructionGhost : MonoBehaviour {
	public Transform[] constructionTypeList;

	private ConstructionType _selectedConstructionType = ConstructionType.Empty;
	private bool _constructionBuildableEnabled;
	private bool _constructionCostsEnabled;
	private bool _constructionEnabled;

	// Use this for initialization
	void Start () {
		ClearGhost ();
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
	}

	void updateGhostColor(){
		Color ghostColor = Color.green;
		if (!_constructionEnabled) {
			ghostColor = Color.red;
		}
		foreach (Transform child in constructionTypeList [(int)_selectedConstructionType]){
			child.gameObject.renderer.material.color = ghostColor;
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
