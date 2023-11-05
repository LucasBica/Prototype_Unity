using System;

using LB.Authentication.Runtime;
using LB.Http.Models;
using LB.Http.Runtime;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class GroupNodesRequester : IGroupNodesRequester {

        private readonly string host;
        private readonly IHttpService httpService;
        private readonly IAuthenticationService authenticationService;

        public GroupNodesRequester(string host, IHttpService httpService, IAuthenticationService authenticationService) {
            this.host = host;
            this.httpService = httpService;
            this.authenticationService = authenticationService;
        }

        public void Get(int id, Action<StrapiResponse<Data<GroupNodeEntity>>> onSuccess, Action<HttpError> onError) {
            Request request = new RequestBuilder(HttpMethodTypes.Get, host + $"group-nodes/{id}?populate=*")
                .SetHeaders(authenticationService.AuthorizationHeader)
                .Build();

            httpService.SendRequest(request, onSuccess, onError);
        }
    }
}