using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class ControlRemover : MonoBehaviour {

        GameObject player;

        private void Awake() {
            player = GameObject.FindWithTag("Player");
        }

        private void Start() {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector a) {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
            print("Disabled Controls");
        }

        private void EnableControl(PlayableDirector a) {
            player.GetComponent<PlayerController>().enabled = true;
            print("Enabled Controls");
        }
    }
}
