namespace LB.Core.Runtime {

    public interface ICacheAsBytesService {

        public void ClearCacheExpired();

        public bool HasKey(string key);

        public byte[] Get(string key, byte[] defaultValue = default);

        public void Set(string key, byte[] value);

        public void Save();
    }
}