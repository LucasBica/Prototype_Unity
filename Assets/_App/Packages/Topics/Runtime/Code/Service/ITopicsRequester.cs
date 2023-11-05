using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.Topics.Runtime {

    public interface ITopicsRequester {

        public void GetAllTopics(Action<StrapiResponse<Data<TopicEntity>[]>> onSuccess, Action<HttpError> onError);
    }
}