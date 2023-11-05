using System;
using System.Collections.Generic;
using System.Linq;

using LB.Core.Runtime.Extensions;

using UnityEngine;

namespace LB.Mvc.Runtime {

    public class UINavigator : IUINavigator {

        private readonly Stack<ViewAndModel> stack = new(4);

        public event Action<ViewAndModel> OnPush;
        public event Action<ViewAndModel> OnPop;
        public event Action<ViewAndModel[]> OnPopToRoot;
        public event Action<ViewAndModel[]> OnPopToView;

        public ViewAndModel TopView => stack.Count > 0 ? stack.Peek() : null;

        public ViewAndModel[] Views => stack.ToArray();

        public int StackCount => stack.Count;

        public bool IsTransitioning {
            get {
                for (int i = 0; i < stack.Count; i++) {
                    if (stack.ElementAt(i).view.IsTransitioning) {
                        return true;
                    }
                }
                return false;
            }
        }

        public ViewAndModel GetByName(string displayName) {
            for (int i = 0; i < stack.Count; i++) {
                if (stack.ElementAt(i).view.DisplayName == displayName) {
                    return stack.ElementAt(i);
                }
            }

            return null;
        }

        public ViewAndModel[] GetAllByName(string displayName) {
            List<ViewAndModel> list = new(4);
            for (int i = stack.Count - 1; i >= 0; i--) {
                if (stack.ElementAt(i).view.DisplayName == displayName) {
                    list.Add(stack.ElementAt(i));
                }
            }

            return list.ToArray();
        }

        public int GetIndex(UIBaseViewUpdater view) {
            for (int i = 0; i < stack.Count; i++) {
                if (stack.ElementAt(i).view == view) {
                    return i;
                }
            }

            return -1;
        }

        public void SetViewAndModel(ViewAndModel[] viewAndModels, bool animated) {
            int stackCount = stack.Count;
            for (int i = 0; i < stackCount; i++) {
                ViewAndModel viewAndModel = stack.Pop();
                viewAndModel.view.Disappear(animated);
            }

            for (int i = 0; i < viewAndModels.Length; i++) {
                ViewAndModel viewAndModel = viewAndModels[i];

                if (viewAndModel == null) {
                    Debug.LogError($"{nameof(ViewAndModel)} is null");
                    continue;
                }

                if (i == viewAndModels.Length - 1) {
                    viewAndModel.view.Appear(animated);
                    viewAndModel.view.UpdateView(viewAndModel.model);
                }

                stack.Push(viewAndModel);
            }
        }

        public void PushViewAndModel(ViewAndModel viewAndModel) {
            OnPush?.Invoke(viewAndModel);
            Push(viewAndModel);
        }

        public ViewAndModel PopViewAndModel() {
            OnPop?.Invoke(SafePeek());
            ViewAndModel viewAndModel = Pop();

            if (viewAndModel != null) {
                if (stack.TryPeek(out ViewAndModel currentTop)) {
                    currentTop.view.Appear(currentTop.model.animated);
                    currentTop.view.UpdateView(currentTop.model);
                }
            }

            return viewAndModel;
        }

        public ViewAndModel[] PopToRootViewAndModel() {
            if (0 == stack.Count - 1) { // Nothing to pop here. We are in the Root View Controller
                return null;
            }

            ViewAndModel[] viewAndModels = PopToIndex(0, OnPopToRoot);

            if (stack.TryPeek(out ViewAndModel currentTop)) {
                currentTop.view.Appear(currentTop.model.animated);
                currentTop.view.UpdateView(currentTop.model);
            }

            return viewAndModels;
        }

        public ViewAndModel[] PopToViewAndModel(ViewAndModel viewAndModel) {
            int index = (stack.Count - 1) - stack.IndexOf(viewAndModel); // We do this because the index 0 is the last that was pushed in the stack and the first item is in the index (Count - 1)

            if (index == stack.Count - 1) { // Nothing to pop here. If index is greater, we will show an error in the console when call the method PopToIndex
                return null;
            }

            ViewAndModel[] viewAndModels = PopToIndex(index, OnPopToView);

            if (viewAndModels == null) {
                return null; // We already showed an error in the console.
            }

            if (stack.TryPeek(out ViewAndModel currentTop)) {
                currentTop.view.Appear(currentTop.model.animated);
                currentTop.view.UpdateView(currentTop.model);
            }

            return viewAndModels;
        }

        private ViewAndModel[] PopToIndex(int index, Action<ViewAndModel[]> action) {
            if (stack.Count < 2) {
                Debug.LogError($"{nameof(stack)}.{nameof(stack.Count)} is less than two");
                return null;
            }

            if (index < 0) {
                Debug.LogError($"{nameof(stack)}.{nameof(stack.Count)} is less than two");
                return null;
            }

            if (index >= stack.Count - 1) {
                Debug.LogError($"{nameof(index)} is greater than or equals to ({nameof(stack)}.{nameof(stack.Count)} - 1)");
                return null;
            }

            int length = (stack.Count - 1) - index;
            ViewAndModel[] viewAndModels = new ViewAndModel[length];

            for (int i = 0; i < length; i++) {
                viewAndModels[(length - 1) - i] = stack.ElementAt(i);
            }

            action?.Invoke(viewAndModels);

            for (int i = 0; i < length; i++) {
                Pop();
            }

            return viewAndModels;
        }

        private void Push(ViewAndModel viewAndModel) {
            if (viewAndModel == null) {
                Debug.LogError($"{nameof(ViewAndModel)} is null");
                return;
            }

            if (stack.TryPeek(out ViewAndModel currentTop)) {
                currentTop.view.Disappear(currentTop.model.animated);
            }

            stack.Push(viewAndModel);
            viewAndModel.view.Appear(viewAndModel.model.animated);
            viewAndModel.view.UpdateView(viewAndModel.model);
        }

        private ViewAndModel Pop() {
            if (stack.Count < 2) {
                Debug.LogError($"{nameof(stack)}.{nameof(stack.Count)} is less than two");
                return null;
            }

            ViewAndModel viewAndModel = stack.Pop();
            viewAndModel.view.Disappear(viewAndModel.model.animated);

            return viewAndModel;
        }

        private ViewAndModel SafePeek() {
            stack.TryPeek(out ViewAndModel currentTop);
            return currentTop;
        }
    }
}