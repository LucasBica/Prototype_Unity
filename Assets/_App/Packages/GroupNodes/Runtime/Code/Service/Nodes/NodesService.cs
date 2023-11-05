using System;
using System.Collections.Generic;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class NodesService : INodesService {

        private readonly INodesRequester requester;
        private readonly Func<HttpError> funcHttpError;
        private readonly Dictionary<int, StrapiResponse<Data<NodeEntity>>> nodesInRAM = new(4);

        public NodesService(INodesRequester requester, Func<HttpError> funcHttpError) {
            this.requester = requester;
            this.funcHttpError = funcHttpError;
        }

        public void Get(int id, Action<StrapiResponse<Data<NodeEntity>>> onSuccess, Action<HttpError> onError) {
            if (nodesInRAM.ContainsKey(id)) {
                onSuccess?.Invoke(nodesInRAM[id]);
                return;
            }

            requester.Get(id,
                (response) => {
                    if (response == null || response.data == null || response.data.attributes == null || string.IsNullOrEmpty(response.data.attributes.data_id) || string.IsNullOrEmpty(response.data.attributes.data_type) || response.data.attributes.rewards == null) { // We do not check if the length of rewards is major of zero because that could be zero and is not an error.
                        HttpError httpError = funcHttpError.Invoke();
                        httpError.error.details = $"Invalid {nameof(StrapiResponse<Data<GroupNodeEntity>>)}:\n{response}";
                        onError?.Invoke(httpError);
                        return;
                    }
                    nodesInRAM.Add(id, response);
                    onSuccess(response);
                }, onError
            );
        }
    }
}