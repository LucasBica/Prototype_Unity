using LB.Authentication.Runtime;
using LB.Core.Runtime;
using LB.Http.Models;
using LB.Http.Runtime;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;
using LB.Topics.Runtime.Mvc;

using UnityEngine;

namespace LB.GroupNodes.Runtime {

    [CreateAssetMenu(fileName = nameof(GroupNodesInstaller), menuName = Consts.PATH_ASSET_INSTALLERS + nameof(GroupNodesInstaller))]
    public class GroupNodesInstaller : ContainerInstaller {

        public override void Initialize() {
            IHttpService httpService = DIContainer.Get<IHttpService>();
            IEnvironmentService environmentService = DIContainer.Get<IEnvironmentService>();
            IMessenger messenger = DIContainer.Get<IMessenger>();
            IInfoController infoController = DIContainer.Get<IInfoController>();
            IAuthenticationService authenticationService = DIContainer.Get<IAuthenticationService>();

            IRewardsRequester rewardsRequester = new RewardsRequester(environmentService.Variables.urlAPI, httpService);
            DIContainer.Set(rewardsRequester);

            IRewardsService rewardsService = new RewardsService(rewardsRequester);
            DIContainer.Set(rewardsService);

            INodesRequester nodeRequester = new NodesRequester(environmentService.Variables.urlAPI, httpService);
            DIContainer.Set(nodeRequester);

            INodesService nodeService = new NodesService(nodeRequester, CreateHttpError);
            DIContainer.Set(nodeService);

            ISubGroupNodesRequester subGroupNodesRequester = new SubGroupNodesRequester(environmentService.Variables.urlAPI, httpService);
            DIContainer.Set(subGroupNodesRequester);

            ISubGroupNodesService subGroupNodesService = new SubGroupNodesService(subGroupNodesRequester, CreateHttpError);
            DIContainer.Set(subGroupNodesService);

            IGroupNodesRequester groupNodesRequester = new GroupNodesRequester(environmentService.Variables.urlAPI, httpService, authenticationService);
            DIContainer.Set(groupNodesRequester);

            IGroupNodesService service = new GroupNodesService(groupNodesRequester, subGroupNodesService, nodeService, CreateHttpError);
            DIContainer.Set(service);

            NodeSelectionController nodeSelectionController = new(messenger, service, infoController,
                () => new NodeSelectionModel(),
                () => new AlertModel(),
                () => new HttpError(null, new InfoError(-1, "Error", "Something was wrong, try again", null)));

            DIContainer.Set<INodeSelectionController>(nodeSelectionController);

            ITopicSelectionController topicSelectionController = DIContainer.Get<ITopicSelectionController>();
            topicSelectionController.AddTopicSelectedListener(nodeSelectionController);
        }

        private HttpError CreateHttpError() {
            return new HttpError(null, new InfoError(-1, "Error", "Something was wrong, try again", null));
        }
    }
}