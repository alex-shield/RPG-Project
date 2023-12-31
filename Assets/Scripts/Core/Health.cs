using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Saving;
using Newtonsoft.Json.Linq;

namespace RPG.Core {
    public class Health : MonoBehaviour, IJsonSaveable {

        [SerializeField] float hitPoints = 100f;
        [SerializeField] bool isDead = false;

        public void TakeDamage(float damage) {
            hitPoints = Mathf.Max(hitPoints -damage, 0);
            if (hitPoints <= 0 && !isDead) {
                Die();
            }
        }

        //private void Die() {
        //    isDead = true;
        //    GetComponent<Animator>().SetTrigger("die");
        //    GetComponent<ActionScheduler>().CancelCurrentAction();
        //    GetComponent<NavMeshAgent>().enabled = false;
        //    if (this.gameObject.tag != "Player") {
        //        GetComponent<CapsuleCollider>().enabled = false;
        //        GetComponent<Fighter>().enabled = false;
        //        Invoke(nameof(Decompose), 2f);
        //    }
        //}

        private void Die() {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        //private void Decompose() {
        //    GetComponent<Rigidbody>().AddForce(Vector3.down * 1f);
        //    GetComponent<Rigidbody>().drag = 30f;
        //    Destroy(this.gameObject, 5f);
        //}

        public bool IsDead() {
            return isDead;
        }

        public JToken CaptureAsJToken() {
            return JToken.FromObject(hitPoints);
        }

        public void RestoreFromJToken(JToken state) {
            hitPoints = (float)state;
            if (hitPoints <= 0) {
                Die();
            } else {
                isDead = false;
                GetComponent<Animator>().Rebind();
            }
        }
    }
}
