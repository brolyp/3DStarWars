using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberControl : MonoBehaviour, IDamageable {
	public int MaxEnergy;
	private int _energy;
	private PlayerController _parent;
	// Use this for initialization
	void Start () {
		_energy = MaxEnergy;		
		_parent = transform.parent.gameObject.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Damage(int damage){
		_energy -= damage;
		if (_energy < 1) {
			_parent.Kill ();
		} else {
			// HERE WE DO THINGS TO SABER LENGTH
		}
	}
}
