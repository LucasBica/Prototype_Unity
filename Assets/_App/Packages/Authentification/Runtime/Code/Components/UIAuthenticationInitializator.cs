using LB.Core.Runtime;
using LB.MessageSystem.Runtime;

using UnityEngine;

namespace LB.Authentication.Runtime.Mvc {

    public class UIAuthenticationInitializator : MonoBehaviour {

        protected virtual void Start() {
            DIContainer.Get<IMessenger>().Send(AuthenticationCallbacks.UpdateSignIn, new AuthenticationModel {
                shouldAppear = true,
                animated = false,
            });
        }
    }
}