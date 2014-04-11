using UnityEngine;
using System.Collections;

public class ConstructionGhost : MonoBehaviour {
	public Transform[] constructionTypeList;

	private ConstructionType _selectedConstructionType = ConstructionType.Empty;

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) {
			child.gameObject.SetActive(false);	
		}
	}
	
	void SwitchConstructionType(ConstructionType newType){

	}
}
