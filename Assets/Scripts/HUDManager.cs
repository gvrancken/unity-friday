using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

	public GUIText textEnergy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void updateEnergy (int amount) {
		textEnergy.text = ("Energy: " + amount);
	}
}
