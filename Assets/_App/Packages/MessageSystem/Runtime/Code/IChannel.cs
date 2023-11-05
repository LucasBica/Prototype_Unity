using System;

namespace LB.MessageSystem.Runtime {

    public interface IChannel<T> : IChannelBase {

        public void Attach(T messageType, Action<IMessage<T>> handler);

        public void Detach(T messageType, Action<IMessage<T>> handler);

        public bool Send(T messageType, object content = null, bool isSenderAsync = false);

        public bool Send(IMessage<T> message);
    }
}