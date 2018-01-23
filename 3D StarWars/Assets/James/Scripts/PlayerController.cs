using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, IKillable {
	private int LIGHT_SABER = 3, GROUND_CHECK = 2, CAMERA = 0;
	private enum SaberState {Idle, Blocking, Shooting};

	public float PlayerRotSpeed = 500.0f;
	public float PlayerMoveSpeed = 10.0f;
	public float JumpHeight = 20.0f;
	public float GroundDistance = .6f;
	public float SaberSpeed = 5.0f;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	private CharacterController _controller;
	private Vector3 _cameraOffset;
	private LayerMask _groundLayer;
	private Vector3 _velocity;
	private float _gravity;
	private bool _isGrounded;
	private Transform _camera;
	private Transform _groundCheck;
	private Transform _saber;
	private SaberState _saberState;
	private Animator _saberAnimator;


	// Use this for initialization
	void Start () {
		_saberState = SaberState.Idle;
		_cameraOffset = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z + 8.0f);
		_controller = GetComponent<CharacterController>();
		_gravity = Physics.gravity.y;
		_groundCheck = transform.GetChild(GROUND_CHECK);
		_groundLayer = 1<<LayerMask.NameToLayer("Ground");
		_saber = transform.GetChild(LIGHT_SABER);
		_saberAnimator = _saber.GetComponent<Animator>();
		_camera = transform.GetChild (CAMERA);
		Debug.Log(_groundLayer);
		Debug.Log (_groundCheck);
	}
	
	// Update is called once per frame
	void Update() {
		//if(_saberState == SaberState.Shooting){
		//	_saberAnimator.Play ("ShootSaber");
		//	_saberState = SaberState.Idle;
		//}
		_isGrounded = Physics.CheckSphere(_groundCheck.position, GroundDistance, _groundLayer, QueryTriggerInteraction.Ignore);
		float deltaRotate = Input.GetAxis ("Mouse X") * PlayerRotSpeed;
		transform.Rotate (0, deltaRotate, 0);
		_cameraOffset = Quaternion.AngleAxis (Input.GetAxis ("Mouse X") * PlayerRotSpeed, Vector3.up) * _cameraOffset;
		_camera.position = transform.position + _cameraOffset; 
		_camera.LookAt(transform.position);
		//var forward = _camera.forward;
		//forward.y = 0;
		//forward.Normalize ();
		//transform.Rotate(0,Input.GetAxis ("Mouse X") * PlayerRotSpeed,0);
			
		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * PlayerMoveSpeed;

		//move = move + transform.forward;
		move = -move;
		_velocity.y += _gravity * Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) {
			_velocity.y += Mathf.Sqrt (JumpHeight * -2f * _gravity);
		}

		if (_isGrounded && _velocity.y < 0) {
			_velocity.y = 0f;
		}
		move.y = _velocity.y;
		//_controller.Move (move * Time.deltaTime);
		transform.Translate(move * Time.deltaTime);


		if(Input.GetMouseButtonDown(0) ){
			//_saberState = SaberState.Shooting;	
			_saberAnimator.Play ("ShootSaber");
			StartCoroutine(Shoot ());
		}
	}

	public void Kill(){
	}

	public void Damage(float damage){
	}

	private IEnumerator Shoot(){
		yield return new WaitForSeconds(0.17f);
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 12;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
		
}