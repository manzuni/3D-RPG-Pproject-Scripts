using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {

	public class Fighter : MonoBehaviour, IAction {

		[SerializeField] float weaponRange = 2f;
		[SerializeField] float timeBetweenAttacks =1f;
		[SerializeField] float weaponDamage = 5f;

		Health target;
		float timeSinceLastAttack = 0;

		private void Update() {
      timeSinceLastAttack += Time.deltaTime;

			if (target == null) return;
			if(target.IsDead()) return;

			// Commenting this will stop moving on attacking
      if (!GetIsInRange()) {
        GetComponent<Mover>().MoveTo(target.transform.position);
      } else { // we are in range
        GetComponent<Mover>().Cancel();
        AttackBehaviour();
      }
    }

    private void AttackBehaviour() {

			transform.LookAt(target.transform);
			if(timeSinceLastAttack > timeBetweenAttacks) {
        // This will trigger the Hit() event. Super smart.
        TriggerAttack();
        timeSinceLastAttack = 0;

      }
    }

    private void TriggerAttack() {
      GetComponent<Animator>().ResetTrigger("stopAttack");
      GetComponent<Animator>().SetTrigger("attack");
    }

    // Animation Event ( do damage when fist actually hits!)
    void Hit() {
			if(target == null) return;
      target.TakeDamage(weaponDamage);
    }

    private bool GetIsInRange() {
      return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
    }

		public bool CanAttack(CombatTarget commbatTarget){
      if (commbatTarget == null) return false;
			Health targetToTest = commbatTarget.GetComponent<Health>();
			return targetToTest != null && !targetToTest.IsDead(); 
		}

    public void Attack(CombatTarget combatTarget){
			GetComponent<ActionScheduler>().StartAction(this);
			target = combatTarget.GetComponent<Health>();
		}

		public void Cancel() {
      StopAttack();
      target = null;
    }

    private void StopAttack() {
      GetComponent<Animator>().ResetTrigger("attack");
      GetComponent<Animator>().SetTrigger("stopAttack");
    }
  }

}