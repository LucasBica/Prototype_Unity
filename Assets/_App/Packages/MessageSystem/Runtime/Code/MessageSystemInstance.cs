using System;
using System.Collections.Generic;

using LB.Core.Runtime;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace LB.MessageSystem.Runtime {

    public class MessageSystemInstance : MonoBehaviourSingleton<MessageSystemInstance> {

        private IMessenger messenger;
        private readonly List<Action> actions = new();

        public static void Add(Action action) {
            if (Instance == null) {
                GameObject gameObject = new("[Message System]");
                gameObject.AddComponent<MessageSystemInstance>();
            }

            if (Instance.actions.Contains(action)) {
                Debug.LogError("[MessageSystem] Action duplicated");
                return;
            }

            Instance.actions.Add(action);
        }

        public static void Remove(Action action) {
            if (!Instance.actions.Contains(action)) {
                Debug.LogError("[MessageSystem] Action non-existant");
                return;
            }

            Instance.actions.Remove(action);
        }

        protected override void Awake() {
            base.Awake();
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void Update() {
            for (int i = actions.Count - 1; i >= 0; i--) {
                actions[i]?.Invoke();
            }
        }

        protected override MessageSystemInstance GetInstance() {
            return this;
        }

        protected override void Initialize() {
            messenger = DIContainer.Get<IMessenger>();
        }

        private void OnApplicationFocus(bool focus) {
            messenger.Send(UnityMessageTypes.OnApplicationFocus, focus);
        }

        private void OnApplicationPause(bool pause) {
            messenger.Send(UnityMessageTypes.OnApplicationPause, pause);
        }

        private void OnApplicationQuit() {
            messenger.Send(UnityMessageTypes.OnApplicationQuit);
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            messenger.Send(UnityMessageTypes.SceneLoaded, new TwoValues<Scene, LoadSceneMode>(scene, loadSceneMode));
        }
    }
}