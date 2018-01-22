using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float PlayerRotSpeed = 500.0f;
	public float PlayerMoveSpeed = 10.0f;
	public float JumpHeight = 10.0f;
	public float GroundDistance = .6f;

	private LayerMask _groundLayer;
	private CharacterController _controller;
	private Vector3 _velocity;
	private float _gravity;
	private bool _isGrounded;
	private Transform _groundCheck;

	// Use this for initialization
	void Start () {
		_controller = GetComponent<CharacterController>();
		_gravity = Physics.gravity.y;
		_groundCheck = transform.GetChild(1);
		_groundLayer = 1<<LayerMask.NameToLayer("Ground");
		Debug.Log(_groundLayer);
		Debug.Log (_groundCheck);
	}
	
	// Update is called once per frame
	void Update() {
		_isGrounded = Physics.CheckSphere(_groundCheck.position, GroundDistance, _groundLayer, QueryTriggerInteraction.Ignore);

		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * PlayerMoveSpeed;
		_velocity.y += _gravity * Time.deltaTime;

		if (Input.GetKey(KeyCode.Space) && _isGrounded) {
			_velocity.y += Mathf.Sqrt (JumpHeight * -2f * _gravity);
		}

		if (_controller.isGrounded && _velocity.y < 0) {
			_velocity.y = 0f;
		}
		move.y = _velocity.y;
		_controller.Move(move * Time.deltaTime);

	}
}