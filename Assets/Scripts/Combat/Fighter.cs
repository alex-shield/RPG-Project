using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        GameObject combatTarget;
        Mover mover;
        Health health;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake() {
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }

        private void Update() {
            if (health.IsDead()) return;
            timeSinceLastAttack += Time.deltaTime;
            if (combatTarget == null) return;
            if (combatTarget.GetComponent<Health>().IsDead()) {
                combatTarget = null;
                return;
            }

            if (!GetIsInRange()) {
                mover.MoveTo(combatTarget.transform.position, 1f);
            } else {
                mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior() {
            transform.LookAt(combatTarget.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks) {
                GetComponent<Animator>().ResetTrigger("stopAttack");
                // Will trigger attack animation
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        private bool GetIsInRange() {
            return Vector3.Distance(combatTarget.transform.position, transform.position) < weaponRange;
        }

        public void Attack(GameObject target) {
            GetComponent<ActionScheduler>().StartAction(this);
            combatTarget = target.gameObject;
        }

        // Triggered by animation
        private void Hit() {
            if(combatTarget == null) return;
            combatTarget.GetComponent<Health>().TakeDamage(weaponDamage);
        }

        public void Cancel() {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            combatTarget = null;
            mover.Cancel();
        }
    }
}
