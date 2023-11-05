using System;

using LB.Http.Models;
using LB.Http.Runtime.Strapi;

namespace LB.Topics.Runtime {

    public class TopicsService : ITopicsService {

        private readonly ITopicsRequester topicRequester;

        public TopicsService(ITopicsRequester topicRequester) {
            this.topicRequester = topicRequester;
        }

        public void GetAllTopics(Action<StrapiResponse<Data<TopicEntity>[]>> onSuccess, Action<HttpError> onError) {
            topicRequester.GetAllTopics(onSuccess, onError);
        }
    }
}