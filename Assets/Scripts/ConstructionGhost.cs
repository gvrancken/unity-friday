using UnityEngine;
using System.Collections;

public class ConstructionGhost : MonoBehaviour {
	public Transform[] constructionTypeList;

	private ConstructionType _selectedConstructionType = ConstructionType.Empty;

	// Use this for initialization
	void Start () {
		ClearGhost ();
	}
	
	public void SetConstructionType(ConstructionType newType, bool enabled = true){
		_selectedConstructionType = newType;
		UpdateConstructionType();
	}

	//Switch Ghost model to an enabled (green) or disabled (red) state to indicate whether the construction can be build or not.
	public void setGhostEnabled(bool enabled){
		//TODO: switch construction ghost material to green or red.
	}

	//Disable/hide all ghost transforms
	void UpdateConstructionType(){
		ClearGhost ();
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
