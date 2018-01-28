using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour {
	public float BulletSpeed = 12.0f;
	public Collider thing;
	int _damage = 10;
	private Vector3 _velocity;
	LayerMask damageLayer;
	// Use this for initialization
	void Start () {
		_velocity = Vector3.forward * BulletSpeed;
		damageLayer = LayerMask.NameToLayer ("Damageable");
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(transform.position + transform.forward, transform.forward, Color.red);
		transform.Translate( _velocity * Time.deltaTime);
	}

	void
	OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == damageLayer) {
			thing = other;
			IDamageable eControl = other.GetComponent<IDamageable> ();
			eControl.Damage (_damage);
			Destroy (this.gameObject);
		}
	}
}
