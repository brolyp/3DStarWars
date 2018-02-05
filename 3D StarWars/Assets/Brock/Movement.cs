using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public Rigidbody rb;

	void FixedUpdate () {
		rb.AddForce (0, 0, 1000 * Time.deltaTime);

		if (Input.GetKey ("d")) {
			FindObjectOfType<PauseMenu> ().loseGame ();
		}

		if (Input.GetKey ("a")) {
			FindObjectOfType<PauseMenu> ().winGame ();
		}

	}
}
