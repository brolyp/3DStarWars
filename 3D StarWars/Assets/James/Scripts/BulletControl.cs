using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour {
	public float BulletSpeed = 12.0f;
	public float ttl = 2.0f;
	int _damage = 10;
	private Vector3 _velocity;
	public AudioSource AudioSource;

	LayerMask damageLayer;
	// Use this for initialization
	void Start () {
		_velocity = Vector3.forward * BulletSpeed;
		damageLayer = LayerMask.NameToLayer ("Damageable");
	}

	void Awake()
	{
		Destroy (this.gameObject, ttl);
		AudioSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		AudioSource.pitch += .1f;
		Debug.DrawRay(transform.position + transform.forward, transform.forward, Color.red);
		transform.Translate( _velocity * Time.deltaTime);
	}

	void
	OnTriggerEnter(Collider other)
	{
		//Debug.Log ("Collsison w/: "+other.gameObject.name);
		if (other.gameObject.layer == damageLayer) {
			//Debug.Log ("Something Hit w/ Bullet:" + other.gameObject.name);
			ICanBlock oBlocker = other.GetComponent<ICanBlock>();
			IDamageable eControl = other.GetComponent<IDamageable> ();
			if (oBlocker != null) 
			{
				oBlocker.Block (transform, _damage);
			}
			else if (eControl != null) 
			{
				eControl.Damage (_damage);
				Destroy (this.gameObject);
			} 
			else 
			{
				Destroy (this.gameObject);
			}
		}
	}
}
