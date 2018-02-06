using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class vaderControl : MonoBehaviour, IDamageable, IKillable, IHealable, ICanBlock {
	public enum AISTATE { patrol, pursue, block, retreat, attack, jump };
	public AISTATE cAIState;

	public Transform[] patrolPoints;
	public Transform batLocNear;
	public Transform playerTransf;
	public Transform bulletTransf;

	Rigidbody rigbod;

	public vaderPerception vPerception;

	public int destPoint;
	public int deaths = 0;
	public int lives = 3;
	public int retreatMultiplier = 3;

	NavMeshAgent agent;

	public float aimFuzz;
	public float aimRangeBottom;
	public float aimRangeTop;
	public float saveTime;
	float runSpeed;

	public GameObject _player;
	public GameObject bulletPrefab;
	RaycastHit outHit;

	public bool playerInRange;
	public bool alertedToPlayer;
	public bool firedAt;
	private bool _needHeal;
	private bool _invincible;
	private bool _canBlock;

	private Transform _saber;
	private SaberControl _saberControl;
	private Animator _saberAnimator;


	// Use this for initialization
	void Start () {
		vPerception = GetComponentInChildren<vaderPerception>();
		agent = GetComponent<NavMeshAgent>();
		_saber = transform.GetChild(1);
		_saberControl = _saber.gameObject.GetComponent<SaberControl>();
		_saberAnimator = _saber.GetComponent<Animator>();
		_player = GameObject.FindWithTag ("Player");
		rigbod = GetComponent<Rigidbody> ();

		_canBlock = true;
		runSpeed = agent.speed;
		agent.autoBraking = false;
		destPoint = 0;
		aimFuzz = 15.0f;
		aimRangeTop = 1.5f;
		aimRangeBottom = -1.5f;
		saveTime = 0.0f;
		NextPoint();
	}
	
	// Update is called once per frame
	void Update () {
		switch (cAIState) {
		case AISTATE.patrol:
			//action
			if (!agent.pathPending && agent.remainingDistance < 0.5f) {
				NextPoint ();
			}

			//update
			if (_needHeal)
				cAIState = AISTATE.retreat;
			
			if (alertedToPlayer) {
				cAIState = AISTATE.pursue;
				agent.destination = playerTransf.position;
			}
			if (firedAt)
				cAIState = AISTATE.block;
			break;
		case AISTATE.pursue:
			//action
			if(playerTransf != null && !_needHeal)agent.destination = playerTransf.position;
			else 
			{
				cAIState = AISTATE.patrol;
				NextPoint ();
			}

			//update
			if (agent.remainingDistance < 1.0f)
				//cAIState = AISTATE.attack;
			if (firedAt)
				cAIState = AISTATE.block;
			break;
		case AISTATE.block:
			//action
			if (bulletTransf != null && Vector3.Distance (transform.position, bulletTransf.position) < 4 && _canBlock) {
				Block (bulletTransf, 10);
				firedAt = false;
			}
			//update
			if (_needHeal) {
				_canBlock = false;
				agent.speed = retreatMultiplier * runSpeed;
				agent.destination = batLocNear.position;
				cAIState = AISTATE.retreat;
			}
			else if (!firedAt && playerTransf != null) 
			{
				cAIState = AISTATE.pursue;
			} else if (!firedAt) 
			{
				if (Physics.Raycast (new Ray (transform.position, _player.transform.position - transform.position), out outHit)) {
					if (outHit.collider.tag == "Player") {
						if (Vector3.Dot (transform.forward, _player.transform.position - transform.position) > 0) 
						{
							cAIState = AISTATE.pursue;
							playerTransf = _player.transform;
						} 
						else 
						{
							cAIState = AISTATE.patrol;
						}
					} 
					else 
					{
						cAIState = AISTATE.patrol;
					}
				} 
				else 
				{
					cAIState = AISTATE.patrol;
				}
			}
			break;
		case AISTATE.retreat:
			//action
			//update
			break;
		case AISTATE.attack:
			//action
			IDamageable player = playerTransf.GetComponentInParent<IDamageable> ();
			player.Damage (102);
			//update
			break;
		}
		
	}

	void NextPoint()
	{
		if (patrolPoints.Length == 0) return;
		agent.destination = patrolPoints[destPoint].position;

		destPoint = (destPoint + 1) % patrolPoints.Length;
	}

	public void 
	Kill(){
		if (deaths < lives) {
			deaths++;

			agent.speed = retreatMultiplier * runSpeed;
			agent.autoBraking = true;
			agent.destination = batLocNear.position;
			_needHeal = true;
			_canBlock = false;

			cAIState = AISTATE.retreat;
		} else if (deaths == lives) {
			Debug.Log ("Vader has been Killed!");
			Destroy (this.gameObject, 0.0f);
		}

	}

	public void 
	Damage(int damage){
		if (!_invincible) {
			_saberControl.Damage (damage);
		}
	}

	public void Heal(int heal){
		_needHeal = false;
		_canBlock = true;
		_saberControl.Heal (heal);
		runSpeed *= 1.5f;
		agent.speed = runSpeed;
		agent.autoBraking = false;
		cAIState = AISTATE.pursue;
	}

	public void
	Block(Transform bullet, int damage)
	{
		if (_canBlock) {
			agent.isStopped = true;
			transform.LookAt (bullet);
		}
		_saberAnimator.Play ("Block");
			_saberControl.Block (bullet);
			Damage (10);
			agent.isStopped = false;

	}
}
