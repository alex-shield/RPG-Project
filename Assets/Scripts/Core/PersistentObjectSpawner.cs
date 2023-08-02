using System;
using UnityEngine;

namespace RPG.Core {
    public class PersistentObjectSpawner : MonoBehaviour {
        [SerializeField] GameObject persistentObjectPrefab;

        static bool hasSpawned = false;

        private void Awake() {
            if (hasSpawned) return;
            SpawnPersistentObjects();
            hasSpawned = true;
        }

        private void SpawnPersistentObjects() {
            DontDestroyOnLoad(Instantiate(persistentObjectPrefab));
            print("Spawned Persistent Objects");
        }
    }
}
