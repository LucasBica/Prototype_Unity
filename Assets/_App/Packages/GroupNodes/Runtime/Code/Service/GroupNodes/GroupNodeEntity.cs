using LB.Http.Runtime.Strapi;

namespace LB.GroupNodes.Runtime {

    public class GroupNodeEntity : EntityDataLocalizable {

        public string title;
        public string description;
        public MediaFile file;
        public Data<SubGroupNodesEntity>[] sub_groups_nodes;
    }
}