using System;

using DanielLochner.Assets.SimpleScrollSnap;

using LB.Core.Runtime;
using LB.Core.Runtime.Utilities;
using LB.Http.Runtime.Strapi;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;
using LB.UIKit.Runtime.Components;
using LB.UIKit.Runtime.Serializables;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.Topics.Runtime.Mvc {

    public class UITopicSelectionView : UIViewUpdater<ITopicSelectionController, TopicSelectionModel> {

        [Header("Assets")]
        [SerializeField] private UITopic prefabTopic = default;
        [SerializeField] private UITransitionCombinatorViewController transitionDefault = default;
        [SerializeField] private UITransitionCombinatorViewController transitionTopicStart = default;
        [SerializeField] private UITransitionCombinatorViewController transitionTopicInfo = default;

        [Header("References")]
        [SerializeField] private UIButton buttonBack = default;
        [SerializeField] private SimpleScrollSnap scroll = default;

        private GameObjectPooling<UITopic> gameObjectPooling;

        protected override void Awake() {
            base.Awake();

            gameObjectPooling = new(prefabTopic, scroll.Content);
            gameObjectPooling.OnCreate += GameObjectPooling_OnCreate;
            gameObjectPooling.OnActive += GameObjectPooling_OnActive;
            gameObjectPooling.OnRelease += GameObjectPooling_OnRelease;

            buttonBack.OnClick += ButtonBack_OnClick;
        }

        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(TopicsCallbacks.UpdateTopicSelectionView, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(TopicsCallbacks.UpdateTopicSelectionView, action);
        }

        protected override ITopicSelectionController GetController() => DIContainer.Get<ITopicSelectionController>();

        public override void UpdateView(TopicSelectionModel model) {
            base.UpdateView(model);

            if (model.strapiResponseTopic == null || model.strapiResponseTopic.data == null) {
                Debug.LogError($"[{nameof(UITopicSelectionView)}] {nameof(model.strapiResponseTopic)} or its data is null");
                return;
            }

            Data<TopicEntity>[] entities = model.strapiResponseTopic.data;
            UITopic[] activeInstances;
            bool resetScroll = gameObjectPooling.ActiveCount != entities.Length;

            if (gameObjectPooling.ActiveCount < entities.Length) {
                int difference = entities.Length - gameObjectPooling.ActiveCount;
                for (int i = 0; i < difference; i++) {
                    UITopic instance = gameObjectPooling.GetInstance();
                    scroll.AddToBack(instance.RectT);
                }

            } else if (gameObjectPooling.ActiveCount > entities.Length) {
                int difference = gameObjectPooling.ActiveCount - entities.Length;
                for (int i = 0; i < difference; i++) {
                    activeInstances = gameObjectPooling.ActiveInstances;
                    gameObjectPooling.ReleaseInstance(activeInstances[activeInstances.Length - (difference + 1)]);
                }
            }

            activeInstances = gameObjectPooling.ActiveInstances;
            resetScroll = resetScroll && activeInstances.Length > 0;

            if (resetScroll) scroll.GoToPanel(0);

            for (int i = 0; i < activeInstances.Length; i++) {
                activeInstances[i].UpdateContent(model, entities[i]);
            }
        }

        private void GameObjectPooling_OnCreate(UITopic topic) {
            topic.OnClickStart += Topic_OnClickStart;
            topic.OnClickInfo += Topic_OnClickInfo;
        }

        private void GameObjectPooling_OnActive(UITopic topic) {
            topic.gameObject.SetActive(true);
        }

        private void GameObjectPooling_OnRelease(UITopic topic) {
            topic.gameObject.SetActive(false);
        }

        private void Topic_OnClickStart(UITopic topic) {
            ViewController.SetTransitionAppear(transitionTopicStart);
            ViewController.SetTransitionDisappear(transitionTopicStart);

            model.topicSelected = topic.Entity;
            Controller.OnClickStart(model);
        }

        private void Topic_OnClickInfo(UITopic topic) {
            ViewController.SetTransitionAppear(transitionTopicInfo);
            ViewController.SetTransitionDisappear(transitionTopicInfo);

            model.topicSelected = topic.Entity;
            Controller.OnClickInfo(model);
        }

        private void ButtonBack_OnClick(UIButton button, PointerEventData eventData) {
            ViewController.SetTransitionAppear(transitionDefault);
            ViewController.SetTransitionDisappear(transitionDefault);

            Controller.OnClickBack(model);
        }
    }
}