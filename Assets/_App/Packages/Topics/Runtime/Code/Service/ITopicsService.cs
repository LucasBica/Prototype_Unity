using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.Topics.Runtime {

    public interface ITopicsService {

        public void GetAllTopics(Action<StrapiResponse<Data<TopicEntity>[]>> onSuccess, Action<HttpError> onError);
    }
}