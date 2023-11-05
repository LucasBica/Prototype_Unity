using System;

using LB.Http.Runtime.Strapi;

namespace LB.Topics.Runtime {

    public interface ITopicSelectedListener {

        public bool CanPerform(Data<TopicEntity> topicEntity);

        public void Perform(Data<TopicEntity> topicEntity, Action onCompleted);
    }
}