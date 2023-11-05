using System;

using UnityEngine;

namespace LB.TweenSystem.Runtime.Internal {

    public class TweenSingleton : MonoBehaviour {

        private static TweenSingleton instance;
        public static TweenSingleton Instance {
            get {
                CreateIfNotExist();
                return instance;
            }
        }

        public event Action OnUpdate;

        private void Update() {
            OnUpdate?.Invoke();
        }

        public static void CreateIfNotExist() {
            if (instance == null) {
                GameObject gameObject = new("[TweenSystem]");
                DontDestroyOnLoad(gameObject);
                instance = gameObject.AddComponent<TweenSingleton>();
            }
        }
    }
}