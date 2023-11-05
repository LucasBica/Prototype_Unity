using System.IO;

using LB.Core.Runtime.Components;
using LB.Core.Runtime.Utilities;

using UnityEngine;

namespace LB.Core.Runtime {

    [CreateAssetMenu(fileName = nameof(CoreInstaller), menuName = Consts.PATH_ASSET_INSTALLERS + nameof(CoreInstaller))]
    public class CoreInstaller : ContainerInstaller {

        public override void Initialize() {
            DIContainer.Set<IUsernameValidator>(new UsernameValidator());
            DIContainer.Set<IEmailValidator>(new EmailValidator());
            DIContainer.Set<IPasswordValidator>(new PasswordValidator());
            DIContainer.Set<IVersionValidator>(new VersionValidator());
            DIContainer.Set<IEnvironmentService>(new EnvironmentService());
            DIContainer.Set<ICacheAsTextService>(new CacheAsTextService());
            DIContainer.Set<ICacheAsBytesService>(new CacheAsBytesService(14 * 24 * 60 * 60, Path.Combine(Application.persistentDataPath, "cache_app/")));
            DIContainer.Set<ICoroutineService>(new CoroutineService(new GameObject($"[{nameof(CoroutineService)}]").AddComponent<GameObjectSingleton>()));
        }
    }
}