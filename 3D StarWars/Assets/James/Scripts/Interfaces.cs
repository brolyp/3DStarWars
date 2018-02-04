using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

public interface IEnemyNotification
{
	void Alert (Transform aTrans);
}
public interface ICanBlock
{
	void Block (Transform bTrans, int damage);
}
public interface ICanEquip
{
	void Equip (Transform item);
}