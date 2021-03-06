﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldControl : MonoBehaviour, IUseable {
	public float ShieldTime = 10f;
	public AudioSource Audio; 

	private float _jokeTime = 2.5f;
	private int MESH = 1, SHIELD = 0;
	private bool _active;
	private bool _held;
	private Transform _holder;
	private Transform _shield;
	private Transform _mesh;
	// Use this for initialization
	void Start () {
		_shield = transform.GetChild (SHIELD);
		_shield.localScale = new Vector3(0f,0f,0f);
		_mesh = transform.GetChild (MESH);
		_active = false;
		_held = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (_active) {
			_shield.Rotate (new Vector3 (360f, 360f, 360f) * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Debug.Log ("Player Grabs Shield");
			_held = true;
			_holder = other.transform;
			_holder.gameObject.GetComponent<ICanEquip>().Equip(transform);
			transform.SetParent (_holder);
			transform.localPosition = new Vector3 (0f, .25f, -1f);
			transform.rotation = _holder.rotation;
		}
	}

	public void Use(){
		Debug.Log ("SHIELDING");
		Audio.Play();
		IInvincible i = _holder.gameObject.GetComponent<IInvincible>();
		StartCoroutine(Joke (i));
	}

	private IEnumerator Joke(IInvincible i){
		Debug.Log ("Joking");
		yield return new WaitForSeconds (_jokeTime);
		i.Invincible (ShieldTime);
		StartCoroutine(InvincibleCountdown ());
	}

	private IEnumerator InvincibleCountdown(){
		_active = true;
		_mesh.localScale = new Vector3(0f,0f,0f);
		_shield.localScale = new Vector3(4f,4f,4f);
		_shield.position = _holder.position;
		Debug.Log ("PARTY");
		yield return new WaitForSeconds (ShieldTime);
		_active = false;
		_held = false;
		Debug.Log ("No More PARTY");
		Destroy (gameObject);
	}
		
}
