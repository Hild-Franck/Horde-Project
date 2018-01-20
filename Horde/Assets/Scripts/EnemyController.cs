using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	public enum EnemyType { Destructer, Killer };
	public static List<Transform> enemies = new List<Transform>();

	public static int playerAttackCount = 0;

	public GameObject buildingToAttack;
	public GameObject player;
	public float lookRadius = 10f;
	public float destructerLookRadius = 1.5f;
	public LayerMask buildingLayer;
	public float attackCooldown = 2f;
	public EnemyType type = EnemyType.Killer;
	
	private EntityController entityController;
	private NavMeshAgent agent;
	private GameObject target;
	private bool attacking = false;
	private bool atRange = false;
	private float nextAttack = 0;
	private float currentLookRadius;
	private float spawnTime;

	// Use this for initialization
	void Start () {
		target = buildingToAttack;
		agent = GetComponent<NavMeshAgent> ();
		MoveToPoint(target.transform.position);
		EnemyController.enemies.Add(transform);
		currentLookRadius = lookRadius;
		spawnTime = Time.time;
		entityController = GetComponent<EntityController>();

	}
	
	// Update is called once per frame
	void Update () {

		float attackCount = EnemyController.playerAttackCount;

		currentLookRadius = (type == EnemyType.Killer || attackCount == 0 || (attackCount == 1 && attacking))
			? lookRadius
			: destructerLookRadius;

		if (Time.time > spawnTime + 0.5f) {
			UpdateTarget();
		}

		if (attacking) {
			Attack();
		}

	}

	void UpdateTarget() {
		float playerDistance = Vector3.Distance(player.transform.position, transform.position);

		if (playerDistance <= currentLookRadius) {
			if (target != player) {
				attacking = true;
				EnemyController.playerAttackCount++;
			}
			SetTarget(player);
		} else if (target == player) {
			attacking = false;
			SetTarget(buildingToAttack);
			EnemyController.playerAttackCount--;
		}
		
		if (agent.velocity.magnitude < 0.2f) {
			if (target == buildingToAttack && !attacking) {
				RaycastHit hit;
				Physics.Raycast(transform.position, GetTargetDirection(), out hit, Mathf.Infinity, buildingLayer);
				if (hit.transform != null && "Building" == hit.transform.gameObject.tag) {
					attacking = true;
					target = hit.transform.gameObject;
				}
			}
		}
	}

	void Attack() {
		// Debug.Log(atRange);
		if (atRange && Time.time > nextAttack) {
			EntityController otherEntityController = target.GetComponent<EntityController>();
			entityController.Attack();
			if (otherEntityController.TakeDamage(1) == 0) {
				attacking = false;
				SetTarget(buildingToAttack);
			}
			nextAttack = Time.time + attackCooldown;
		}
	}

	void MoveToPoint (Vector3 point) {
		agent.SetDestination(point);
	}

	void SetTarget(GameObject newTarget) {
		target = newTarget;
		MoveToPoint(newTarget.transform.position);
	}

	Vector3 GetTargetDirection() {
		return (target.transform.position - transform.position);
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, currentLookRadius);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 0.4f);
	}

	public void SetType(EnemyType _type) {
		type = _type;
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject == target) {
			atRange = true;
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject == target) {
			atRange = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject == target) {
			atRange = false;
		}
	}

	void OnDestroy() {
		if (target == player) {
			EnemyController.playerAttackCount--;
		}
	}
}
