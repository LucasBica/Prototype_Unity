using Newtonsoft.Json;

using UnityEngine;

namespace LB.Core.Runtime {

    public class EnvironmentService : IEnvironmentService {

        public const string FILENAME = "env";

        public EnvironmentVariablesModel Variables { get; private set; }

        public EnvironmentService() {
            TextAsset textAsset = Resources.Load<TextAsset>(FILENAME);

            if (textAsset == null) {
                Debug.LogError($"[{nameof(EnvironmentService)}] Environment variables file with name '{FILENAME}' not found in resources");
                return;
            }

            Variables = JsonConvert.DeserializeObject<EnvironmentVariablesModel>(textAsset.text, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}