using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class vaderControl : MonoBehaviour, IDamageable, IKillable, IHealable, ICanBlock {
	public enum AISTATE { patrol, pursue, block, retreat, attack };
	public AISTATE cAIState;

	public Transform[] patrolPoints;
	public Transform bulletSpawnT;
	public Transform rifle;
	public Transform batLocNear;
	public Transform playerTransf;
	public Transform bulletTransf;


	public vaderPerception vPerception;

	public int destPoint;
	public int deaths = 0;
	public int lives = 3;

	NavMeshAgent agent;

	public float aimFuzz;
	public float aimRangeBottom;
	public float aimRangeTop;
	public float saveTime;
	public GameObject bulletPrefab;
	RaycastHit outHit;

	private bool _invincible;
	public bool playerInRange;
	public bool alertedToPlayer;
	public bool firedAt;

	private Transform _saber;
	private SaberControl _saberControl;
	private Animator _saberAnimator;


	// Use this for initialization
	void Start () {
		vPerception = GetComponentInChildren<vaderPerception>();
		agent = GetComponent<NavMeshAgent>();
		agent.autoBraking = false;
		destPoint = 0;
		aimFuzz = 15.0f;
		aimRangeTop = 1.5f;
		aimRangeBottom = -1.5f;
		saveTime = 0.0f;
		NextPoint();
		_saber = transform.GetChild(1);
		_saberControl = _saber.gameObject.GetComponent<SaberControl>();
		_saberAnimator = _saber.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		switch (cAIState) {
		case AISTATE.patrol:
			//action
			agent.stoppingDistance = 0.5f;
			if (!agent.pathPending && agent.remainingDistance < 0.5f) {
				NextPoint ();
			}

			//update
			if (alertedToPlayer) {
				cAIState = AISTATE.pursue;
				agent.destination = playerTransf.position;
			}
			if (firedAt)
				cAIState = AISTATE.block;
			break;
		case AISTATE.pursue:
			//action
			agent.destination = playerTransf.position;
			//update
			if (agent.remainingDistance < 1.0f)
				cAIState = AISTATE.attack;
			if (firedAt)
				cAIState = AISTATE.block;
			break;
		case AISTATE.block:
			//action
			agent.isStopped = true;
			if (bulletTransf != null && Vector3.Distance (transform.position, bulletTransf.position) < 4) {
				Block (bulletTransf, 10);
				firedAt = false;
			}
			//update
			if (!firedAt && playerTransf != null) {
				cAIState = AISTATE.pursue;
			} else if (!firedAt) {
				cAIState = AISTATE.patrol;
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
		/*if (ePT.playerInArea) {
			transform.LookAt (ePT.pTrans);
			//Vector3 target = ePT.pTrans.position + (transform.position.normalized * 5);
			agent.stoppingDistance = 5.0f;
			agent.autoBraking = true;
			agent.destination = ePT.pTrans.position;
			if ((Time.time - saveTime) > 2f) {
				float yRot = Random.Range(aimRangeBottom, aimRangeTop) * aimFuzz;
				Quaternion aimRot = Quaternion.Euler (0, yRot, 0);
				Vector3 target = aimRot * (ePT.pTrans.position - bulletSpawnT.position);
				Debug.DrawRay (bulletSpawnT.position, target, Color.red);
				if (Physics.Raycast (bulletSpawnT.position, target, out outHit, 7.0f)) {
					if (outHit.collider.gameObject.tag == "Player") {
						//Debug.Log ("Something Hit w/ RayCast:" + outHit.collider.gameObject.name);
					}
				}
				saveTime = Time.time;

				GameObject shotBullet = (GameObject)Instantiate (bulletPrefab, bulletSpawnT.position, Quaternion.identity);
				shotBullet.transform.forward = target;
				//Destroy (shotBullet, 2.0f);  - moved to Bullet.Awake for autonomy
			}
		} else if (!agent.pathPending && agent.remainingDistance < 0.5f) {
			//agent.stoppingDistance = 0.5f;
			NextPoint ();
		} else 
		{
			agent.autoBraking = false;
			agent.stoppingDistance = 0.5f;
		}*/
		
	}

	void NextPoint()
	{
		if (patrolPoints.Length == 0) return;
		agent.destination = patrolPoints[destPoint].position;

		destPoint = (destPoint + 1) % patrolPoints.Length;
	}

	public void 
	Kill(){
		if (deaths < lives && cAIState == AISTATE.pursue) {
			cAIState = AISTATE.retreat;
			deaths++;
			agent.destination = batLocNear.position;
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
		cAIState = AISTATE.pursue;
		_saberControl.Heal (heal);
	}

	public void
	Block(Transform bullet, int damage)
	{
		agent.isStopped = true;
		transform.LookAt (bullet);
		_saberControl.Block (bullet);
		Damage (10);
		agent.isStopped = false;
	}

	//need get nearest capsule - list of all negative capsules in the area - accompanying list of 1 for up and 0 for bad

	//movement option after healing
}
