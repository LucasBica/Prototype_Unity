using System;
using System.Collections.Generic;

using UnityEngine;

namespace LB.Core.Runtime {

    public static class DIContainer {

        private static readonly Dictionary<Type, object> dictionary = new();

        public static bool Set<T>(T service) where T : class {
            Type type = typeof(T);

            if (dictionary.ContainsKey(type)) {
                Debug.Log($"[{nameof(DIContainer)}] Service already registered: {type}");
                return false;
            }

            dictionary.Add(type, service);
            return true;
        }

        public static T Get<T>() where T : class {
            Type type = typeof(T);

            if (!dictionary.TryGetValue(type, out object service)) {
                Debug.Log($"[{nameof(DIContainer)}] Service not found: {type}");
                return null;
            }

            return (T)service;
        }
    }
}