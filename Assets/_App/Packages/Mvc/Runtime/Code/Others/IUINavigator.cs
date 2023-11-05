using System;

namespace LB.Mvc.Runtime {

    public interface IUINavigator {

        public event Action<ViewAndModel> OnPush;
        public event Action<ViewAndModel> OnPop;
        public event Action<ViewAndModel[]> OnPopToRoot;
        public event Action<ViewAndModel[]> OnPopToView;

        public ViewAndModel TopView { get; }

        public ViewAndModel[] Views { get; }

        public int StackCount { get; }

        public bool IsTransitioning { get; }

        public ViewAndModel GetByName(string displayName);

        public ViewAndModel[] GetAllByName(string displayName);

        public int GetIndex(UIBaseViewUpdater view);

        public void SetViewAndModel(ViewAndModel[] viewAndModels, bool animated);

        public void PushViewAndModel(ViewAndModel viewAndModel);

        public ViewAndModel PopViewAndModel();

        public ViewAndModel[] PopToRootViewAndModel();

        public ViewAndModel[] PopToViewAndModel(ViewAndModel viewAndModel);
    }
}