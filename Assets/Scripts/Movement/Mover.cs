﻿using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Movement {
	public class Mover : MonoBehaviour {
   
		[SerializeField] Transform target;

		NavMeshAgent navMeshAgent;

		private void Start(){
			navMeshAgent = GetComponent<NavMeshAgent>();
		}

    void Update(){    
			UpdateAnimator();
		}

		public void StartMoveAction(Vector3 destination){
			GetComponent<Fighter>().Cancel();
      MoveTo(destination);
		}

		public void MoveTo(Vector3 destination){
			navMeshAgent.destination = destination;
      navMeshAgent.isStopped = false;
		}
		

		public void Stop(){
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
