using LB.Mvc.Runtime;

namespace LB.GroupNodes.Runtime {

    public interface INodeSelectionController : IController<NodeSelectionModel> {

        public void OnClickNode(NodeSelectionModel model);

        public void OnClickBack(NodeSelectionModel model);
    }
}