using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control {

  public class AIController : MonoBehaviour {

    [SerializeField] float chaseDistance = 5f;
	[SerializeField] float suspicionTime = 3f;
	[SerializeField] PatrolPath patrolPath;	
	[SerializeField] float waypointTolerance = 1f;

    Fighter fighter;
    Health health;
    Mover mover;
    GameObject player;

    Vector3 guardPosition;
    float timeSinceLastSawPlayer = Mathf.Infinity;
	int currentWaypointIndex = 0;


    private void Start() {
      mover = GetComponent<Mover>();
      fighter = GetComponent<Fighter>();
      player = GameObject.FindWithTag("Player");
      health = GetComponent<Health>();

      guardPosition = transform.position;
    }

    private void Update() {

		if(health.IsDead()) return;

		if(InAttackRangeOfPlayer() && fighter.CanAttack(player)) { 
			timeSinceLastSawPlayer = 0;
			AttackBehavior();
      	} else if (timeSinceLastSawPlayer < suspicionTime) { 
        	SuspicionBehavior();
      	} else {
        	PatrolBehavior();
      	}	

      	timeSinceLastSawPlayer += Time.deltaTime;
    }

    private void PatrolBehavior() {
		Vector3 nextPosition = guardPosition;
		if(patrolPath != null){
			if(AtWaypoint()){
				CycleWaypoint();
			}
			nextPosition = GetCurrentWaypoint();
		} 
		
		mover.StartMoveAction(nextPosition);
    }

    private bool AtWaypoint() {
      float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
	  return distanceToWaypoint < waypointTolerance; 
    }

    private void CycleWaypoint() {
		currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private Vector3 GetCurrentWaypoint() {
      return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void SuspicionBehavior() {
      GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    private void AttackBehavior() {
      fighter.Attack(player);
    }

    private bool InAttackRangeOfPlayer() {
      float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
      return distanceToPlayer < chaseDistance;
    }

    // Called by Unity
    private void OnDrawGizmosSelected() {

      Gizmos.color = Color.yellow;
      Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1, 0, 1));
      Gizmos.DrawWireSphere(Vector3.zero, chaseDistance);
 

    }


  }

}