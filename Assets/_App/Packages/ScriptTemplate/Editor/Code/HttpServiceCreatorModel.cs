using System.Collections.Generic;

namespace LB.ScriptTemplate.Editor {

    public class HttpServiceCreatorModel : ScriptCreatorModel {

        public string scriptNameSingular;
        public string scriptNamePlural;
        public string fullNamespace;

        public string templateRequester;
        public string templateIRequester;
        public string templateService;
        public string templateIService;
        public string templateModel;
        public string templateInstaller;

        public string RequesterName => $"{scriptNamePlural}Requester";
        public string IRequesterName => $"I{scriptNamePlural}Requester";
        public string ServiceName => $"{scriptNamePlural}Service";
        public string IServiceName => $"I{scriptNamePlural}Service";
        public string ModelName => $"{scriptNameSingular}Entity";
        public string InstallerName => $"{scriptNamePlural}Installer";

        protected override List<string> GetKeysList() {
            return new List<string> {
                "scriptNameSingular",
                "scriptNamePlural",
                "fullNamespace"
            };
        }

        public override void SetValues(Dictionary<string, string> values) {
            base.SetValues(values);

            if (values.TryGetValue("scriptNameSingular", out string scriptNameSingular)) {
                this.scriptNameSingular = scriptNameSingular;
            }

            if (values.TryGetValue("scriptNamePlural", out string scriptNamePlural)) {
                this.scriptNamePlural = scriptNamePlural;
            }

            if (values.TryGetValue("fullNamespace", out string fullNamespace)) {
                this.fullNamespace = fullNamespace;
            }
        }

        public override Dictionary<string, string> GetTemplateKeywords() {
            return new Dictionary<string, string> {
                {"#NAMESPACE#", fullNamespace },
                {"#REQUESTER#", RequesterName },
                {"#IREQUESTER#", IRequesterName },
                {"#SERVICE#", ServiceName },
                {"#ISERVICE#", IServiceName },
                {"#MODEL#", ModelName },
                {"#INSTALLER#", InstallerName },
            };
        }

        public override TemplateModel[] GetTemplates() {
            return new TemplateModel[] {
                new TemplateModel(RequesterName, templateRequester),
                new TemplateModel(IRequesterName, templateIRequester),
                new TemplateModel(ServiceName, templateService),
                new TemplateModel(IServiceName, templateIService),
                new TemplateModel(ModelName, templateModel),
                new TemplateModel(InstallerName, templateInstaller),
            };
        }

        public override bool IsDone() {
            if (string.IsNullOrEmpty(path)) return false;
            if (string.IsNullOrEmpty(scriptNameSingular)) return false;
            if (string.IsNullOrEmpty(scriptNamePlural)) return false;
            if (string.IsNullOrEmpty(fullNamespace)) return false;
            if (string.IsNullOrEmpty(templateRequester)) return false;
            if (string.IsNullOrEmpty(templateIRequester)) return false;
            if (string.IsNullOrEmpty(templateService)) return false;
            if (string.IsNullOrEmpty(templateIService)) return false;
            if (string.IsNullOrEmpty(templateModel)) return false;
            if (string.IsNullOrEmpty(templateInstaller)) return false;
            return true;
        }
    }
}