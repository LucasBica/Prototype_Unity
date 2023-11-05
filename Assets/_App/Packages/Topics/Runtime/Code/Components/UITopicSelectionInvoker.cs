using LB.Core.Runtime;
using LB.Topics.Runtime.Mvc;
using LB.UIKit.Runtime.Components;
using LB.UIKit.Runtime.Serializables;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LB.Topics {

    public class UITopicSelectionInvoker : UIView {

        [Header("Assets")]
        [SerializeField] private UITransitionCombinatorViewController transition = default;

        [Header("References")]
        [SerializeField] private UIViewController viewController = default;
        [SerializeField] private GraphicRaycaster graphicRaycaster = default;
        [SerializeField] private SwitchGameObjects switchGameObjects = default;
        [SerializeField] private UIButton button = default;

        private ITopicSelectionController topicSelectionController;
        private TopicSelectionModel model;

        private void OnDestroy() {
            if (topicSelectionController != null) {
                topicSelectionController.OnPropertyChanged -= TopicSelectionController_OnPropertyChanged;
            }
        }

        private void Awake() {
            topicSelectionController = DIContainer.Get<ITopicSelectionController>();
            topicSelectionController.OnPropertyChanged += TopicSelectionController_OnPropertyChanged;
            button.OnClick += Button_OnClick;
        }

        private void TopicSelectionController_OnPropertyChanged(TopicSelectionModel model) {
            this.model = model;
            switchGameObjects.SetState(model.isLoading);
            graphicRaycaster.enabled = !model.isLoading;
        }

        private void Button_OnClick(UIButton button, PointerEventData eventData) {
            viewController.SetTransitionAppear(transition);
            viewController.SetTransitionDisappear(transition);

            switchGameObjects.SetState(true);
            graphicRaycaster.enabled = false;
            topicSelectionController.OnClickAppear(model);
        }
    }
}