using System;
using System.Collections.Generic;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class GroupNodesService : IGroupNodesService {

        private readonly IGroupNodesRequester requester;
        private readonly ISubGroupNodesService subGroupNodesService;
        private readonly INodesService nodeService;

        private readonly Dictionary<int, StrapiResponse<Data<GroupNodeEntity>>> groupNodesInRAM = new(4);

        private readonly Func<HttpError> funcHttpError;

        public GroupNodesService(IGroupNodesRequester requester, ISubGroupNodesService subGroupNodesService, INodesService nodeService, Func<HttpError> funcHttpError) {
            this.requester = requester;
            this.subGroupNodesService = subGroupNodesService;
            this.nodeService = nodeService;
            this.funcHttpError = funcHttpError;
        }

        public void Get(int id, Action<StrapiResponse<Data<GroupNodeEntity>>> onSuccess, Action<HttpError> onError) {
            if (groupNodesInRAM.ContainsKey(id)) {
                onSuccess?.Invoke(groupNodesInRAM[id]);
                return;
            }

            requester.Get(id,
                (response) => {
                    if (response == null || response.data == null || response.data.attributes.sub_groups_nodes == null || response.data.attributes.sub_groups_nodes.Length == 0) {
                        HttpError httpError = funcHttpError.Invoke();
                        httpError.error.details = $"Invalid {nameof(StrapiResponse<Data<GroupNodeEntity>>)}:\n{response}";
                        onError?.Invoke(httpError);
                        return;
                    }

                    groupNodesInRAM.Add(id, response);
                    onSuccess(response);
                },
                onError
            );
        }

        public void Populate(Data<SubGroupNodesEntity> subGroupNodesEntity, Action<Data<SubGroupNodesEntity>> onSuccess, Action<HttpError> onError) {
            if (subGroupNodesEntity == null) {
                HttpError httpError = funcHttpError.Invoke();
                httpError.error.details = $"Invalid {nameof(Data<SubGroupNodesEntity>)}:\n{subGroupNodesEntity}";
                onError?.Invoke(httpError);
                return;
            }

            subGroupNodesService.Get(subGroupNodesEntity.id,
                (response) => {
                    subGroupNodesEntity.attributes = response.data.attributes;
                    onSuccess?.Invoke(subGroupNodesEntity);
                },
                onError
            );
        }

        public void Populate(Data<NodeEntity> nodeEntity, Action<Data<NodeEntity>> onSuccess, Action<HttpError> onError) {
            if (nodeEntity == null) {
                HttpError httpError = funcHttpError.Invoke();
                httpError.error.details = $"Invalid {nameof(Data<NodeEntity>)}:\n{nodeEntity}";
                onError?.Invoke(httpError);
                return;
            }

            nodeService.Get(nodeEntity.id,
                (response) => {
                    nodeEntity.attributes = response.data.attributes;
                    onSuccess?.Invoke(nodeEntity);
                },
                onError
            );
        }
    }
}