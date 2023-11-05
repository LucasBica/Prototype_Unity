using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace LB.ScriptTemplate.Editor {

    public class ScriptCreatorViewEditor : EditorWindow {

        private ScriptCreatorScriptableObject scriptCreatorScriptableObject;
        private Dictionary<string, string> keyValues = new();
        private string[] keys = new string[0];

        [MenuItem("LB/Window/" + nameof(ScriptCreatorViewEditor))]
        public static void ShowWindow() {
            EditorWindow editorWindow = GetWindow(typeof(ScriptCreatorViewEditor));
            editorWindow.titleContent.text = nameof(ScriptCreatorViewEditor);
        }

        protected virtual void OnGUI() {
            GUILayout.Label("Settings", EditorStyles.boldLabel);

            ScriptCreatorScriptableObject scriptable = EditorGUILayout.ObjectField($"{nameof(scriptable)}", scriptCreatorScriptableObject, typeof(ScriptCreatorScriptableObject), false) as ScriptCreatorScriptableObject;

            if (scriptCreatorScriptableObject != scriptable) {
                scriptCreatorScriptableObject = scriptable;
                keyValues.Clear();

                if (scriptable != null) {
                    keys = scriptable.GetKeys();
                    for (int i = 0; i < keys.Length; i++) {
                        keyValues.Add(keys[i], string.Empty);
                    }
                } else {
                    keys = new string[0];
                }
            }

            for (int i = 0; i < keys.Length; i++) {
                keyValues[keys[i]] = EditorGUILayout.TextField(keys[i], keyValues[keys[i]]);
            }

            if (scriptable != null) {
                scriptable.SetValues(keyValues);
            }

            EditorGUI.BeginDisabledGroup(scriptable == null || !scriptable.IsDoneToCreateScripts());

            if (GUILayout.Button("Create Scripts")) {
                scriptable.CreateScripts();
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();

                scriptCreatorScriptableObject = null;
                keyValues.Clear();
                keys = new string[0];
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.LabelField($"{nameof(Application)}.{nameof(Application.dataPath)}: {Application.dataPath}");
        }
    }
}