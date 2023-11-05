using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class SubGroupNodesEntity : EntityData {

        public string title;
        public string description;
        public MediaFile file;
        public Data<NodeEntity>[] nodes;
    }
}