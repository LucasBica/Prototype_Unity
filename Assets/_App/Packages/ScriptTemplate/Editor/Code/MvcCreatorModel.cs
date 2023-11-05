using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace LB.ScriptTemplate.Editor {

    public abstract class ScriptCreatorModel {

        public string path;

        public string[] GetKeys() {
            List<string> keys = GetKeysList();
            keys.Add("path");
            return keys.ToArray();
        }

        public virtual void SetValues(Dictionary<string, string> values) {
            if (values.TryGetValue("path", out string path)) {
                this.path = Path.Combine(Application.dataPath, path);
            }
        }

        protected abstract List<string> GetKeysList();

        public abstract Dictionary<string, string> GetTemplateKeywords();

        public abstract TemplateModel[] GetTemplates();

        public abstract bool IsDone();
    }

    public class TemplateModel {

        public string filename;
        public string content;

        public TemplateModel(string filename, string content) {
            this.filename = filename;
            this.content = content;
        }
    }

    public class MvcCreatorModel : ScriptCreatorModel {

        public string baseScriptName;
        public string fullNamespace;

        public string templateView;
        public string templateModel;
        public string templateController;
        public string templateInterface;

        public string ViewName => $"UI{baseScriptName}View";
        public string ModelName => $"{baseScriptName}Model";
        public string ControllerName => $"{baseScriptName}Controller";
        public string InterfaceName => $"I{baseScriptName}Controller";

        protected override List<string> GetKeysList() {
            return new List<string> {
                "baseScriptName",
                "fullNamespace"
            };
        }

        public override void SetValues(Dictionary<string, string> values) {
            base.SetValues(values);

            if (values.TryGetValue("baseScriptName", out string baseScriptName)) {
                this.baseScriptName = baseScriptName;
            }

            if (values.TryGetValue("fullNamespace", out string fullNamespace)) {
                this.fullNamespace = fullNamespace;
            }
        }

        public override Dictionary<string, string> GetTemplateKeywords() {
            return new Dictionary<string, string> {
                {"#NAMESPACE#", fullNamespace },
                {"#VIEW#", ViewName },
                {"#MODEL#", ModelName },
                {"#CONTROLLER#", ControllerName },
                {"#INTERFACE#", InterfaceName }
            };
        }

        public override TemplateModel[] GetTemplates() {
            return new TemplateModel[] {
                new TemplateModel(ViewName, templateView),
                new TemplateModel(ModelName, templateModel),
                new TemplateModel(ControllerName, templateController),
                new TemplateModel(InterfaceName, templateInterface),
            };
        }

        public override bool IsDone() {
            if (string.IsNullOrEmpty(path)) return false;
            if (string.IsNullOrEmpty(baseScriptName)) return false;
            if (string.IsNullOrEmpty(fullNamespace)) return false;
            if (string.IsNullOrEmpty(templateView)) return false;
            if (string.IsNullOrEmpty(templateModel)) return false;
            if (string.IsNullOrEmpty(templateController)) return false;
            if (string.IsNullOrEmpty(templateInterface)) return false;
            return true;
        }
    }
}