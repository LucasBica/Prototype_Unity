using System;
using System.Collections.Generic;

using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;

using UnityEngine;

namespace LB.Topics.Runtime.Mvc {

    public class TopicSelectionController : Controller<TopicSelectionModel>, ITopicSelectionController {

        private readonly IMessenger messenger;
        private readonly ITopicsService topicsService;
        private readonly IInfoController infoController;

        private readonly List<ITopicSelectedListener> listeners = new(2);

        private readonly Func<TopicSelectionModel> funcTopicSelectionModel;
        private readonly Func<AlertModel> funcAlertModel;

        public TopicSelectionController(IMessenger messenger, ITopicsService topicsService, IInfoController infoController, Func<TopicSelectionModel> funcTopicSelectionModel, Func<AlertModel> funcAlertModel) {
            this.messenger = messenger;
            this.topicsService = topicsService;
            this.infoController = infoController;

            this.funcTopicSelectionModel = funcTopicSelectionModel;
            this.funcAlertModel = funcAlertModel;
        }

        public void AddTopicSelectedListener(ITopicSelectedListener listener) {
            if (listeners.Contains(listener)) {
                Debug.LogError($"[{nameof(TopicSelectionController)}] The list {nameof(listeners)} already contains this listener: {listener}");
                return;
            }

            listeners.Add(listener);
        }

        public void RemoveTopicSelectedListener(ITopicSelectedListener listener) {
            if (!listeners.Contains(listener)) {
                Debug.LogError($"[{nameof(TopicSelectionController)}] The list {nameof(listeners)} do not contains this listener: {listener}");
                return;
            }

            listeners.Remove(listener);
        }

        private void Appear(TopicSelectionModel model) {
            model.isLoading = false;
            model.shouldAppear = true;
            model.animated = true;
            CallOnPropertyChanged(model);

            messenger.Send(TopicsCallbacks.UpdateTopicSelectionView, model);
        }

        public void OnClickAppear(TopicSelectionModel model) {
            if (model != null && model.strapiResponseTopic != null) {
                Appear(model);
                return;
            }

            model = funcTopicSelectionModel.Invoke();
            model.isLoading = true;
            CallOnPropertyChanged(model);

            topicsService.GetAllTopics(
                (strapiResponseTopic) => {
                    model.strapiResponseTopic = strapiResponseTopic;
                    Appear(model);
                },
                (httpError) => {
                    model.isLoading = false;
                    CallOnPropertyChanged(model);
                    AppearError(httpError, funcAlertModel.Invoke(), infoController, messenger);
                }
            );
        }

        public void OnClickStart(TopicSelectionModel model) {
            if (model.topicSelected == null) {
                return;
            }

            model.isLoading = true;
            CallOnPropertyChanged(model);

            for (int i = 0; i < listeners.Count; i++) {
                if (listeners[i].CanPerform(model.topicSelected)) {
                    listeners[i].Perform(model.topicSelected,
                        () => {
                            model.isLoading = false;
                            CallOnPropertyChanged(model);
                        }
                    );
                    return;
                }
            }

            Debug.LogError($"[{GetType()}] Do not found a listener that match with the {nameof(model.topicSelected)}: {model.topicSelected}");
        }

        public void OnClickInfo(TopicSelectionModel model) {
            if (model.topicSelected == null) {
                return;
            }

        }

        public void OnClickBack(TopicSelectionModel model) {
            model.shouldDisappear = true;
            CallOnPropertyChanged(model);
        }
    }
}