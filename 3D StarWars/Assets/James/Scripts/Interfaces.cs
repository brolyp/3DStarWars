using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable {
	void Use();
}

public interface IKillable {
	void Kill();
}

public interface IDamageable {
	void Damage(int damage);
}

public interface IHealable {
	void Heal (int heal);
}

public interface IInvincible {
	IEnumerator Invincible (float time);
}