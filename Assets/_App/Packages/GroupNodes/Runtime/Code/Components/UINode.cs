using System;

using LB.Core.Runtime;
using LB.FileLoader.Runtime;
using LB.Http.Runtime.Strapi;
using LB.UIKit.Runtime.Components;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LB.GroupNodes.Runtime.Components {

    public class UINode : UIView {

        [Header("Assets")]
        [SerializeField] private DictionarySerializable<string, Sprite> nodeSprites = default;

        [Header("References")]
        [SerializeField] private UIButton button = default;
        [SerializeField] private TMP_Text textTitle = default;
        [SerializeField] private TMP_Text textDescription = default;
        [SerializeField] private Image imageIllustration = default;
        [SerializeField] private Image imageIcon = default;

        private IFileLoaderService fileLoaderService;

        public event Action<UINode> OnClick;

        public Data<NodeEntity> Entity { get; private set; }

        private void Awake() {
            fileLoaderService = DIContainer.Get<IFileLoaderService>();
            button.OnClick += Button_OnClick;
        }

        public void UpdateContent(NodeSelectionModel model, Data<NodeEntity> entity) {
            Entity = entity;

            if (textTitle != null) textTitle.text = entity.attributes.title;
            if (textDescription != null) textDescription.text = entity.attributes.description;
            if (imageIllustration != null && entity.attributes.file != null && entity.attributes.file.data != null) {
                fileLoaderService.GetFromCacheOrDownload(entity.attributes.file, (response) => imageIllustration.overrideSprite = fileLoaderService.BytesToSprite(response), Debug.LogError);
            }

            if (imageIcon != null && nodeSprites.Dictionary.TryGetValue(entity.attributes.data_type, out Sprite sprite)) {
                imageIcon.sprite = sprite;
            }
        }

        private void Button_OnClick(UIButton button, PointerEventData eventData) {
            OnClick?.Invoke(this);
        }
    }
}