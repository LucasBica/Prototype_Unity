using System;

using LB.Http.Models;
using LB.Http.Runtime;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class NodesRequester : INodesRequester {

        private readonly string host;
        private readonly IHttpService httpService;

        public NodesRequester(string host, IHttpService httpService) {
            this.host = host;
            this.httpService = httpService;
        }

        public void Get(int id, Action<StrapiResponse<Data<NodeEntity>>> onSuccess, Action<HttpError> onError) {
            Request request = new RequestBuilder(HttpMethodTypes.Get, host + $"NodeEntity/{id}?populate=*")
                .Build();

            httpService.SendRequest(request, onSuccess, onError);
        }
    }
}