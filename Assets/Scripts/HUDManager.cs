using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour {

	public GUIText textLevel;
	public GUIText textEnergy;
	public Transform ConstructionGhost;
	public GUISkin ConstructionPanelSkin;

	private GameObject lm;
	private LevelManager lmscript;
	private GameObject player;
	private ShieldManager _shieldManager;

	// Use this for initialization
	void Start () {
		lm = GameObject.Find ("LevelManager");
		lmscript = lm.GetComponent<LevelManager>();
		player = GameObject.Find ("Player");
		_shieldManager = player.GetComponent<ShieldManager> ();

	}
	

	public void updateLevel (int amount) {
		textLevel.text = ("Level: " + amount);
	}

	public void updateEnergy (int amount) {
		textEnergy.text = ("Energy: " + amount);
	}
	
	
	void OnGUI() {
		for (int i=0; i<lmscript.constructionsList.Count; i++ ) 
		{
			Transform unit = lmscript.constructionsList[i];
			int constructionCost = 50;
			int y = 30 * i;
			bool canBuy = false;
			string selectedText = "";
			if (lmscript.selectedBuildConstruct == unit){
				selectedText = ">> ";
			}
			string canBuyText = " (costs: " + constructionCost + ")";
			GUI.skin = ConstructionPanelSkin;
			GUI.color = Color.red;
			if (_shieldManager.GetEnergy() >= constructionCost) {
				canBuy = true;
				canBuyText = " (costs: 50)";
				GUI.color = Color.green;
			}
			ConstructionGhost.GetComponent<ConstructionGhost>().setGhostCostsEnabled(canBuy);
			if (GUI.Button(new Rect(Screen.width-250,30+y,230,30), selectedText + unit.name + canBuyText)) {
				//if (canBuy) {
				lmscript.selectedBuildConstruct = unit;

				ConstructionGhost.GetComponent<ConstructionGhost>().SetConstructionType((ConstructionType)i);
				//}
			}
		}
	}


}
