using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, IKillable, IHealable {
	private int LIGHT_SABER = 3, GROUND_CHECK = 2, CAMERA = 0, BULLET_SPAWN = 4;
	private enum SaberState {Idle, Blocking, Shooting};

	public float PlayerRotSpeed = 500.0f;
	public float PlayerMoveSpeed = 10.0f;
	public float JumpHeight = 20.0f;
	public float GroundDistance = .6f;
	public float SaberSpeed = 5.0f;
	public GameObject BulletPrefab;
	public Transform BulletSpawn;
	public Transform AimPoint;

	private Vector3 _cameraOffset;
	private LayerMask _groundLayer;
	private Vector3 _velocity;
	private float _gravity;
	private bool _isGrounded;
	private Transform _camera;
	private Transform _groundCheck;
	private Transform _saber;
	private SaberControl _saberControl;
	private Animator _saberAnimator;
	private CharacterController _controller;
	//private Transform _bulletSpawn;


	// Use this for initialization
	void Start () {
		//TODO put this and scrollwheel change in camera
		_cameraOffset = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z + 8.0f);
		_gravity = Physics.gravity.y;
		_controller = GetComponent<CharacterController>();
		_groundCheck = transform.GetChild(GROUND_CHECK);
		_groundLayer = 1<<LayerMask.NameToLayer("Ground");
		_saber = transform.GetChild(LIGHT_SABER);
		_saberControl = _saber.gameObject.GetComponent<SaberControl>();
		_saberAnimator = _saber.GetComponent<Animator>();
		_camera = transform.GetChild (CAMERA);
		//_bulletSpawn = transform.GetChild (BULLET_SPAWN);
		Debug.Log(_groundLayer);
		Debug.Log (_groundCheck);
	}
	
	// Update is called once per frame
	void Update() {

		if (Input.GetKeyDown(KeyCode.P)){
			Damage(10);
		}
		if (Input.GetKeyDown(KeyCode.O)){
			Heal(10);
		}

		_isGrounded = Physics.CheckSphere(_groundCheck.position, GroundDistance, _groundLayer, QueryTriggerInteraction.Ignore);
		float deltaRotate = Input.GetAxis ("Mouse X") * PlayerRotSpeed;
		transform.Rotate (0, deltaRotate, 0);
		_cameraOffset = Quaternion.AngleAxis (Input.GetAxis ("Mouse X") * PlayerRotSpeed, Vector3.up) * _cameraOffset;
		//_camera.position = transform.position + _cameraOffset; 
		//_camera.LookAt(transform.position);
			
		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * PlayerMoveSpeed;
		move = transform.rotation * move;

		_velocity.y += _gravity * Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) {
			_velocity.y += Mathf.Sqrt (JumpHeight * -2f * _gravity);
		}

		if (_isGrounded && _velocity.y < 0) {
			_velocity.y = 0f;
		}
		move.y = _velocity.y;
		//transform.Translate(move * Time.deltaTime);
		_controller.Move(move * Time.deltaTime);

		//Debug.DrawRay(transform.position + transform.forward, transform.forward, Color.green);
		//Debug.DrawRay (BulletSpawn.transform.position + BulletSpawn.transform.forward, BulletSpawn.transform.forward, Color.green);
		Transform aim = BulletSpawn.transform;
		//aim.LookAt (AimPoint.transform);
		Debug.DrawRay(aim.position + aim.forward, aim.forward, Color.red);
		if(Input.GetMouseButtonDown(0) ){
			_saberAnimator.Play ("ShootSaber");
			StartCoroutine(Shoot ());
		}
	}

	public void Kill(){
		Debug.Log ("Player has been Killed!");
		Destroy (this.gameObject, 0.0f);
	}

	public void Damage(int damage){
		_saberControl.Damage (damage);
	}

	public void Heal(int heal){
		_saberControl.Heal (heal);
	}

	private IEnumerator Shoot(){
		yield return new WaitForSeconds(0.17f);
 		//BulletSpawn.transform.LookAt(AimPoint.transform);
		var bullet = (GameObject)Instantiate (
			BulletPrefab,
			BulletSpawn.position,
			BulletSpawn.rotation);

		Destroy(bullet, 2.0f);
	}
		
}