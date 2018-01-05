using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	public GameObject buildingToAttack;
	public GameObject player;
	public float lookRadius = 10f;
	public LayerMask buildingLayer;
	public float attackCooldown = 10f;
	public float attackRange = 1f;

	private NavMeshAgent agent;
	private GameObject target;
	private bool attacking = false;
	private float nextAttack;

	// Use this for initialization
	void Start () {
		target = buildingToAttack;
		agent = GetComponent<NavMeshAgent> ();
		MoveToPoint(target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		float playerDistance = Vector3.Distance(player.transform.position, transform.position);

		if (playerDistance <= lookRadius) {
			target = player;
			MoveToPoint(target.transform.position);
		} else if (target == player) {
			target = buildingToAttack;
			MoveToPoint(target.transform.position);
		}

		if (agent.velocity == Vector3.zero) {
			if (target == buildingToAttack && !attacking) {
				RaycastHit hit;
				Physics.Raycast(transform.position, GetTargetDirection(), out hit, Mathf.Infinity, buildingLayer);
				if ("Building" == hit.transform.gameObject.tag) {
					Debug.Log(hit.transform.gameObject);
					attacking = true;
					target = hit.transform.gameObject;
				}
			}
		}

		if (attacking) {
			float targetDistance = Vector3.Distance(target.transform.position, transform.position);
			if (targetDistance < attackRange && Time.time > nextAttack) {
				Debug.Log(target);
				EntityController entityController = target.GetComponent<EntityController>();
				if (--entityController.health == 0) {
					target = buildingToAttack;
					MoveToPoint(target.transform.position);
				}
				nextAttack = Time.time + attackCooldown;
			}
		}

	}

	void MoveToPoint (Vector3 point) {
		agent.SetDestination(point);
	}

	Vector3 GetTargetDirection() {
		return (target.transform.position - transform.position);
	}

	void OnDrawGizmoSelected () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}
}
