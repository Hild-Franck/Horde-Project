using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	public enum EnemyType { Destructer, Killer };
	
	public static int playerAttackCount = 0;
	public static int enemyCount;

	public GameObject buildingToAttack;
	public GameObject player;
	public float lookRadius = 10f;
	public float attackRadius = 0.4f;
	public LayerMask buildingLayer;
	public float attackCooldown = 2f;
	public EnemyType type = EnemyType.Killer;
	public bool targetBuildings;
	public Image attackBar;
	public int goldWorth;
	
	private EntityController entityController;
	private NavMeshAgent agent;
	private Transform target;
	private float targetDistance;
	private Bounds bounds;
	private bool attacking = false;
	private bool atRange = false;
	private float nextAttack = 0;
	private float spawnTime;
	private float attackTime;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		SetTarget(buildingToAttack.transform);
		MoveToPoint(target.position);
		spawnTime = Time.time;
		entityController = GetComponent<EntityController>();
		bounds = GetComponent<Collider>().bounds;
		enemyCount++;
		float modifier = SpawnController.instance.waveCount - 1;
		entityController.startHealth += modifier;
		entityController.health += modifier;
		entityController.damage += modifier;
	}
	
	// Update is called once per frame
	void Update () {

		float attackCount = EnemyController.playerAttackCount;

		if (entityController.swordAnimation.GetBool("Charging") && attackTime == 0f) {
			attackTime = Time.time;
		}

		if (entityController.swordAnimation.GetBool("Charging")) {
			attackBar.fillAmount = (Time.time - attackTime) / 1f;
		} else {
			attackBar.fillAmount = 0f;
			attackTime = 0f;
		}

		if (Time.time > spawnTime + 0.5f) {
			UpdateTarget();
			targetDistance = TargetDistance();
		}

		Attack();
	}

	public void Init(GameObject _buildingToAttack, GameObject _player) {
		buildingToAttack = _buildingToAttack;
		player = _player;
	}

	float TargetDistance() {
		RaycastHit hit;
		Physics.Raycast(transform.position, GetTargetDirection(), out hit);
		return hit.distance;
	}

	Transform GetNearestBuilding(List<Transform> buildings) {
		Transform nearestBuilding = null;
		Vector3 position = transform.position;
		float minDistance = lookRadius + 2f;


		buildings.ForEach(delegate(Transform building){
			float distance = Vector3.Distance(position, building.position);
			if (distance < minDistance) {
				nearestBuilding = building;
				minDistance = distance;
			}
		});

		return nearestBuilding;
	}

	void UpdateTarget() {
		if (target == null) SetTarget(buildingToAttack.transform);


		float playerDistance = Vector3.Distance(player.transform.position, transform.position);

		bool minCnt;

		if (type == EnemyType.Destructer) {
			minCnt = (target == player.transform || EnemyController.playerAttackCount < 2);
		} else {
			minCnt = (target == player.transform || EnemyController.playerAttackCount < 5);
		}

	
		if (playerDistance <= lookRadius && minCnt) {
			if (target != player.transform) {
				EnemyController.playerAttackCount++;
			}
			SetTarget(player.transform);
		} else if (target == player.transform) {
			SetTarget(buildingToAttack.transform);
			EnemyController.playerAttackCount--;
		}
		
		if (targetBuildings && target != player.transform) {
			Transform nearestBuilding = GetNearestBuilding(BuildingController.instance.GetBuildings());
			if (nearestBuilding != null) SetTarget(nearestBuilding);
		}

		if (targetDistance <= attackRadius) {
			atRange = true;
		} else {
			atRange = false;
		}
		
		if (agent.velocity.magnitude < 0.2f) {
			if (target == buildingToAttack.transform) {
				RaycastHit hit;
				Physics.Raycast(transform.position, GetTargetDirection(), out hit);
				if (hit.transform != null && hit.transform.name != "BuildingToDefend") SetTarget(hit.transform);
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
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 30f);
	}

	void MoveToPoint (Vector3 point) {
		agent.SetDestination(point);
	}

	void SetTarget(Transform newTarget) {
		target = newTarget;
		MoveToPoint(newTarget.transform.position);
	}

	Vector3 GetTargetDirection() {
		return (target.position - transform.position);
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

	void OnDestroy() {
		BuildingController.instance.gold += goldWorth;
		enemyCount--;
	}
}
