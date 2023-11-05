using System;
using System.IO;

namespace LB.Core.Runtime {

    public class CacheAsBytesService : ICacheAsBytesService {

        private readonly int expirationDelayInSeconds;
        private readonly string path;

        public CacheAsBytesService(int expirationDelayInSeconds, string path) {
            this.expirationDelayInSeconds = expirationDelayInSeconds;
            this.path = path;
        }

        public void ClearCacheExpired() {
            if (!File.Exists(path)) {
                return;
            }

            DateTime dateTimeExpiration = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(expirationDelayInSeconds));
            string[] files = Directory.GetDirectories(path);

            for (int i = 0; i < files.Length; i++) {
                DateTime dateTime = Directory.GetLastWriteTimeUtc(files[i]);
                if (dateTime > dateTimeExpiration) {
                    File.Delete(files[i]);
                }
            }
        }

        private string GetFullPath(string key) {
            return Path.Combine(path, key);
        }

        public bool HasKey(string key) {
            return File.Exists(GetFullPath(key));
        }

        public byte[] Get(string key, byte[] defaultValue = default) {
            if (!File.Exists(GetFullPath(key))) {
                return defaultValue;
            }

            byte[] bytes = File.ReadAllBytes(GetFullPath(key));
            return bytes;
        }

        public void Set(string key, byte[] value) {
            if (File.Exists(GetFullPath(key))) {
                File.Delete(GetFullPath(key));
            }

            File.WriteAllBytes(GetFullPath(key), value);
        }

        public void Save() {
            // Nothing to do.
        }
    }
}