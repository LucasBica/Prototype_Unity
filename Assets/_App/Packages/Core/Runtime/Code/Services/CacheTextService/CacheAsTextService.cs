using Newtonsoft.Json;

using UnityEngine;

namespace LB.Core.Runtime {

    public class CacheAsTextService : ICacheAsTextService {

        private string prefix;

        private string GetKeyWithPrefix(string key) {
            return prefix + key;
        }

        public void SetKeyPrefix(string prefix) {
            this.prefix = prefix;
        }

        public bool HasKey(string key) {
            return PlayerPrefs.HasKey(GetKeyWithPrefix(key));
        }

        public T Get<T>(string key, T defaultValue = default) {
            if (!PlayerPrefs.HasKey(GetKeyWithPrefix(key))) {
                return defaultValue;
            }

            return JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(GetKeyWithPrefix(key)));
        }

        public void Set<T>(string key, T value) {
            PlayerPrefs.SetString(GetKeyWithPrefix(key), JsonConvert.SerializeObject(value));
        }

        public void Save() {
            PlayerPrefs.Save();
        }
    }
}