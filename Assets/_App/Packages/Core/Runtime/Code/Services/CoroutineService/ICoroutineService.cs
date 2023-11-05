using System.Collections;

using UnityEngine;

namespace LB.Core.Runtime {

    public interface ICoroutineService {

        public Coroutine StartCoroutine(IEnumerator routine);

        public Coroutine StartCoroutine(string methodName);

        public Coroutine StartCoroutine(string methodName, object value);

        public void StopCoroutine(IEnumerator routine);

        public void StopCoroutine(Coroutine routine);

        public void StopCoroutine(string methodName);

        public void StopAllCoroutines();
    }
}