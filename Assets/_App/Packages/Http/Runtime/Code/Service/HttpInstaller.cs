using LB.Core.Runtime;

using Newtonsoft.Json;

using UnityEngine;

namespace LB.Http.Runtime {

    [CreateAssetMenu(fileName = nameof(HttpInstaller), menuName = Consts.PATH_ASSET_INSTALLERS + nameof(HttpInstaller))]
    public class HttpInstaller : ContainerInstaller {

        public override void Initialize() {
            JsonSerializerSettings jsonSerializerSettings = new() {
                NullValueHandling = NullValueHandling.Ignore
            };

            DIContainer.Set<IHttpService>(new HttpService(jsonSerializerSettings,
                () => new(null, new(-1, "Error", "Something was wrong, try again", $"This is a error catched from {nameof(HttpService)}."))));
        }
    }
}