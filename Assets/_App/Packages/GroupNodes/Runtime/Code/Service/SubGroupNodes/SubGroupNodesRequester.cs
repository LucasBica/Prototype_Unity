using System;

using LB.Http.Models;
using LB.Http.Runtime;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class SubGroupNodesRequester : ISubGroupNodesRequester {

        private readonly string host;
        private readonly IHttpService httpService;

        public SubGroupNodesRequester(string host, IHttpService httpService) {
            this.host = host;
            this.httpService = httpService;
        }

        public void Get(int id, Action<StrapiResponse<Data<SubGroupNodesEntity>>> onSuccess, Action<HttpError> onError) {
            Request request = new RequestBuilder(HttpMethodTypes.Get, host + $"SubGroupNodeEntity/{id}?populate=*")
                .Build();

            httpService.SendRequest(request, onSuccess, onError);
        }
    }
}