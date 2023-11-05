using System.Collections.Generic;

using UnityEngine;

namespace LB.Core.Runtime {

    [System.Serializable]
    public class DictionarySerializable<TKey, TValue> {

        [SerializeField] private KeyValuePair<TKey, TValue>[] keyValuePairs = default;

        private Dictionary<TKey, TValue> dictionary;
        public Dictionary<TKey, TValue> Dictionary {
            get {
                if (keyValuePairs == null) {
                    dictionary.Clear();
                    return dictionary;
                }

                if (dictionary == null || dictionary.Count != keyValuePairs.Length) {
                    dictionary = new();
                    for (int i = 0; i < keyValuePairs.Length; i++) {
                        dictionary.Add(keyValuePairs[i].key, keyValuePairs[i].value);
                    }
                }

                return new Dictionary<TKey, TValue>(dictionary);
            }
        }
    }

    [System.Serializable]
    public class KeyValuePair<TKey, TValue> {
        public TKey key;
        public TValue value;
    }
}