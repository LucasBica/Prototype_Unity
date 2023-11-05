using System;

namespace LB.MessageSystem.Runtime {

    public interface IMessenger {

        public void Attach<T>(T messageType, Action<IMessage<T>> handler);

        public void Attach(string messageType, Action<IMessage<string>> handler);

        public void Detach<T>(T messageType, Action<IMessage<T>> handler);

        public void Detach(string messageType, Action<IMessage<string>> handler);

        public void Send<T>(T messageType, object content = null, bool isSenderAsync = false);

        public void Send(string messageType, object content = null, bool isSenderAsync = false);
    }
}