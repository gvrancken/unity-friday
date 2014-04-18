using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	[Range(0.01f, 1f)]
	public float cameraZoomSpeed = 0.2f;
	public Transform topRightPanel;

	private float _viewSizeMin = 3;
	private float _viewSizeMax = 10;
	private float _viewSizeCurrent = 20;
	private float _viewSizeTarget = 8;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//temp mouse scroll camera zoom
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
		{
			_viewSizeTarget = Mathf.Max(_viewSizeCurrent-1, _viewSizeMin);
			
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
		{
			_viewSizeTarget = Mathf.Min(_viewSizeCurrent+1, _viewSizeMax);
		}
		if (Mathf.Abs(_viewSizeCurrent - _viewSizeTarget)>0.05) {
			_viewSizeCurrent -= cameraZoomSpeed*(_viewSizeCurrent-_viewSizeTarget);
			updateCameraOrthographicSize();
		} else {
			_viewSizeCurrent = _viewSizeTarget;
		}

	}

	public void setMaxViewSize(float viewSize){
		if (viewSize<10) {
			return;
		}
		_viewSizeMax = viewSize;
		if (_viewSizeMax-_viewSizeCurrent < 3f) {
			_viewSizeTarget = _viewSizeMax;
		}
	}

	public void setViewSizeTarget(float viewSizeTarget){
		if (_viewSizeTarget>_viewSizeMax){
			_viewSizeMax = _viewSizeTarget;
		}
		_viewSizeTarget = viewSizeTarget;
	}

	public void setMinViewSize(float viewSize){
		_viewSizeMin = viewSize;
	}

	void updateCameraOrthographicSize(){
		Camera.main.orthographicSize = _viewSizeCurrent;
		//updateHUDPanels();
	}

	void updateHUDPanels(){
		topRightPanel.position = new Vector3(_viewSizeCurrent*1.2f, 10f, 5);
	}
}


