namespace LB.Core.Runtime {

    public interface ICacheAsTextService {

        public void SetKeyPrefix(string prefix);

        public bool HasKey(string key);

        public T Get<T>(string key, T defaultValue = default);

        public void Set<T>(string key, T value);

        public void Save();
    }
}