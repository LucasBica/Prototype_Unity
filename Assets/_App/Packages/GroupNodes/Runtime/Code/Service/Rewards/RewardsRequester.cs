using System;

using LB.Http.Models;
using LB.Http.Runtime;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class RewardsRequester : IRewardsRequester {

        private readonly string host;
        private readonly IHttpService httpService;

        public RewardsRequester(string host, IHttpService httpService) {
            this.host = host;
            this.httpService = httpService;
        }

        public void Get(Action<StrapiResponse<Data<RewardEntity>>> onSuccess, Action<HttpError> onError) {
            Request request = new RequestBuilder(HttpMethodTypes.Get, host + "RewardEntity?populate=*")
                .Build();

            httpService.SendRequest(request, onSuccess, onError);
        }
    }
}