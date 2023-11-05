using System;
using System.Collections.Generic;

using UnityEngine;

namespace LB.MessageSystem.Runtime {

    public class GenericChannel<T> : IChannel<T> {

        private readonly bool isDevelopmentBuild;
        private bool globalUpdateAttached = false;
        private readonly Dictionary<T, List<Action<IMessage<T>>>> listeners = new();
        private readonly List<IMessage<T>> pendingMessages = new();

        public GenericChannel(bool isDevelopmentBuild) {
            this.isDevelopmentBuild = isDevelopmentBuild;
        }

        public void Attach(T messageType, Action<IMessage<T>> handler) {
            if (handler == null) {
                Debug.LogError($"[{nameof(GenericChannel<T>)}.{nameof(Attach)}] {nameof(Action<IMessage<T>>)} is null");
                return;
            }

            if (!listeners.ContainsKey(messageType)) {
                listeners.Add(messageType, new List<Action<IMessage<T>>>());
            }

            List<Action<IMessage<T>>> listenerList = listeners[messageType];

            if (listenerList.Contains(handler)) {
                Debug.LogWarning($"[{nameof(GenericChannel<T>)}.{nameof(Attach)}] Attached existant listener: {messageType}");
            } else {
                listenerList.Add(handler);
            }
        }

        public void Detach(T messageType, Action<IMessage<T>> handler) {
            if (!listeners.ContainsKey(messageType)) {
                Debug.LogWarning($"[{nameof(GenericChannel<T>)}.{nameof(Detach)}] Detached non-existant listener: {messageType}");
            } else {
                listeners[messageType].Remove(handler);
                if (listeners[messageType].Count == 0) {
                    listeners.Remove(messageType);
                }
            }
        }

        public bool Send(T messageType, object content = null, bool isSenderAsync = false) {
            return Send(new Message<T>(messageType, content, isSenderAsync));
        }

        public bool Send(IMessage<T> message) {
            if (message == null) {
                Debug.LogError($"[{nameof(GenericChannel<T>)}.{nameof(Send)}] {nameof(IMessage<T>)} is null");
                return false;
            }

            message.SetChannel(this);

            if (!message.IsSenderAsync && !listeners.ContainsKey(message.MessageType)) {
                return false;
            }

            if (message.IsSenderAsync) {
                if (!globalUpdateAttached) {
                    globalUpdateAttached = true;
                    MessageSystemInstance.Add(OnUpdateGlobal);
                }

                pendingMessages.Add(message);
            } else {
                SendMessage(message, listeners[message.MessageType].ToArray());
            }

            return true;
        }

        private void OnUpdateGlobal() {
            globalUpdateAttached = false;
            MessageSystemInstance.Remove(OnUpdateGlobal);

            IMessage<T>[] messagesToSend = pendingMessages.ToArray();

            for (int i = 0; i < messagesToSend.Length; i++) {
                IMessage<T> message = messagesToSend[i];

                if (!listeners.ContainsKey(message.MessageType)) {
                    continue;
                }

                Action<IMessage<T>>[] handlers = listeners[message.MessageType].ToArray();
                SendMessage(message, handlers);
            }

            pendingMessages.Clear();
        }

        private void SendMessage(IMessage<T> message, Action<IMessage<T>>[] handlers) { // handlers should be an Array to avoid a Detach when we are calling the listeners.
            for (int i = 0; i < handlers.Length; i++) { // We do not need check if this action or the message is null because we did it in the Attach and Send function.
                try {
                    if (isDevelopmentBuild) {
                        Debug.Log($"[{nameof(GenericChannel<T>)}<{typeof(T)}>] {nameof(SendMessage)} (type: {message.MessageType}, content: {message.Content})");
                    }
                    handlers[i](message);
                } catch (Exception ex) {
                    string error = $"[{nameof(GenericChannel<T>)}<{typeof(T)}>] {nameof(SendMessage)} (type: {message.MessageType}, content: {message.Content})\n";
                    if (ex == null) {
                        error += "Exception is null";
                    } else {
                        error += ex.ToString();
                    }

                    Debug.LogError(error);
                }
            }
        }

        public void DetachAll(bool clearPendings = true) {
            listeners.Clear();

            if (clearPendings) {
                pendingMessages.Clear();
            }
        }
    }
}