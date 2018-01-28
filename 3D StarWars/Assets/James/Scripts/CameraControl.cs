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
	private float _rotSpeed;

	// Use this for initialization
	void Start () {
		_camera = transform.GetChild(0);
		_player = GetComponent < PlayerControl> ();
		_rotSpeed = _player.PlayerRotSpeed;
		_offset = new Vector3(0, yOffset, zOffset);
	}

	// Update is called once per frame
	void LateUpdate () {
		//Vector3 cPos = _camera.localPosition;
		_offset = Quaternion.AngleAxis (Input.GetAxis ("Mouse X") * _rotSpeed, Vector3.up) * _offset;
		_camera.position = transform.position + _offset; 
		transform.LookAt(transform.position);
	}
}
