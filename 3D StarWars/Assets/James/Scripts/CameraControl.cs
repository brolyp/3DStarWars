using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	Vector3 _offset;
	Transform _camera;
	PlayerControl _player;
	float rotSpeed;
	public float yOffset = 2.0f;
	public float zOffset = 0.0f;
	public float minZOffset = -10f;
	public float maxZOffset = -2f;

	private float _rotSpeed;

	// Use this for initialization
	void Start () {
		_camera = transform.GetChild(0);
		_player = GetComponent < PlayerControl> ();
		_rotSpeed = _player.PlayerRotSpeed;
		_offset = new Vector3(0, yOffset, zOffset);
	}

	// Update is called once per frame
	void Update() {
		
	}

	void LateUpdate () {
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
		_offset = Quaternion.AngleAxis (Input.GetAxis ("Mouse X") * _rotSpeed, Vector3.up) * _offset;
		_camera.position = transform.position + _offset;
		_camera.LookAt(transform.position);
	}
}
