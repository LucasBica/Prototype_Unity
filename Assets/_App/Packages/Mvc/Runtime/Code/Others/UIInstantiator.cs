using System;
using System.Collections.Generic;

using LB.MessageSystem.Runtime;
using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.Mvc.Runtime {

    public class UIInstantiator : UIView {

        [Header("Settings")]
        [SerializeField] protected bool attachOnAwake = default;
        [SerializeField] protected bool attachOnEnable = default;
        [SerializeField] protected bool attachOnStart = default;

        [Header("Assets")]
        [SerializeField] protected UIBaseViewUpdater[] prefabs = default;

        [Header("References")]
        [SerializeField] protected RectTransform rectContainer = default;

        private readonly Dictionary<int, Action<IMessage>> dictionary = new();
        private readonly Dictionary<int, UIBaseViewUpdater> instances = new();

        private void OnDestroy() {
            for (int i = 0; i < prefabs.Length; i++) {
                if (dictionary.ContainsKey(i)) {
                    prefabs[i].DetachIt(dictionary[i]);
                }
            }
        }

        private void Awake() {
            if (attachOnAwake) {
                AttachAll();
            }
        }

        private void OnEnable() {
            if (attachOnEnable) {
                AttachAll();
            }
        }

        private void Start() {
            if (attachOnStart) {
                AttachAll();
            }
        }

        private void AttachAll() {
            for (int i = 0; i < prefabs.Length; i++) {
                int index = i;
                dictionary.Add(i, (message) => AppearView(message, index));
                prefabs[i].AttachIt(dictionary[i]);
            }
        }

        private void AppearView(IMessage message, int index) {
            if (!instances.TryGetValue(index, out UIBaseViewUpdater viewUpdater)) {
                viewUpdater = Instantiate(prefabs[index], rectContainer, false);
                instances.Add(index, viewUpdater);
            }

            viewUpdater.UpdateView(message);
        }
    }
}