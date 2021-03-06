﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour, IDamageable, IKillable, IHealable, IInvincible, ICanEquip, ICanBlock {
	private int MESH = 1, LIGHT_SABER = 2, BULLET_SPAWN = 3, CAMERA = 0;
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

	private bool _running;
	private Transform _shield;
	private bool _invincible;
	private bool _crouch;
	private Transform _mesh;
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
		_meshDefaultScale = _mesh.localScale;
		_gravity = Physics.gravity.y * GravityMod;
		_controller = GetComponent<CharacterController>();
		_groundLayer = 1<<LayerMask.NameToLayer("Ground") | 1<<LayerMask.NameToLayer("Default") & ~(1<<LayerMask.NameToLayer("Player"));
		_saber = transform.GetChild(LIGHT_SABER);
		_saberControl = _saber.gameObject.GetComponent<SaberControl>();
		_saberAnimator = _saber.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update() {
		

		_isGrounded = Physics.CheckSphere(transform.position, GroundDistance, _groundLayer, QueryTriggerInteraction.Ignore);
		_mesh.localScale = new Vector3 (_meshDefaultScale.x, _meshDefaultScale.y, _meshDefaultScale.z);
		_mesh.localPosition = new Vector3 (0, 0, 0);

		if (_saberAnimator.GetBool("Crouched")) {
			if (!Input.GetKey (KeyCode.C)) {
				if (_shield) {
					_shield.localPosition = new Vector3 (0, 0, -.5f);
				}
				_controller.height = 1f;
				_controller.radius = .5f;
				_controller.center = new Vector3 (0,0,0);
				_saberAnimator.SetBool ("Crouched", false);
				_saberAnimator.CrossFade ("Idle", .1f);
			} else {
				if (_shield) {
					_shield.localPosition = new Vector3 (0, -.125f, -.5f);
				}
				_controller.height = .5f;
				_controller.radius = .01f;
				_controller.center = new Vector3 (0,-.25f,0);
				_mesh.localPosition = new Vector3 (0, -.25f, 0);
				_mesh.localScale = new Vector3 (_meshDefaultScale.x, _meshDefaultScale.y * .5f, _meshDefaultScale.z);
			}

		} 
			

		float deltaRotate = Input.GetAxis ("Mouse X") * PlayerRotSpeed;
		transform.Rotate (0, deltaRotate, 0);

		float moveSpeed = PlayerMoveSpeed;
		if (_isGrounded && Input.GetKey (KeyCode.LeftShift)) {
			_running = true;
			moveSpeed = 2 * PlayerMoveSpeed;
		} else {
			_running = false;
		}
		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed;
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
			if (!_running && !_saberAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShootSaber")) {
				StartCoroutine (Shoot ());
			}
		}

		if(Input.GetKey(KeyCode.Q) ){
			Debug.Log ("attempting shield");
			if (_shield != null) {
				_shield.gameObject.GetComponent<IUseable>().Use ();
				Debug.Log ("using shield");
			}

		}

		if(Input.GetKey(KeyCode.C)){
			if(!_saberAnimator.GetBool("Crouched")){
				_saberAnimator.SetBool ("Crouched", true);
				_saberAnimator.Play ("toCrouch");
			}
		}
			
		if (Input.GetKeyDown(KeyCode.P)){
			Quaternion b = BulletSpawn.rotation;
			b = b * Quaternion.AngleAxis (180f,Vector3.up);
			var bullet = (GameObject)Instantiate (
				BulletPrefab,
				BulletSpawn.position + 5 * BulletSpawn.forward,
				b);
			
		}
		if (Input.GetKeyDown(KeyCode.O)){
			Heal(10);
		}
	}


	public 
	void Kill(){
		Debug.Log ("Player has been Killed!");
		Transform camera = _mesh = transform.GetChild (CAMERA);
		camera.GetChild(1).gameObject.GetComponent<PauseMenu> ().loseGame ();
		//Destroy (this.gameObject, 0.0f);
	}

	public 
	void Damage(int damage){
		if (!_invincible) {
			_saberControl.Damage (damage);
		}
	}

	public 
	void Heal(int heal){
		_saberControl.Heal (heal);
	}

	public void
	Block(Transform bTransf, int damage)
	{
		if (!_invincible) {
			if (!_saberAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Block")) {
				_saberAnimator.Play ("Block");
			}
			_saberControl.Block (bTransf);
			Damage (damage);
		}
		
	}

	public void Invincible(float time){
		StartCoroutine (InvincibleTime(time));
	}
	public IEnumerator InvincibleTime(float time){
		_invincible = true;
		yield return new WaitForSeconds (time);
		_invincible = false;
	}

	private IEnumerator Shoot(){
		
		_saberAnimator.Play ("ShootSaber");
		yield return new WaitForSeconds(0.17f);
 		//BulletSpawn.transform.LookAt(AimPoint.transform);
		var bullet = (GameObject)Instantiate (
			BulletPrefab,
			BulletSpawn.position,
			BulletSpawn.rotation);
		
		//Destroy(bullet, 2.0f); - moved to public float ttl in BulletControl for autonomy
	}

	public void Equip(Transform item){
		_shield = item;
	}
		
}