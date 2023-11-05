using UnityEngine;

namespace LB.UIKit.Runtime.Components {

    public class SwitchGameObjects : UIView {

        [Header("Settings")]
        [SerializeField] private bool defaultState = default;

        [Header("References")]
        [SerializeField] private GameObject[] objectsTrue = default;
        [SerializeField] private GameObject[] objectsFalse = default;

        private void Awake() {
            SetState(defaultState);
        }

        public void SetState(bool state) {
            for (int i = 0; i < objectsTrue.Length; i++) {
                objectsTrue[i].SetActive(state);
            }

            for (int i = 0; i < objectsFalse.Length; i++) {
                objectsFalse[i].SetActive(!state);
            }
        }
    }
}