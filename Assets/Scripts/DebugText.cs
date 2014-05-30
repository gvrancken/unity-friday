using UnityEngine;
using System.Collections;

public class DebugText : MonoBehaviour {
	private Vector2 _position;
	private string _debugText = "";
	private GUIStyle _style = new GUIStyle();
	public bool showDebugText;

	void Start(){

	}

	void OnGUI() {
		if (showDebugText) {
			_position = Camera.main.WorldToScreenPoint (transform.position);
			_position += new Vector2 (1, 1);
			string debugText = GUI.TextArea (new Rect(_position.x,Screen.height-_position.y, 180, 20) , _debugText);
		}

	}

	public void setDebugText(string text){
		_debugText = text;
	}
}
