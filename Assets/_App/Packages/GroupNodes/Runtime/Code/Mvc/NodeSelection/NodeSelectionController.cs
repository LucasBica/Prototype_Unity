using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;
using LB.Topics.Runtime;

namespace LB.GroupNodes.Runtime {

    public class NodeSelectionController : Controller<NodeSelectionModel>, INodeSelectionController, ITopicSelectedListener {

        private readonly IMessenger messenger;
        private readonly IGroupNodesService groupNodesService;
        private readonly IInfoController infoController;

        private readonly Func<NodeSelectionModel> funcNodeSelectionModel;
        private readonly Func<AlertModel> funcAlertModel;
        private readonly Func<HttpError> funcHttpError;

        public NodeSelectionController(IMessenger messenger, IGroupNodesService groupNodesService, IInfoController infoController, Func<NodeSelectionModel> funcNodeSelectionModel, Func<AlertModel> funcAlertModel, Func<HttpError> funcHttpError) {
            this.messenger = messenger;
            this.groupNodesService = groupNodesService;
            this.infoController = infoController;

            this.funcNodeSelectionModel = funcNodeSelectionModel;
            this.funcAlertModel = funcAlertModel;
            this.funcHttpError = funcHttpError;
        }

        public bool CanPerform(Data<TopicEntity> topicEntity) {
            return topicEntity.attributes.data_type == "group-node" && int.TryParse(topicEntity.attributes.data_id, out _);
        }

        public void Perform(Data<TopicEntity> topicEntity, Action onComplete) {
            if (!int.TryParse(topicEntity.attributes.data_id, out int id)) {
                HttpError httpError = funcHttpError.Invoke();
                httpError.error.details = $"Invalid topic id: {id}";
                AppearError(httpError, funcAlertModel.Invoke(), infoController, messenger);
                return;
            }

            groupNodesService.Get(id,
                (response) => {
                    onComplete?.Invoke();
                    NodeSelectionModel model = funcNodeSelectionModel.Invoke();
                    model.entity = response.data;
                    Appear(model);
                },
                (httpError) => {
                    onComplete?.Invoke();
                    AppearError(httpError, funcAlertModel.Invoke(), infoController, messenger);
                }
            );
        }

        private void Appear(NodeSelectionModel model) {
            model.isLoading = false;
            model.shouldAppear = true;
            model.animated = true;
            CallOnPropertyChanged(model);

            messenger.Send(GroupNodesCallbacks.UpdateNodeSelectonView, model);
        }

        public void OnClickNode(NodeSelectionModel model) {

        }

        public void OnClickBack(NodeSelectionModel model) {
            model.shouldDisappear = true;
            CallOnPropertyChanged(model);
        }
    }
}