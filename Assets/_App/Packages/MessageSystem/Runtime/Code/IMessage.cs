using System;
using System.Collections.Generic;

using LB.Core.Runtime.Utilities;

using UnityEngine;

namespace LB.MessageSystem.Runtime {

    public interface IMessage {

        public object Content { get; }
        public bool IsSenderAsync { get; }

        public bool GetContent<T>(out T content);

        public static bool GetContent<T>(IMessage message, out T content) {
            if (message == null) {
                content = default;
                Debug.LogError($"[{nameof(IMessage)}.{nameof(GetContent)}] {nameof(message)} is null");
                return false;
            }

            return message.GetContent(out content);
        }
    }

    public interface IMessage<T> : IMessage {

        public T MessageType { get; }
        public IChannel<T> Channel { get; }

        public void SetChannel(IChannel<T> channel);
    }

    public class Message<T> : IMessage<T> {

        public T MessageType { get; private set; }
        public object Content { get; private set; }
        public bool IsSenderAsync { get; private set; }
        public IChannel<T> Channel { get; set; }

        public Message(T messageType, object content = null, bool isSenderAsync = false) {
            MessageType = messageType;
            Content = content;
            IsSenderAsync = isSenderAsync;
        }

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }

        public void SetChannel(IChannel<T> channel) {
            if (Channel == null || channel == null) {
                return;
            }

            Channel = channel;
        }

        public bool GetContent<TContent>(out TContent content) {
            bool isCastable = Content is TContent;

            if (isCastable) {
                content = (TContent)Content;
                return true;
            }

            content = default;
            Debug.LogError($"[{nameof(Message<T>)}.{nameof(GetContent)}] Error Casting: {Content} to {typeof(TContent).FullName}");
            return false;
        }
    }

    public sealed class TwoValues<T1, T2> {
        public T1 Value1 { get; private set; }
        public T2 Value2 { get; private set; }

        public TwoValues(T1 value1, T2 value2) {
            Value1 = value1;
            Value2 = value2;
        }

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }
    }

    public sealed class ItemsContainer<T> {

        private readonly List<T> items = new List<T>();

        public ItemsContainer(List<T> items = null) {
            if (items == null) {
                this.items = new List<T>();
            } else {
                this.items = items;
            }
        }

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }

        public void AddItem(T item) => items.Add(item);
        public T[] GetItems() => items.ToArray();
    }

    public sealed class CallbackContainer {

        public event Action OnCallback;

        public CallbackContainer(Action callback) {
            OnCallback = callback;
        }

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }

        public void ExecuteCallback() {
            OnCallback?.Invoke();
        }

        public void ClearCallback() {
            OnCallback = null;
        }
    }

    public sealed class CallbackContainer<T> {

        public event Action<T> OnCallback;

        public CallbackContainer(Action<T> callback) {
            OnCallback = callback;
        }

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }

        public void ExecuteCallback(T obj) {
            OnCallback?.Invoke(obj);
        }

        public void ClearCallback() {
            OnCallback = null;
        }
    }
}