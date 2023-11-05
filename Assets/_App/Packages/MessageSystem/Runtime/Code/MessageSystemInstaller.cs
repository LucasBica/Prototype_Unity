using LB.Core.Runtime;

using UnityEngine;

namespace LB.MessageSystem.Runtime {

    [CreateAssetMenu(fileName = nameof(MessageSystemInstaller), menuName = Consts.PATH_ASSET_INSTALLERS + nameof(MessageSystemInstaller))]
    public class MessageSystemInstaller : ContainerInstaller {

        public override void Initialize() {
            IEnvironmentService environmentService = DIContainer.Get<IEnvironmentService>();
            DIContainer.Set<IMessenger>(new Messenger(environmentService.Variables.isDevelopmentBuild));
        }
    }
}