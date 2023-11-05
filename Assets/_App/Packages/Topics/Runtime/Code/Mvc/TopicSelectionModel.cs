using LB.Http.Runtime.Strapi;
using LB.Mvc.Runtime;

namespace LB.Topics.Runtime.Mvc {

    public class TopicSelectionModel : ViewUpdaterModel {

        public StrapiResponse<Data<TopicEntity>[]> strapiResponseTopic;
        public Data<TopicEntity> topicSelected;
    }
}