using LB.Authentication.Runtime;
using LB.Core.Runtime;
using LB.Http.Runtime;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;
using LB.Topics.Runtime.Mvc;

using UnityEngine;

namespace LB.Topics.Runtime {

    [CreateAssetMenu(fileName = nameof(TopicNodesInstaller), menuName = Consts.PATH_ASSET_INSTALLERS + nameof(TopicNodesInstaller))]
    public class TopicNodesInstaller : ContainerInstaller {

        public override void Initialize() {
            IHttpService httpService = DIContainer.Get<IHttpService>();
            IEnvironmentService environmentService = DIContainer.Get<IEnvironmentService>();
            IMessenger messenger = DIContainer.Get<IMessenger>();
            IInfoController infoController = DIContainer.Get<IInfoController>();
            IAuthenticationService authenticationService = DIContainer.Get<IAuthenticationService>();

            ITopicsRequester topicsRequester = new TopicsRequester(environmentService.Variables.urlAPI, httpService, authenticationService);
            DIContainer.Set(topicsRequester);

            ITopicsService topicsService = new TopicsService(topicsRequester);
            DIContainer.Set(topicsService);

            ITopicSelectionController topicSelectionController = new TopicSelectionController(messenger, topicsService, infoController, () => new TopicSelectionModel(), () => new AlertModel());
            DIContainer.Set(topicSelectionController);
        }
    }
}