using System;

using LB.Authentication.Runtime;
using LB.Http.Models;
using LB.Http.Runtime;
using LB.Http.Runtime.Strapi;

namespace LB.Topics.Runtime {

    public class TopicsRequester : ITopicsRequester {

        private readonly string host;
        private readonly IHttpService httpService;
        private readonly IAuthenticationService authenticationService;

        public TopicsRequester(string host, IHttpService httpService, IAuthenticationService authenticationService) {
            this.host = host;
            this.httpService = httpService;
            this.authenticationService = authenticationService;
        }

        public void GetAllTopics(Action<StrapiResponse<Data<TopicEntity>[]>> onSuccess, Action<HttpError> onError) {
            Request request = new RequestBuilder(HttpMethodTypes.Get, host + "topics?populate=*")
                .AddHeaders(authenticationService.AuthorizationHeader)
                .Build();

            httpService.SendRequest(request, onSuccess, onError);
        }
    }
}