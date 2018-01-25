using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable {
	void Kill();
}

public interface IDamageable {
	void Damage(int damage);
}