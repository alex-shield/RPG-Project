using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Control {
    public class AIController : MonoBehaviour {

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointGuardTime = 2f;
        [SerializeField] PatrolPath patrolPath;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [Range(0, 1)]
        [SerializeField] float attackSpeedFraction = .8f;

        float timeAtWaypoint = Mathf.Infinity;
        int waypointIndex = 0;

        GameObject player;
        Fighter fighter;
        Mover mover;
        Health health;

        Vector3 startingLocation;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        Vector3 lastKnownPlayerPosition;

        private void Awake() {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }

        private void Start() {
            startingLocation = transform.position;
        }

        private void Update() {
            if (health.IsDead()) return;
            if (GetIsInRange(player)) {
                AttackBehavior();
            } else if (timeSinceLastSawPlayer < suspicionTime) {
                SuspicionBehavior();
            } else {
                PatrolBehavior();
            }
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void AttackBehavior() {
            timeSinceLastSawPlayer = 0;
            lastKnownPlayerPosition = player.transform.position;
            fighter.Attack(player);
        }

        private void SuspicionBehavior() {
            if (transform.position != lastKnownPlayerPosition) {
                mover.StartMoveAction(lastKnownPlayerPosition, attackSpeedFraction);
            } else {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }

        private void PatrolBehavior() {
            Vector3 nextPosition = startingLocation;

            if(patrolPath != null) {
                if (AtWaypoint()) {
                    if (timeAtWaypoint > waypointGuardTime) {
                        waypointIndex = CycleWayPoint();
                        timeAtWaypoint = 0f;
                    } else {
                        timeAtWaypoint += Time.deltaTime;
                        return;
                    }
                }
                nextPosition = GetCurrentWayPoint();
            }
            
            mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }

        private bool AtWaypoint() {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private int CycleWayPoint() {
            return patrolPath.GetNextIndex(waypointIndex);
        }

        private Vector3 GetCurrentWayPoint() {
            return patrolPath.GetWaypoint(waypointIndex);
        }

        private bool GetIsInRange(GameObject player) {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        // Called by Unity Editor
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}