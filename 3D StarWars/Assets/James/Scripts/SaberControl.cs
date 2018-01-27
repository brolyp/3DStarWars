using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberControl : MonoBehaviour, IDamageable, IHealable {
	public int MaxEnergy = 100;

	private int MESH = 0;
	private int _energy;
	private PlayerController _parent;
	private Transform _mesh;
	private Vector3 _initialPosition;
	private Vector3 _initialScale;

	// Use this for initialization
	void Start () {
		_mesh = transform.GetChild (MESH);
		_energy = MaxEnergy;		
		_parent = transform.parent.gameObject.GetComponent<PlayerController>();
		_initialPosition = _mesh.localPosition;
		_initialScale = _mesh.localScale;

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Damage(int damage){
		_energy -= damage;
		//Debug.Log ("Energy:" + _energy);
		if (_energy < 0) {
			_parent.Kill ();
		} else {
			float scale = _energy/100f;
			float pos = scale/2 + 1 -_initialPosition.y;
			AdjustSaberLength(scale, pos);
		}
	}

	public void Heal(int heal){
		_energy += heal;
		if (_energy > MaxEnergy) {
			_energy = MaxEnergy;
		}
		//Debug.Log ("Energy:" + _energy);
		float scale = _energy/100f;
		float pos = scale/2 + 1 - _initialPosition.y;
		AdjustSaberLength(scale, pos);
	}

	private void AdjustSaberLength(float scale, float pos){
		//_mesh.transform.localScale = new Vector3(_initialScale.x,_initialScale.y,_initialScale.z);
		_mesh.transform.localPosition = new Vector3 (_initialPosition.x, pos, _initialPosition.z);
		_mesh.transform.localScale = new Vector3(_initialScale.x, scale, _initialScale.z);
	}
}
	