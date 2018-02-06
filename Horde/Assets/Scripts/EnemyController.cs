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
	public float attackRadius = 0.4f;
	public LayerMask buildingLayer;
	public float attackCooldown = 2f;
	public EnemyType type = EnemyType.Killer;
	
	private EntityController entityController;
	private NavMeshAgent agent;
	private GameObject target;
	private float targetDistance;
	private Bounds bounds;
	private bool attacking = false;
	private bool atRange = false;
	private float nextAttack = 0;
	private float spawnTime;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		SetTarget(buildingToAttack);
		MoveToPoint(target.transform.position);
		EnemyController.enemies.Add(transform);
		spawnTime = Time.time;
		entityController = GetComponent<EntityController>();
		bounds = GetComponent<Collider>().bounds;
	}
	
	// Update is called once per frame
	void Update () {

		float attackCount = EnemyController.playerAttackCount;

		if (Time.time > spawnTime + 0.5f) {
			UpdateTarget();
			targetDistance = TargetDistance();
		}

		Attack();
	}

	float TargetDistance() {
		RaycastHit hit;
		Physics.Raycast(transform.position, GetTargetDirection(), out hit);
		return hit.distance;
	}

	void UpdateTarget() {
		Debug.Log(target);
		if (target == null) SetTarget(buildingToAttack);
		float playerDistance = Vector3.Distance(player.transform.position, transform.position);

		bool minCnt;

		if (type == EnemyType.Destructer) {
			minCnt = (atRange || EnemyController.playerAttackCount <= 2);
		} else {
			minCnt = (atRange || EnemyController.playerAttackCount <= 5);
		}
	
		if (playerDistance <= lookRadius && minCnt) {
			if (target != player) {
				EnemyController.playerAttackCount++;
			}
			SetTarget(player);
		} else if (target == player) {
			SetTarget(buildingToAttack);
			EnemyController.playerAttackCount--;
		}

		if (targetDistance <= attackRadius) {
			atRange = true;
		} else {
			atRange = false;
		}
		
		if (agent.velocity.magnitude < 0.2f) {
			if (target == buildingToAttack) {
				RaycastHit hit;
				Physics.Raycast(transform.position, GetTargetDirection(), out hit);
				if (hit.transform.name != "BuildingToDefend") SetTarget(hit.transform.gameObject);
			}
			FaceTarget();
		}
	}

	void Attack() {
		if (atRange && Time.time > nextAttack) {
			entityController.Attack();
			nextAttack = Time.time + attackCooldown;
		}
	}

	void FaceTarget() {
		Vector3 direction = (target.transform.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

	void MoveToPoint (Vector3 point) {
		agent.SetDestination(point);
	}

	void SetTarget(GameObject newTarget) {
		target = newTarget;
		MoveToPoint(newTarget.transform.position);

		if (target == buildingToAttack) {
			agent.stoppingDistance = 0.9f;
		} else {
			agent.stoppingDistance = 0.4f;
		}
	}

	Vector3 GetTargetDirection() {
		return (target.transform.position - transform.position);
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRadius);
	}

	public void SetType(EnemyType _type) {
		type = _type;
	}
}
