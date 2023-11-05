using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.UIKit.Runtime.Components;

namespace LB.Mvc.Runtime {

    public abstract class UIViewUpdater<TController, TModel> : UIBaseViewUpdater where TController : IController<TModel> where TModel : ViewUpdaterModel, new() {

        protected IUINavigator navigator;
        public IUINavigator Navigator {
            get {
                navigator ??= DIContainer.Get<IUINavigator>();
                return navigator;
            }
        }

        protected UIViewController viewController;
        public UIViewController ViewController {
            get {
                if (viewController == null) {
                    viewController = GetComponent<UIViewController>();
                }
                return viewController;
            }
        }

        public override bool IsTransitioning => ViewController.IsTransitioning;

        public override string DisplayName => ViewController.DisplayName;

        public TController controller;
        public TController Controller {
            get {
                if (controller == null) {
                    controller = GetController();
                    controller.OnPropertyChanged += UpdateView;
                }
                return controller;
            }
        }

        public TModel model;
        public TModel Model => model;

        protected virtual void OnDestroy() {
            if (controller != null) {
                controller.OnPropertyChanged -= UpdateView;
            }
        }

        protected virtual void Awake() {
            ViewController.OnWillAppear += (x) => OnWillAppear();
            ViewController.OnDidAppear += (x) => OnDidAppear();
            ViewController.OnWillDisappear += (x) => OnWillDisappear();
            ViewController.OnDidDisappear += (x) => OnDidDisappear();
        }

        protected abstract TController GetController();

        public override void UpdateView(IMessage message) {
            if (!message.GetContent(out model)) {
                return;
            }

            UpdateView(model);
        }

        public override void UpdateView(object model) {
            this.model = (model as TModel) ?? new();
        }

        public virtual void UpdateView(TModel model) {
            if (this.model.shouldAppear) {
                this.model.shouldAppear = false;
                Navigator.PushViewAndModel(new ViewAndModel(this, model));

            } else if (this.model.shouldDisappear) {
                this.model.shouldDisappear = false;
                Navigator.PopViewAndModel();
            }
        }

        public override void Appear(bool animated) {
            ViewController.Appear(animated);
            ViewController.Canvas.sortingOrder = Navigator.StackCount - Navigator.GetIndex(this);
        }

        public override void Disappear(bool animated) {
            ViewController.Disappear(animated);
        }
    }
}