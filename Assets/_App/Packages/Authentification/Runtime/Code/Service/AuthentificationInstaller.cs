using LB.Authentication.Models;
using LB.Authentication.Runtime.Mvc;
using LB.Core.Runtime;
using LB.Core.Runtime.Utilities;
using LB.Http.Runtime;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;

using UnityEngine;

namespace LB.Authentication.Runtime {

    [CreateAssetMenu(fileName = nameof(AuthentificationInstaller), menuName = Consts.PATH_ASSET_INSTALLERS + nameof(AuthentificationInstaller))]
    public class AuthentificationInstaller : ContainerInstaller {

        [Header("Settings")]
        [SerializeField] protected string sceneToLoadAfterSignUp = default;
        [SerializeField] protected string sceneToLoadAfterSignIn = default;
        [SerializeField] protected string sceneToLoadAfterSignOut = default;

        public override void Initialize() {
            IEnvironmentService environmentService = DIContainer.Get<IEnvironmentService>();
            ICacheAsTextService cacheTextService = DIContainer.Get<ICacheAsTextService>();
            IHttpService httpService = DIContainer.Get<IHttpService>();
            IMessenger messenger = DIContainer.Get<IMessenger>();

            IUsernameValidator usernameValidator = DIContainer.Get<IUsernameValidator>();
            IEmailValidator emailValidator = DIContainer.Get<IEmailValidator>();
            IPasswordValidator passwordValidator = DIContainer.Get<IPasswordValidator>();

            IInfoController infoController = DIContainer.Get<IInfoController>();
            ISceneLoadingController sceneLoader = DIContainer.Get<ISceneLoadingController>();

            IAuthenticationRequester authenticationRequester = new AuthenticationRequester(environmentService.Variables.urlAPI, httpService);
            DIContainer.Set(authenticationRequester);

            IAuthenticationService authenticationService = new AuthenticationService(usernameValidator, emailValidator, passwordValidator, authenticationRequester);
            DIContainer.Set(authenticationService);

            IRecoveryPasswordController recoveryPasswordController = new RecoveryPasswordController(authenticationService, infoController, messenger);
            DIContainer.Set(recoveryPasswordController);

            DIContainer.Set<ISignOutController>(new SignOutController(authenticationService,
                (choiceConfirmationModel) => {
                    messenger.Send(MvcCallbacks.UpdateLoadingView, new SceneLoadingModel {
                        shouldAppear = true,
                        animated = true,
                        isVisible = true,
                        sceneToLoad = sceneToLoadAfterSignOut
                    });
                })
            );

            SignUpController signUpController = new(
                authenticationService,
                infoController,
                messenger,
                (signInResponse) => {
                    cacheTextService.SetKeyPrefix($"{signInResponse.user.id}_");
                    messenger.Send(MvcCallbacks.UpdateLoadingView, new SceneLoadingModel {
                        shouldAppear = true,
                        animated = true,
                        isVisible = true,
                        sceneToLoad = sceneToLoadAfterSignUp,
                        shouldTryLoadNext = true,
                    });
                },
                () => new SignUpRequest(),
                () => new AuthenticationModel(),
                () => new AlertModel()
            );

            DIContainer.Set<ISignUpController>(signUpController);

            SignInController signInController = new(
                authenticationService,
                recoveryPasswordController,
                infoController,
                messenger,
                (signInResponse) => {
                    cacheTextService.SetKeyPrefix($"{signInResponse.user.id}_");
                    messenger.Send(MvcCallbacks.UpdateLoadingView, new SceneLoadingModel {
                        shouldAppear = true,
                        animated = true,
                        isVisible = true,
                        sceneToLoad = sceneToLoadAfterSignIn,
                        shouldTryLoadNext = true,
                    });
                },
                () => new SignInRequest(),
                () => new AuthenticationModel(),
                () => new AlertModel(),
                () => new TextConfirmationModel()
            );

            DIContainer.Set<ISignInController>(signInController);
        }
    }
}