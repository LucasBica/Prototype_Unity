using System;

using LB.Core.Runtime;
using LB.Core.Runtime.Utilities;
using LB.FileLoader.Runtime;
using LB.Http.Runtime.Strapi;
using LB.UIKit.Runtime.Components;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace LB.GroupNodes.Runtime.Components {

    public class UISubGroupNodes : UIView {

        [Header("Assets")]
        [SerializeField] private UINode prefabNode = default;

        [Header("References")]
        [SerializeField] private TMP_Text textTitle = default;
        [SerializeField] private TMP_Text textDescription = default;
        [SerializeField] private Image imageIllustration = default;
        [SerializeField] private RectTransform rectContent = default;

        private IFileLoaderService fileLoaderService;
        private GameObjectPooling<UINode> gameObjectPooling;

        public event Action<UISubGroupNodes, UINode> OnClickNode;

        public Data<SubGroupNodesEntity> Entity { get; private set; }

        private void Awake() {
            fileLoaderService = DIContainer.Get<IFileLoaderService>();

            gameObjectPooling = new(prefabNode, rectContent);
            gameObjectPooling.OnCreate += GameObjectPooling_OnCreate;
            gameObjectPooling.OnActive += GameObjectPooling_OnActive;
            gameObjectPooling.OnRelease += GameObjectPooling_OnRelease;
        }

        public void UpdateContent(NodeSelectionModel model, Data<SubGroupNodesEntity> entity) {
            Entity = entity;

            if (textTitle != null) textTitle.text = entity.attributes.title;
            if (textDescription != null) textDescription.text = entity.attributes.description;
            if (imageIllustration != null && entity.attributes.file != null && entity.attributes.file.data != null) {
                fileLoaderService.GetFromCacheOrDownload(entity.attributes.file, (response) => imageIllustration.overrideSprite = fileLoaderService.BytesToSprite(response), Debug.LogError);
            }

            Data<NodeEntity>[] entities = entity.attributes.nodes;
            UINode[] activeInstances;

            if (gameObjectPooling.ActiveCount < entities.Length) {
                int difference = entities.Length - gameObjectPooling.ActiveCount;
                for (int i = 0; i < difference; i++) {
                    UINode instance = gameObjectPooling.GetInstance();
                    instance.RectT.SetAsLastSibling();
                }

            } else if (gameObjectPooling.ActiveCount > entities.Length) {
                int difference = gameObjectPooling.ActiveCount - entities.Length;
                for (int i = 0; i < difference; i++) {
                    activeInstances = gameObjectPooling.ActiveInstances;
                    gameObjectPooling.ReleaseInstance(activeInstances[activeInstances.Length - (difference + 1)]);
                }
            }

            activeInstances = gameObjectPooling.ActiveInstances;

            for (int i = 0; i < activeInstances.Length; i++) {
                activeInstances[i].UpdateContent(model, entities[i]);
            }
        }

        private void GameObjectPooling_OnCreate(UINode node) {
            node.OnClick += Node_OnClick;
        }

        private void GameObjectPooling_OnActive(UINode node) {
            node.gameObject.SetActive(true);
        }

        private void GameObjectPooling_OnRelease(UINode node) {
            node.gameObject.SetActive(false);
        }

        private void Node_OnClick(UINode node) {
            OnClickNode?.Invoke(this, node);
        }
    }
}