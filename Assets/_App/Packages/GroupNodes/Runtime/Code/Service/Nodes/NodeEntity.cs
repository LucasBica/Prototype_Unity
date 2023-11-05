using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class NodeEntity : EntityData {

        public string title;
        public string description;
        public string data_id;
        public string data_type;
        public MediaFile file;
        public Data<RewardEntity>[] rewards;
    }
}