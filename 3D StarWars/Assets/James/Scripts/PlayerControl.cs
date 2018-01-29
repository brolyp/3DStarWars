using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour, IDamageable, IKillable, IHealable {
	private int MESH = 1, LIGHT_SABER = 2, BULLET_SPAWN = 3, SHIELD = 5;
	private enum SaberState {Idle, Blocking, Shooting};

	public float GravityMod = 1.2f;
	public float PlayerRotSpeed = 500.0f;
	public float PlayerMoveSpeed = 10.0f;
	public float JumpHeight = 20.0f;
	public float GroundDistance = .6f;
	public float SaberSpeed = 5.0f;
	public GameObject BulletPrefab;
	public Transform BulletSpawn;
	public Transform AimPoint;

	private bool _invincible;
	private bool _crouch;
	private Transform _mesh;
	private Transform _shield;
	private Vector3 _meshDefaultScale;
	private Vector3 _cameraOffset;
	private LayerMask _groundLayer;
	private Vector3 _velocity;
	private float _gravity;
	private bool _isGrounded;
	private Transform _groundCheck;
	private Transform _saber;
	private SaberControl _saberControl;
	private Animator _saberAnimator;
	private CharacterController _controller;
	//private Transform _bulletSpawn;


	// Use this for initialization
	void Start () {

		_invincible = false;
		_mesh = transform.GetChild (MESH);
		_shield = transform.GetChild (SHIELD);
		_shield.localScale = new Vector3(0f,0f,0f);
		_meshDefaultScale = _mesh.localScale;
		_crouch = false;
		_gravity = Physics.gravity.y * GravityMod;
		_controller = GetComponent<CharacterController>();
		_groundLayer = 1<<LayerMask.NameToLayer("Ground");
		_saber = transform.GetChild(LIGHT_SABER);
		_saberControl = _saber.gameObject.GetComponent<SaberControl>();
		_saberAnimator = _saber.GetComponent<Animator>();
		//_bulletSpawn = transform.GetChild (BULLET_SPAWN);
		//Debug.Log(_groundLayer);
		//Debug.Log (_groundCheck);
	}
	
	// Update is called once per frame
	void Update() {
		_shield.Rotate (new Vector3 (360f,360f,360f) * Time.deltaTime);
		_controller.height = 1f;
		_isGrounded = Physics.CheckSphere(transform.position, GroundDistance, _groundLayer, QueryTriggerInteraction.Ignore);
		_mesh.localScale = new Vector3 (_meshDefaultScale.x, _meshDefaultScale.y, _meshDefaultScale.z);
		_mesh.localPosition = new Vector3 (0, 0, 0);
		if (_crouch) {
			if (!Input.GetKey (KeyCode.C)) {
				_crouch = false;
				_saberAnimator.CrossFade ("Idle", .1f);
			} else {
				_controller.height = .5f;
				_mesh.localPosition = new Vector3 (0, -.25f, 0);
				_mesh.localScale = new Vector3 (_meshDefaultScale.x, _meshDefaultScale.y * .5f, _meshDefaultScale.z);
			}

		} 
			

		float deltaRotate = Input.GetAxis ("Mouse X") * PlayerRotSpeed;
		transform.Rotate (0, deltaRotate, 0);

		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * PlayerMoveSpeed;
		move = transform.rotation * move;

		if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) {
			_velocity.y += Mathf.Sqrt (JumpHeight * -2f * _gravity);
		}
		_velocity.y += _gravity * Time.deltaTime;
		if (_isGrounded && _velocity.y < 0) {
			_velocity.y = 0f;
		}
		move.y = _velocity.y;

		_controller.Move(move * Time.deltaTime);

		if(Input.GetMouseButtonDown(0) ){
			StartCoroutine(Shoot ());
		}

		if(Input.GetKey(KeyCode.Q) ){
			StartCoroutine(Invincible());
		}

		if(Input.GetKey(KeyCode.C)){
			if(!_crouch){
				_crouch = true;
				_saberAnimator.Play ("toCrouch");
			}
		}
			
		if (Input.GetKeyDown(KeyCode.P)){
			Damage(1);
		}
		if (Input.GetKeyDown(KeyCode.O)){
			Heal(1);
		}
	}

	private IEnumerator Invincible(){
		_shield.localScale = new Vector3(3.1f,3.1f,3.1f);
		_invincible = true;
		Debug.Log ("PARTY");
		yield return new WaitForSeconds (10f);
		_invincible = false;
		Debug.Log ("No More PARTY");
		_shield.localScale = new Vector3(0f,0f,0f);
	}

	public void Kill(){
		Debug.Log ("Player has been Killed!");
		Destroy (this.gameObject, 0.0f);
	}

	public void Damage(int damage){
		if (!_invincible) {
			_saberControl.Damage (damage);
		}
	}

	public void Heal(int heal){
		_saberControl.Heal (heal);
	}

	private IEnumerator Shoot(){
		_saberAnimator.Play ("ShootSaber");
		yield return new WaitForSeconds(0.17f);
 		//BulletSpawn.transform.LookAt(AimPoint.transform);
		var bullet = (GameObject)Instantiate (
			BulletPrefab,
			BulletSpawn.position,
			BulletSpawn.rotation);

		Destroy(bullet, 2.0f);
	}
		
}