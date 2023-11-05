using LB.Authentication.Runtime;
using LB.Authentication.Runtime.Mvc;
using LB.Core.Runtime;
using LB.MessageSystem.Runtime;

using UnityEngine;

namespace LB.Authentication.Testing {

    public class AuthenticationTest : MonoBehaviour {

        [ContextMenu(nameof(SignInFromUI))]
        public void SignInFromUI() {
            IMessenger messenger = DIContainer.Get<IMessenger>();
            ISignInController controller = DIContainer.Get<ISignInController>();
            AuthenticationModel model = new AuthenticationModel {
                email = "lucasgabrielbica@gmail.com",
                password = "Pass1234!",
            };

            messenger.Send(AuthenticationCallbacks.UpdateSignIn, model);
            controller.OnClickSignIn(model);
        }
    }
}