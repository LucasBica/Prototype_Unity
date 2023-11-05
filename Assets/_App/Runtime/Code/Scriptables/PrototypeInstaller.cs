using LB.Authentication.Runtime.Mvc;
using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;
using LB.Prototype.Runtime.Mvc;

using UnityEngine;

namespace LB.Prototype.Runtime {

    [CreateAssetMenu(fileName = nameof(PrototypeInstaller), menuName = Consts.PATH_ASSET_INSTALLERS + nameof(PrototypeInstaller))]
    public class PrototypeInstaller : ContainerInstaller {

        [Header("Settings")]
        [SerializeField] private int targetFrameRate = default;

        public override void Initialize() {
            Application.targetFrameRate = targetFrameRate;

            IMessenger messenger = DIContainer.Get<IMessenger>();
            ISignOutController signOutController = DIContainer.Get<ISignOutController>();
            ICacheAsTextService cacheAsTextService = DIContainer.Get<ICacheAsTextService>();

            DIContainer.Set<IMenuController>(new MenuController());
            DIContainer.Set<ISettingsController>(new SettingsController(messenger, signOutController, cacheAsTextService, () => new ChoiceConfirmationModel()));
        }
    }
}