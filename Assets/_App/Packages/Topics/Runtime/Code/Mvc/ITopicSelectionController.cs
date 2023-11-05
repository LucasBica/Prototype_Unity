using LB.Mvc.Runtime;

namespace LB.Topics.Runtime.Mvc {

    public interface ITopicSelectionController : IController<TopicSelectionModel> {

        public void AddTopicSelectedListener(ITopicSelectedListener listener);

        public void RemoveTopicSelectedListener(ITopicSelectedListener listener);

        public void OnClickAppear(TopicSelectionModel model);

        public void OnClickStart(TopicSelectionModel model);

        public void OnClickInfo(TopicSelectionModel model);

        public void OnClickBack(TopicSelectionModel model);
    }
}