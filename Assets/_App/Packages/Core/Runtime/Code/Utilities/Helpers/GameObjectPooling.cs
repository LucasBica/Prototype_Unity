using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace LB.Core.Runtime.Utilities {

    public class GameObjectPooling<T> where T : Component {

        private readonly LinkedList<T> activeInstances;
        private readonly Stack<T> inactiveInstances;

        public event Action<T> OnCreate;
        public event Action<T> OnActive;
        public event Action<T> OnRelease;

        private readonly T prefab;
        public T Prefab => prefab;

        private readonly Transform parent;
        public Transform Parent => parent;

        public T[] ActiveInstances => activeInstances.ToArray();
        public T[] InactiveInstances => inactiveInstances.ToArray();

        public int ActiveCount => activeInstances.Count;
        public int InactiveCount => inactiveInstances.Count;
        public int ObjectCount => ActiveCount + InactiveCount;

        public GameObjectPooling(T prefab, Transform parent, int capacity = 16) {
            this.prefab = prefab;
            this.parent = parent;

            activeInstances = new();
            inactiveInstances = new(capacity);
        }

        public T GetInstance() {
            if (!inactiveInstances.TryPop(out T instance)) {
                instance = UnityEngine.Object.Instantiate(prefab, parent, false);
                OnCreate?.Invoke(instance);
            } else {
                OnActive?.Invoke(instance);
            }

            activeInstances.AddLast(instance);

            return instance;
        }

        public bool ReleaseInstance(T instance) {
            if (!activeInstances.Contains(instance)) {
                return false;
            }

            if (inactiveInstances.Contains(instance)) {
                return false;
            }

            activeInstances.Remove(instance);
            inactiveInstances.Push(instance);
            OnRelease?.Invoke(instance);

            return true;
        }

        public void ReleaseAll() {
            for (int i = activeInstances.Count - 1; i >= 0; i--) {
                ReleaseInstance(activeInstances.ElementAt(i));
            }
        }
    }
}