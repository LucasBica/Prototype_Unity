using System.Collections.Generic;

using UnityEngine;

namespace LB.ScriptTemplate.Editor {

    [CreateAssetMenu(fileName = nameof(MvcCreatorScriptable), menuName = ScriptTemplateConsts.PATH_ASSETS + nameof(MvcCreatorScriptable))]
    public class MvcCreatorScriptable : ScriptCreatorScriptableObject {

        [SerializeField] private TextAsset textAssetView = default;
        [SerializeField] private TextAsset textAssetModel = default;
        [SerializeField] private TextAsset textAssetController = default;
        [SerializeField] private TextAsset textAssetInterface = default;

        private readonly MvcCreatorModel model = new();
        protected override ScriptCreatorModel Model { get => model; }

        public override void SetValues(Dictionary<string, string> values) {
            base.SetValues(values);

            model.templateView = textAssetView.text;
            model.templateModel = textAssetModel.text;
            model.templateController = textAssetController.text;
            model.templateInterface = textAssetInterface.text;
        }
    }
}