using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace LB.ScriptTemplate.Editor {

    public abstract class ScriptCreatorScriptableObject : ScriptableObject {

        protected abstract ScriptCreatorModel Model { get; }

        public virtual string[] GetKeys() {
            return Model.GetKeys();
        }

        public virtual void SetValues(Dictionary<string, string> values) {
            Model.SetValues(values);
        }

        public virtual bool IsDoneToCreateScripts() {
            return Model.IsDone();
        }

        public virtual void CreateScripts() {
            TemplateModel[] templates = Model.GetTemplates();
            Dictionary<string, string> keyValuePairs = Model.GetTemplateKeywords();

            for (int i = 0; i < templates.Length; i++) {
                string filename = templates[i].filename;
                string content = templates[i].content;

                foreach (KeyValuePair<string, string> kvp in keyValuePairs) {
                    content = content.Replace(kvp.Key, kvp.Value);
                }

                content = content.Replace("#SCRIPTNAME#", filename);

                CreateScript(Model.path, filename, content);
            }
        }

        protected virtual void CreateScript(string path, string scriptName, string content) {
            CreateFolderIfNotExist(path);
            string file = Path.Combine(path, $"{scriptName}.cs");

            if (File.Exists(file)) {
                Debug.LogError($"[{nameof(ScriptCreatorScriptableObject)}] File already exist: {file}");
                return;
            }

            StreamWriter streamWriter = File.CreateText(file);
            streamWriter.Write(content);
            streamWriter.Close();
        }

        protected virtual void CreateFolderIfNotExist(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }
    }
}