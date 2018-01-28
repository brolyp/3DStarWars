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
	public float maxZOffset = -10f;
	public float minZOffset = -2f;

	private float _rotSpeed;

	// Use this for initialization
	void Start () {
		_camera = transform.GetChild(0);
		_player = GetComponent < PlayerControl> ();
		_rotSpeed = _player.PlayerRotSpeed;
		_offset = new Vector3(0, 0, 0);
	}

	// Update is called once per frame
	void Update() {
		if(Input.mouseScrollDelta.y > 0){
			zOffset += 1;
			if(zOffset > maxZOffset){
				zOffset = maxZOffset;
			}
		} else if(Input.mouseScrollDelta.y < 0){
			zOffset -= 1;
			if(zOffset < minZOffset){
				zOffset = minZOffset;
			}
		}
	}

	void LateUpdate () {
		//Vector3 cPos = _camera.localPosition;
		_offset = Quaternion.AngleAxis (Input.GetAxis ("Mouse X") * _rotSpeed, Vector3.up) * _offset;
		_camera.position = transform.position + _offset + new Vector3(0, yOffset, zOffset);
		_camera.LookAt(transform.position);
	}
}
