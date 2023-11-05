using LB.Core.Runtime;
using LB.Prototype.Runtime;
using LB.Prototype.Runtime.Mvc;
using LB.MessageSystem.Runtime;

using UnityEngine;

public class UIMenuInitializator : MonoBehaviour {

    protected virtual void Start() {
        DIContainer.Get<IMessenger>().Send(PrototypeCallbacks.UpdateMenu, new MenuModel {
            shouldAppear = true,
            animated = true,
        });
    }
}
