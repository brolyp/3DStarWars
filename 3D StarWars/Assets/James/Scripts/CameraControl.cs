using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	Vector3 _offset;
	Vector3 _parentPosition;
	public float RotationSpeed;

	public float yOffset = 2.0f;
	public float zOffset = 4.0f;

	// Use this for initialization
	void Start () {
		_parentPosition = transform.parent.position;
		_offset = new Vector3(_parentPosition.x, _parentPosition.y + yOffset, _parentPosition.z + zOffset);
	}
	
	// Update is called once per frame
	void Update () {
		_parentPosition = transform.parent.position;
		_offset = Quaternion.AngleAxis (Input.GetAxis ("Mouse X") * RotationSpeed, Vector3.up) * _offset;
		transform.position = _parentPosition + _offset; 
		transform.LookAt(transform.position);
	}
}
