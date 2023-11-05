using Newtonsoft.Json;

namespace LB.Core.Runtime.Utilities {

    public static class StaticUtilities {

        public static string ToString<T>(T t) {
            return $"{t.GetType()} -> {JsonConvert.SerializeObject(t)}";
        }
    }
}