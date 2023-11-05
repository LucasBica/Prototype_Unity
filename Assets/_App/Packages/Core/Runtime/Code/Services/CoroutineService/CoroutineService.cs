using System.Collections;

using UnityEngine;

namespace LB.Core.Runtime {

    public class CoroutineService : ICoroutineService {

        private readonly MonoBehaviour monoBehaviour;

        public CoroutineService(MonoBehaviour monoBehaviour) {
            this.monoBehaviour = monoBehaviour;
        }

        public Coroutine StartCoroutine(IEnumerator routine) {
            return monoBehaviour.StartCoroutine(routine);
        }

        public Coroutine StartCoroutine(string methodName) {
            return monoBehaviour.StartCoroutine(methodName);
        }

        public Coroutine StartCoroutine(string methodName, object value) {
            return monoBehaviour.StartCoroutine(methodName, value);
        }

        public void StopCoroutine(IEnumerator routine) {
            monoBehaviour.StopCoroutine(routine);
        }

        public void StopCoroutine(Coroutine routine) {
            monoBehaviour.StopCoroutine(routine);
        }

        public void StopCoroutine(string methodName) {
            monoBehaviour.StopCoroutine(methodName);
        }

        public void StopAllCoroutines() {
            monoBehaviour.StopAllCoroutines();
        }
    }
}