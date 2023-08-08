using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, IAction, IJsonSaveable {

        NavMeshAgent navMeshAgent;
        [SerializeField] float maxSpeed = 6f;

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            UpdateAnimator();
        }

        //public void StartMoveAction(Vector3 destination)
        //{
        //    GetComponent<ActionScheduler>().StartAction(this);
        //    MoveTo(destination);
        //}

        public void StartMoveAction(Vector3 destination, float speedFraction) {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        //public void MoveTo(Vector3 destination) {
        //    navMeshAgent.destination = destination;
        //    navMeshAgent.isStopped = false;
        //}

        public void MoveTo(Vector3 destination, float speedFraction) {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        private void UpdateAnimator() {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void Cancel() {
            navMeshAgent.isStopped = true;
        }

        [System.Serializable]
        struct MoverSaveData {
            public Vector3 position;
            public Vector3 rotation;
        }

        public JToken CaptureAsJToken() {
            MoverSaveData token = new MoverSaveData();
            token.position = transform.position;
            token.rotation = transform.rotation.eulerAngles;
            return JsonUtility.ToJson(token);
        }

        public void RestoreFromJToken(JToken state) {
            MoverSaveData token = JsonUtility.FromJson<MoverSaveData>((string)state);
            navMeshAgent.enabled = false;
            transform.position = token.position;
            transform.eulerAngles = token.rotation;
            navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}