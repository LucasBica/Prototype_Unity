using LB.Core.Runtime;
using LB.Http.Models;
using LB.Http.Runtime;

using UnityEngine;

namespace LB.FileLoader.Runtime {

    [CreateAssetMenu(fileName = nameof(FileLoaderInstaller), menuName = Consts.PATH_ASSET_INSTALLERS + nameof(FileLoaderInstaller))]
    public class FileLoaderInstaller : ContainerInstaller {

        [Header("Settings")]
        [SerializeField] private bool useHostPrefix = default;

        public override void Initialize() {
            IEnvironmentService environmentService = DIContainer.Get<IEnvironmentService>();
            ICacheAsBytesService cacheAsBytesService = DIContainer.Get<ICacheAsBytesService>();
            IHttpService httpService = DIContainer.Get<IHttpService>();

            string prefix = useHostPrefix ? environmentService.Variables.host : string.Empty;
            IFileLoaderService fileLoaderService = new FileLoaderService(prefix, httpService, cacheAsBytesService, CreateHttpError);
        }

        private HttpError CreateHttpError() {
            return new HttpError(null, new InfoError(-1, "Error", "Something was wrong, try again", null));
        }
    }
}