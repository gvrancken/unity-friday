using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour {

	public GUIText textEnergy;
	private GameObject lm;
	private LevelManager lmscript;

	// Use this for initialization
	void Start () {
		lm = GameObject.Find ("LevelManager");
		lmscript = lm.GetComponent<LevelManager>();

	}
	
	// Update is called once per frame
	public void updateEnergy (int amount) {
		textEnergy.text = ("Energy: " + amount);
	}

	void OnGUI() {
		for (int i=0; i<lmscript.constructionsList.Count; i++ ) 
		{
			Transform unit = lmscript.constructionsList[i];
			int y = 22 * i;
			if (GUI.Button(new Rect(Screen.width-120,20+y,100,20), unit.name)) {
				lmscript.selectedBuildConstruct = unit;
			}
		}
	}


}
