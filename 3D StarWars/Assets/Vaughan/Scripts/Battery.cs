using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

    public int HealAmount;
    public float respawnTimer = 0;
    private float timeLeft = 0;
    private AudioSource _audioSource;

	// Use this for initialization
	void Start () {
        _audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.deltaTime != 0)
        {
            if (GetComponent<Renderer>().enabled == false)
            {
                timeLeft -= Time.deltaTime;
                if(timeLeft <= 0)
                {
                    GetComponent<Renderer>().enabled = true;
                    GetComponent<Collider>().enabled = true;
                    timeLeft = respawnTimer;
                }
            }
        }
	}

    void
    OnTriggerEnter(Collider other)
    {
        if (HealAmount > 0)
        {
            IHealable isHealable = other.GetComponent<IHealable>();
            if (isHealable != null)
            {
                isHealable.Heal(HealAmount);
                timeLeft = respawnTimer;
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                _audioSource.Play();
            }
        }
        else if (HealAmount < 0)
        {
            IDamageable isDamagable = other.GetComponent<IDamageable>();
            if (isDamagable != null)
            {
                isDamagable.Damage(-HealAmount);
                timeLeft = respawnTimer;
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                _audioSource.Play();
            }
        }

    }
}
