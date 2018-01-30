using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	private Vector3 _offset;
	private Transform _camera;
	private PlayerControl _player;
	private float rotSpeed;
	private float _cameraAngle;

	public float yOffset = 2.0f;
	public float zOffset = 0.0f;
	public float minZOffset = -10f;
	public float maxZOffset = -2f;

	private float _hRotSpeed;
	public float VerticalRotationSpeed;

	// Use this for initialization
	void Start () {
		_camera = transform.GetChild(0);
		_player = GetComponent < PlayerControl> ();
		_hRotSpeed = _player.PlayerRotSpeed;
		_offset = new Vector3(0, yOffset, zOffset);
	}

	// Update is called once per frame
	void Update() {
		
	}

	void LateUpdate () {
		_cameraAngle = Input.GetAxis ("Mouse Y");

		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			zOffset += 1f;
			Debug.Log ("Up" + zOffset);
			if (zOffset >= maxZOffset) {
				zOffset = maxZOffset - 1;
			} else {
				Debug.Log ("changing MW");
				_offset += _offset.normalized;
			}
		} else if(Input.GetAxis("Mouse ScrollWheel") < 0){
			zOffset -= 1f;
			Debug.Log ("Down" + zOffset);
			if (zOffset <= minZOffset) {
				zOffset = minZOffset + 1;
			} else {
				Debug.Log ("changing MW");
				_offset -= _offset.normalized;
			}
		}
		//Vector3 cPos = _camera.localPosition;
		_offset= Quaternion.AngleAxis (Input.GetAxis ("Mouse X") * _hRotSpeed, Vector3.up) * _offset;
		_offset = Quaternion.AngleAxis (Input.GetAxis ("Mouse Y") * VerticalRotationSpeed, transform.right) * _offset;
		_camera.position = transform.position + _offset;
		_camera.LookAt(transform.position);
	}
}
