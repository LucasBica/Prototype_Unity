using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public interface IRewardsRequester {

        public void Get(Action<StrapiResponse<Data<RewardEntity>>> onSuccess, Action<HttpError> onError);
    }
}