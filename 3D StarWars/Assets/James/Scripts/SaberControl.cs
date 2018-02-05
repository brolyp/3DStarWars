using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberControl : MonoBehaviour, IDamageable, IHealable {
	public int MaxEnergy = 100;
	public bool Overchargeable;

	private int MESH = 0, OVERCHARGEMESH = 2;
	private int _overcharge;
	private int _energy;
	private IKillable _parent;
	public Transform _mesh;
	private Transform _oMesh;


	// Use this for initialization
	void Start () {
		_mesh = transform.GetChild (MESH);
		if(Overchargeable)_oMesh = transform.GetChild (OVERCHARGEMESH);
		_energy = MaxEnergy;		
		_parent = transform.parent.gameObject.GetComponent<IKillable>();
		_overcharge = 0;
		Heal (10);
		Damage (10);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Damage(int damage){
		if (_overcharge == 0) {
			_energy -= damage;
			Debug.Log ("Energy: " + _energy);
			//Debug.Log ("Energy:" + _energy);
			if (_energy < 0) {
				try {
					_parent.Kill ();
				} catch {
					Debug.Log ("on Luke Death exception caught");
				}
			} else {
				float scale = _energy / 100f;
				float pos = scale / 2 + .45f;
				AdjustSaberLength (scale, pos, _mesh);
			}
		} else {
			_overcharge -= damage;
			//Debug.Log ("Energy:" + _energy);
			if (_overcharge < 0) {
				_overcharge = 0;
			} else {
				float scale = _overcharge / 100f;
				float pos = scale / 2 + .45f;
				AdjustSaberLength (scale, pos, _oMesh);
			}
		}
	}

	public void Heal(int heal){
		if (_energy == 100) {
			if (!Overchargeable) {
				return;
			}
			_overcharge += heal;
			if (_overcharge > MaxEnergy) {
				_overcharge = MaxEnergy;
			}
			//Debug.Log ("Energy:" + _energy);
			float scale = _overcharge / 100f;
			float pos = scale / 2 + .45f;
			if (_overcharge == 100f) {
				scale += .1f;
			}
			AdjustSaberLength (scale, pos, _oMesh);
		} else {
			_energy += heal;
			if (_energy > MaxEnergy) {
				_energy = MaxEnergy;
			}
			//Debug.Log ("Energy:" + _energy);
			float scale = _energy / 100f;
			float pos = scale / 2 + .45f;
			AdjustSaberLength (scale, pos, _mesh);
		}
	}

	public void Block(Transform bTrans)
	{
		bTrans.forward = Vector3.up;
	}

	private void AdjustSaberLength(float scale, float pos, Transform mesh){
		mesh.transform.localPosition = new Vector3 (mesh.localPosition.x, pos, mesh.localPosition.z);
		mesh.transform.localScale = new Vector3(mesh.localScale.x, scale, mesh.localScale.z);
	}

}
	