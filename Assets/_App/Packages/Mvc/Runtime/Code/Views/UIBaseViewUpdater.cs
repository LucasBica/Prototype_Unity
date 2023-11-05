using System;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.UIKit.Runtime.Components;

namespace LB.Mvc.Runtime {

    public abstract class UIBaseViewUpdater : UIView {

        public abstract string DisplayName { get; }

        public abstract bool IsTransitioning { get; }

        protected IMessenger messenger;
        protected IMessenger Messenger {
            get {
                messenger ??= DIContainer.Get<IMessenger>();
                return messenger;
            }
        }

        public abstract void UpdateView(IMessage message);

        public abstract void UpdateView(object model);

        public abstract void AttachIt(Action<IMessage> action);

        public abstract void DetachIt(Action<IMessage> action);

        protected virtual void OnWillAppear() { }

        protected virtual void OnDidAppear() { }

        protected virtual void OnWillDisappear() { }

        protected virtual void OnDidDisappear() { }

        public abstract void Appear(bool animated);

        public abstract void Disappear(bool animated);
    }
}