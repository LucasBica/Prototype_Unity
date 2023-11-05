using LB.Http.Runtime.Strapi;

namespace LB.Topics.Runtime {

    public class TopicEntity : EntityDataLocalizable {

        public string title;
        public string description;
        public string data_id;
        public string data_type;
        public MediaFile file;
    }
}