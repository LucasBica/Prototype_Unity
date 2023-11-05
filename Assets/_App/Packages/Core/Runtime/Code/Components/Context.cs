using System;
using System.Collections;

using UnityEngine;

namespace LB.Core.Runtime {

    public class Context : MonoBehaviour {

        [Header("Assets")]
        [SerializeField] private ContainerInstaller[] installers = default;

        public event Action OnInitialized;

        private void Awake() {
            for (int i = 0; i < installers.Length; i++) {
                installers[i].Initialize();
            }
        }

        private void Start() {
            StartCoroutine(CallOnInitialized());
        }

        private IEnumerator CallOnInitialized() {
            yield return null;
            OnInitialized?.Invoke();
        }
    }
}