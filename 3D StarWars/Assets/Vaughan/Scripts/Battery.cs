using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

    public int HealAmount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void
    OnTriggerEnter(Collider other)
    {
        if (HealAmount > 0)
        {
            IHealable isHealable = other.GetComponent<IHealable>();
            if (isHealable != null)
                isHealable.Heal(HealAmount);
        }
        else if (HealAmount < 0)
        {
            IDamageable isDamagable = other.GetComponent<IDamageable>();
            if (isDamagable != null)
                isDamagable.Damage(HealAmount);
        }
    }
}
