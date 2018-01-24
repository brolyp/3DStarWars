using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour {
	public float BulletSpeed = 12.0f;
	private Vector3 _velocity;
	// Use this for initialization
	void Start () {
		_velocity = BulletSpeed * transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(transform.position + transform.forward, transform.forward, Color.red);
		transform.Translate( _velocity * Time.deltaTime);
	}
}
