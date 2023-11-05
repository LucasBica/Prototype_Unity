using System;
using System.Collections.Generic;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class SubGroupNodesService : ISubGroupNodesService {

        private readonly ISubGroupNodesRequester requester;
        private readonly Func<HttpError> funcHttpError;
        private readonly Dictionary<int, StrapiResponse<Data<SubGroupNodesEntity>>> subGroupNodesInRAM = new(4);

        public SubGroupNodesService(ISubGroupNodesRequester requester, Func<HttpError> funcHttpError) {
            this.requester = requester;
            this.funcHttpError = funcHttpError;
        }

        public void Get(int id, Action<StrapiResponse<Data<SubGroupNodesEntity>>> onSuccess, Action<HttpError> onError) {
            if (subGroupNodesInRAM.ContainsKey(id)) {
                onSuccess?.Invoke(subGroupNodesInRAM[id]);
                return;
            }

            requester.Get(id,
            (response) => {
                if (response == null || response.data == null || response.data.attributes.nodes == null || response.data.attributes.nodes.Length == 0) {
                    HttpError httpError = funcHttpError.Invoke();
                    httpError.error.details = $"Invalid {nameof(StrapiResponse<Data<GroupNodeEntity>>)}:\n{response}";
                    onError?.Invoke(httpError);
                    return;
                }
                subGroupNodesInRAM.Add(id, response);
                onSuccess(response);
            },
            onError
           );
        }
    }
}