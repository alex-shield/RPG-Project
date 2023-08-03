using RPG.Saving;
using System.Collections;
using UnityEngine;
namespace RPG.SceneManagement {
	public class SavingWrapper : MonoBehaviour {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 1f;

        IEnumerator Start() {
            Fader fader = FindAnyObjectByType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
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
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load() {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}
