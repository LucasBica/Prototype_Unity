using System;

using LB.Core.Runtime;
using LB.Core.Runtime.Utilities;
using LB.FileLoader.Runtime;
using LB.GroupNodes.Runtime.Components;
using LB.Http.Runtime.Strapi;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;
using LB.UIKit.Runtime.Components;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LB.GroupNodes.Runtime {

    public class UINodeSelectionView : UIViewUpdater<INodeSelectionController, NodeSelectionModel> {

        [Header("Assets")]
        [SerializeField] private UISubGroupNodes prefabSubGroupsNodes = default;

        [Header("References")]
        [SerializeField] private UIButton buttonBack = default;
        [SerializeField] private TMP_Text textTitle = default;
        [SerializeField] private TMP_Text textDescription = default;
        [SerializeField] private Image imageIllustration = default;
        [SerializeField] private ScrollRect scroll = default;

        private IFileLoaderService fileLoaderService;
        private GameObjectPooling<UISubGroupNodes> gameObjectPooling;

        protected override void Awake() {
            base.Awake();

            fileLoaderService = DIContainer.Get<IFileLoaderService>();

            gameObjectPooling = new(prefabSubGroupsNodes, scroll.content);
            gameObjectPooling.OnCreate += GameObjectPooling_OnCreate;
            gameObjectPooling.OnActive += GameObjectPooling_OnActive;
            gameObjectPooling.OnRelease += GameObjectPooling_OnRelease;

            buttonBack.OnClick += ButtonBack_OnClick;
        }

        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(GroupNodesCallbacks.UpdateNodeSelectonView, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(GroupNodesCallbacks.UpdateNodeSelectonView, action);
        }

        protected override INodeSelectionController GetController() => DIContainer.Get<INodeSelectionController>();

        public override void UpdateView(NodeSelectionModel model) {
            base.UpdateView(model);

            if (textTitle != null) textTitle.text = model.entity.attributes.title;
            if (textDescription != null) textDescription.text = model.entity.attributes.description;
            if (imageIllustration != null && model.entity.attributes.file != null && model.entity.attributes.file.data != null) {
                fileLoaderService.GetFromCacheOrDownload(model.entity.attributes.file, (response) => imageIllustration.overrideSprite = fileLoaderService.BytesToSprite(response), Debug.LogError);
            }

            Data<SubGroupNodesEntity>[] entities = model.entity.attributes.sub_groups_nodes;
            UISubGroupNodes[] activeInstances;
            bool resetScroll = gameObjectPooling.ActiveCount != entities.Length;

            if (gameObjectPooling.ActiveCount < entities.Length) {
                int difference = entities.Length - gameObjectPooling.ActiveCount;
                for (int i = 0; i < difference; i++) {
                    UISubGroupNodes instance = gameObjectPooling.GetInstance();
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
            resetScroll = resetScroll && activeInstances.Length > 0;

            if (resetScroll) scroll.content.anchoredPosition = Vector2.zero;

            for (int i = 0; i < activeInstances.Length; i++) {
                activeInstances[i].UpdateContent(model, entities[i]);
            }
        }

        private void GameObjectPooling_OnCreate(UISubGroupNodes subGroupNodes) {
            subGroupNodes.OnClickNode += SubGroupNodes_OnClickNode;
        }

        private void GameObjectPooling_OnActive(UISubGroupNodes subGroupNodes) {
            subGroupNodes.gameObject.SetActive(true);
        }

        private void GameObjectPooling_OnRelease(UISubGroupNodes subGroupNodes) {
            subGroupNodes.gameObject.SetActive(false);
        }

        private void SubGroupNodes_OnClickNode(UISubGroupNodes subGroupNodes, UINode node) {
            model.nodeSelected = node.Entity;
            Controller.OnClickNode(model);
        }

        private void ButtonBack_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickBack(model);
        }
    }
}