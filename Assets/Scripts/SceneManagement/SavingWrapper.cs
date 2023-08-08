using RPG.Saving;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RPG.SceneManagement {
	public class SavingWrapper : MonoBehaviour {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 1f;

        JsonSavingSystem savingSystem;

        private void Awake() {
            savingSystem = GetComponent<JsonSavingSystem>();
        }

        IEnumerator Start() {
            Fader fader = FindAnyObjectByType<Fader>();
            fader.FadeOutImmediate();
            yield return savingSystem.LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            } else if (Input.GetKeyUp(KeyCode.L)) {
                Load();
            }
        }

        public void Save() {
            savingSystem.Save(defaultSaveFile);
        }

        public void Load() {
            savingSystem.Load(defaultSaveFile);
        }
    }
}
