using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

namespace RPG.Core {
    public class FollowCamera : MonoBehaviour {
        [SerializeField] Transform target;

        void LateUpdate() {
            transform.position = target.position;
        }
    }
}
