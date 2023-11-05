using System.Collections.Generic;

using UnityEngine;

namespace LB.ScriptTemplate.Editor {

    [CreateAssetMenu(fileName = nameof(HttpServiceCreatorScriptable), menuName = ScriptTemplateConsts.PATH_ASSETS + nameof(HttpServiceCreatorScriptable))]
    public class HttpServiceCreatorScriptable : ScriptCreatorScriptableObject {

        [SerializeField] private TextAsset textAssetRequester = default;
        [SerializeField] private TextAsset textAssetIRequester = default;
        [SerializeField] private TextAsset textAssetService = default;
        [SerializeField] private TextAsset textAssetIService = default;
        [SerializeField] private TextAsset textAssetModel = default;
        [SerializeField] private TextAsset textAssetInstaller = default;

        private readonly HttpServiceCreatorModel model = new();
        protected override ScriptCreatorModel Model { get => model; }

        public override void SetValues(Dictionary<string, string> values) {
            base.SetValues(values);

            model.templateRequester = textAssetRequester.text;
            model.templateIRequester = textAssetIRequester.text;
            model.templateService = textAssetService.text;
            model.templateIService = textAssetIService.text;
            model.templateModel = textAssetModel.text;
            model.templateInstaller = textAssetInstaller.text;
        }
    }
}