using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	private bool _enabled = true;
	private ButtonState _buttonState = ButtonState.Normal;

	public void OnClick(){
		if (_enabled) {

		}
	}

	public void Enabled(bool enabled){
		_enabled = enabled;
		updateButtonState ();
	}

	private void updateButtonState(){
		if (!_enabled) {
			_buttonState = ButtonState.Disabled;
		}
	}
}
