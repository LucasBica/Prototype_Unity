using System;

using LB.Core.Runtime;
using LB.FileLoader.Runtime;
using LB.Http.Runtime.Strapi;
using LB.Topics.Runtime.Mvc;
using LB.UIKit.Runtime.Components;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LB.Topics.Runtime {

    public class UITopic : UIView {

        [Header("References (optional)")]
        [SerializeField] private SwitchGameObjects switchGameObjects = default;
        [SerializeField] private TMP_Text textTitle = default;
        [SerializeField] private TMP_Text textDescription = default;
        [SerializeField] private Image imageIllustration = default;
        [SerializeField] private UIButton buttonPlay = default;
        [SerializeField] private UIButton buttonLearn = default;

        private IFileLoaderService fileLoaderService;

        public event Action<UITopic> OnClickStart;
        public event Action<UITopic> OnClickInfo;

        public Data<TopicEntity> Entity { get; private set; }

        private void Awake() {
            fileLoaderService = DIContainer.Get<IFileLoaderService>();

            if (buttonPlay != null) buttonPlay.OnClick += ButtonPlay_OnClick;
            if (buttonLearn != null) buttonLearn.OnClick += ButtonLearn_OnClick;
        }

        public void UpdateContent(TopicSelectionModel model, Data<TopicEntity> entity) {
            Entity = entity;

            if (switchGameObjects != null) switchGameObjects.SetState(model.isLoading && model.topicSelected != null && entity == model.topicSelected);
            if (textTitle != null) textTitle.text = entity.attributes.title;
            if (textDescription != null) textDescription.text = entity.attributes.description;

            if (imageIllustration != null && entity.attributes.file != null && entity.attributes.file.data != null) {
                fileLoaderService.GetFromCacheOrDownload(entity.attributes.file, (response) => imageIllustration.overrideSprite = fileLoaderService.BytesToSprite(response), Debug.LogError);
            }
        }

        private void ButtonPlay_OnClick(UIButton button, PointerEventData eventData) {
            OnClickStart?.Invoke(this);
        }

        private void ButtonLearn_OnClick(UIButton button, PointerEventData eventData) {
            OnClickInfo?.Invoke(this);
        }
    }
}