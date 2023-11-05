using System;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;

namespace LB.Prototype.Runtime.Mvc {

    public class UIMenuView : UIViewUpdater<IMenuController, MenuModel> {
        
        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(PrototypeCallbacks.UpdateMenu, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(PrototypeCallbacks.UpdateMenu, action);
        }

        protected override IMenuController GetController() => DIContainer.Get<IMenuController>();

        protected override void OnDidAppear() {
            base.OnDidAppear();
            ViewController.SetTransitionAppear(null);
        }
    }
}