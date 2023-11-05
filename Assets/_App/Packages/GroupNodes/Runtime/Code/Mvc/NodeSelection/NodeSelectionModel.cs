using LB.Http.Runtime.Strapi;
using LB.Mvc.Runtime;

namespace LB.GroupNodes.Runtime {

    public class NodeSelectionModel : ViewUpdaterModel {

        public Data<NodeEntity> nodeSelected;
        public Data<GroupNodeEntity> entity;
    }
}