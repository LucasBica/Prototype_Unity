using System;
using System.Collections.Generic;

namespace LB.MessageSystem.Runtime {

    public class Messenger : IMessenger {

        private readonly bool isDevelopmentBuild;
        private readonly Dictionary<Type, IChannelBase> channels = new();

        public Messenger(bool isDevelopmentBuild) {
            this.isDevelopmentBuild = isDevelopmentBuild;
        }

        private GenericChannel<T> GetGenericChannel<T>() {
            Type messageType = typeof(T);
            GenericChannel<T> channel;

            if (channels.TryGetValue(messageType, out IChannelBase value)) {
                channel = value as GenericChannel<T>;
            } else {
                channel = new GenericChannel<T>(isDevelopmentBuild);
                channels.Add(messageType, channel);
            }

            return channel;
        }

        public void Attach<T>(T messageType, Action<IMessage<T>> handler) {
            GetGenericChannel<T>().Attach(messageType, handler);
        }

        public void Attach(string messageType, Action<IMessage<string>> handler) {
            GetGenericChannel<string>().Attach(messageType, handler);
        }

        public void Detach<T>(T messageType, Action<IMessage<T>> handler) {
            GetGenericChannel<T>().Detach(messageType, handler);
        }

        public void Detach(string messageType, Action<IMessage<string>> handler) {
            messageType = messageType.ToLower();
            GetGenericChannel<string>().Detach(messageType, handler);
        }

        public void Send<T>(T messageType, object content = null, bool isSenderAsync = false) {
            GetGenericChannel<T>().Send(messageType, content, isSenderAsync);
        }

        public void Send(string messageType, object content = null, bool isSenderAsync = false) {
            GetGenericChannel<string>().Send(messageType, content, isSenderAsync);
        }
    }

    public enum UnityMessageTypes {
        None,
        OnApplicationFocus,
        OnApplicationPause,
        OnApplicationQuit,
        SceneLoaded,
    }
}