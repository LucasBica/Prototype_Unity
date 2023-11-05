using System;
using System.Collections;
using System.IO;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace LB.Mvc.Runtime {

    public class SceneLoadingController : Controller<SceneLoadingModel>, ISceneLoadingController {

        private readonly IMessenger messenger;
        private readonly ICoroutineService coroutineService;

        private readonly Action<SceneLoadingModel> actionPreLoad;
        private readonly Action<SceneLoadingModel> actionPostLoad;

        public SceneLoadingController(IMessenger messenger, ICoroutineService coroutineService, Action<SceneLoadingModel> actionPreLoad, Action<SceneLoadingModel> actionPostLoad) {
            this.messenger = messenger;
            this.coroutineService = coroutineService;
            this.actionPreLoad = actionPreLoad;
            this.actionPostLoad = actionPostLoad;
        }

        public void OnDidAppear(SceneLoadingModel model) {
            LoadItOrNext(model);
        }

        private void LoadItOrNext(SceneLoadingModel model) {
            if (!string.IsNullOrEmpty(model.sceneToLoad)) {
                int index = GetSceneIndexByName(model.sceneToLoad);
                if (index > 0 && index < SceneManager.sceneCountInBuildSettings) {
                    LoadScene(model, index);
                    return;
                }
            }

            if (model.shouldTryLoadNext) {
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) {
                    LoadScene(model, nextSceneIndex);
                    return;
                }
            }

            Debug.LogError($"[{nameof(SceneLoadingController)}] Scene not found: '{model.sceneToLoad}'. {nameof(SceneLoadingModel)}.{nameof(model.shouldTryLoadNext)} is {model.shouldTryLoadNext} and {nameof(SceneManager.sceneCountInBuildSettings)} is {SceneManager.sceneCountInBuildSettings}.");
        }

        private void LoadScene(SceneLoadingModel model, int sceneIndex) {
            actionPreLoad?.Invoke(model);
            CallOnPropertyChanged(model);

            coroutineService.StartCoroutine(LoadScene(sceneIndex,
                () => {
                    actionPostLoad?.Invoke(model);
                    model.shouldDisappear = true;
                    CallOnPropertyChanged(model);
                })
            );
        }

        private IEnumerator LoadScene(int sceneIndex, Action onCompleted) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

            while (!asyncLoad.isDone) {
                yield return null;
            }

            yield return null; // The first frame after loading a scene, take too time that expected, so, I prefer wait one more frame.
            onCompleted?.Invoke();
        }

        private string GetSceneNameByIndex(int index) {
            string path = SceneUtility.GetScenePathByBuildIndex(index);
            return Path.GetFileNameWithoutExtension(path);
        }

        private int GetSceneIndexByName(string name) {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
                string sceneNameByIndex = GetSceneNameByIndex(i);
                if (sceneNameByIndex == name) return i;
            }

            return -1;
        }
    }
}