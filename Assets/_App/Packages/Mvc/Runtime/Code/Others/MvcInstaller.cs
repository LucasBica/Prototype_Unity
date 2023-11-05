using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime.Components;
using LB.TweenSystem.Runtime;

using UnityEngine;

namespace LB.Mvc.Runtime {

    [CreateAssetMenu(fileName = nameof(MvcInstaller), menuName = Consts.PATH_ASSET_INSTALLERS + nameof(MvcInstaller))]
    public class MvcInstaller : ContainerInstaller {

        public override void Initialize() {
            IMessenger messenger = DIContainer.Get<IMessenger>();
            IEnvironmentService environmentService = DIContainer.Get<IEnvironmentService>();

            InfoController.IsDevelopmentBuild = environmentService.Variables.isDevelopmentBuild;

            DIContainer.Set<IAlertController>(new AlertController());
            DIContainer.Set<IInfoController>(new InfoController());
            DIContainer.Set<IChoiceConfirmationController>(new ChoiceConfirmationController());
            DIContainer.Set<ITextConfirmationController>(new TextConfirmationController());

            IUINavigator navigator = new UINavigator();
            DIContainer.Set(navigator);
            UIInput.OnBack += () => navigator.PopViewAndModel();

            ICoroutineService coroutineService = DIContainer.Get<ICoroutineService>();

            ISceneLoadingController sceneLoadingController = new SceneLoadingController(messenger, coroutineService,
                (sceneLoadingModel) => {
                    Debug.Log("OnPreLoadScene");
                    navigator.SetViewAndModel(new ViewAndModel[0], false);
                    Tween.StopAll();
                },
                (sceneLoadingModel) => {
                    Debug.Log("OnPostLoadScene");
                }
            );
            DIContainer.Set(sceneLoadingController);

            DIContainer.Set<ISplashScreenController>(new SplashScreenController(
                (model) => {
                    DIContainer.Get<IMessenger>().Send(MvcCallbacks.UpdateLoadingView, new SceneLoadingModel {
                        shouldAppear = true,
                        animated = false,
                        shouldTryLoadNext = true,
                    });
                }
            ));
        }
    }
}