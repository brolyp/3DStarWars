using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	private Vector3 _offset;
	private Transform _camera;
	private PlayerControl _player;
	private float rotSpeed;
	private float _cameraAngle;


	public float CameraDistance = 0.0f;
	public float maxCameraDistance = 10f;
	public float minCameraDistance = 2f;

	private float _hRotSpeed;
	public float VerticalRotationSpeed;
	private float _horizontal;
	private float _vertical;

	// Use this for initialization
	void Start () {
		_camera = transform.GetChild(0);
		_player = GetComponent < PlayerControl> ();
		_hRotSpeed = _player.PlayerRotSpeed;

	}

	// Update is called once per frame
	void Update() {
		_horizontal += Input.GetAxis ("Mouse X") * _hRotSpeed;
		_vertical -= Input.GetAxis ("Mouse Y") * VerticalRotationSpeed;
	}

	void LateUpdate () {

		if(Input.GetAxis("Mouse ScrollWheel") < 0){
			CameraDistance += 1f;
			Debug.Log ("Up" + CameraDistance);
			if (CameraDistance >= maxCameraDistance) {
				CameraDistance = maxCameraDistance;
			}
		} else if(Input.GetAxis("Mouse ScrollWheel") > 0){
			CameraDistance -= 1f;
			Debug.Log ("Down" + CameraDistance);
			if (CameraDistance <= minCameraDistance) {
				CameraDistance = minCameraDistance;
			}
		}
		_vertical = Mathf.Clamp(_vertical, -80f, 80f);
		//Quaternion xRot = Quaternion.AngleAxis (_horizontal, Vector3.up);
		Quaternion yRot = Quaternion.AngleAxis (_vertical, transform.right);
		_camera.position = transform.position + yRot * transform.forward * -CameraDistance;
		_camera.LookAt(transform.position);
	}
}
