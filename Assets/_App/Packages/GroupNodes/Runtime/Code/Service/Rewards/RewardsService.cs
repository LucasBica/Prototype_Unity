using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class RewardsService : IRewardsService {

        private readonly IRewardsRequester requester;

        public RewardsService(IRewardsRequester requester) {
            this.requester = requester;
        }

        public void Get(Action<StrapiResponse<Data<RewardEntity>>> onSuccess, Action<HttpError> onError) {
            requester.Get(onSuccess, onError);
        }
    }
}