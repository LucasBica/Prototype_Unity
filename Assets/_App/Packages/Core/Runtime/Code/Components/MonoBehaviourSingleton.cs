using UnityEngine;

namespace LB.Core.Runtime {

    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour {

        public static T Instance { get; private set; }

        protected virtual void Awake() {
            if (Instance == null) {
                DontDestroyOnLoad(gameObject);
                Instance = GetInstance();
                Initialize();
            } else {
                Destroy(gameObject);
            }
        }

        protected abstract T GetInstance();

        protected abstract void Initialize();
    }
}