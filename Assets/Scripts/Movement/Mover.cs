﻿using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement {

	public class Mover : MonoBehaviour, IAction {
   
		[SerializeField] Transform target;

		NavMeshAgent navMeshAgent;

		private void Start(){
			navMeshAgent = GetComponent<NavMeshAgent>();
		}

    void Update(){    
			UpdateAnimator();
		}

		public void StartMoveAction(Vector3 destination){
      GetComponent<ActionScheduler>().StartAction(this);
      MoveTo(destination);
		}

		public void MoveTo(Vector3 destination){
			navMeshAgent.destination = destination;
      navMeshAgent.isStopped = false;
		}
		

		public void Cancel(){
			navMeshAgent.isStopped = true;
		}



		private void UpdateAnimator(){
			Vector3 velocity = navMeshAgent.velocity;
			Vector3 localVelocity = transform.InverseTransformDirection(velocity); // InverseTransformDirection - "No matter where you are in the world, lets convert that into animator for e.g you're moving forward at 3units"
			float speed = localVelocity.z;
			GetComponent<Animator>().SetFloat("forwardSpeed", speed); // Animator - Parameters - forwardSpeed exactly!
		}

	}

}
