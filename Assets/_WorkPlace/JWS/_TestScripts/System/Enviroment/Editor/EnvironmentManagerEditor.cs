using UnityEngine;
using UnityEditor;

namespace JWS
{
    [CustomEditor(typeof(EnvironmentManager))]
    public class EnvironmentManagerEditor : Editor
    {
        private Editor materialEditor;

        private SerializedProperty overrideSunColorProp;
        private SerializedProperty overrideFogColorProp;
        private SerializedProperty overrideCloudColorProp;

        private bool lightingFoldout = true;
        private bool windFoldout = true;
        private bool grassFoldout = true;
        private bool cloudsFoldout = true;

        private void OnEnable()
        {
            overrideSunColorProp = serializedObject.FindProperty("overrideSunColor");
            overrideFogColorProp = serializedObject.FindProperty("overrideFogColor");
            overrideCloudColorProp = serializedObject.FindProperty("overrideCloudColor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontStyle = FontStyle.Bold;

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.margin = new RectOffset(2, 2, 2, 2);
            boxStyle.padding = new RectOffset(5, 5, 5, 5);

            EnvironmentManager EMscript = (EnvironmentManager)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical(boxStyle);
            lightingFoldout = EditorGUILayout.Foldout(lightingFoldout, "Global Lighting", foldoutStyle);
            if (lightingFoldout)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("directionalLight"), new GUIContent("Directional Light"));

                EditorGUILayout.Space(10);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("timeOfDay"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("daySpeed"));

                EditorGUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                overrideSunColorProp.boolValue = EditorGUILayout.ToggleLeft("Sun", overrideSunColorProp.boolValue, GUILayout.Width(70));
                using (new EditorGUI.DisabledScope(!overrideSunColorProp.boolValue))
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("sunColorGradient"), GUIContent.none);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                overrideFogColorProp.boolValue = EditorGUILayout.ToggleLeft("Fog", overrideFogColorProp.boolValue, GUILayout.Width(70));
                using (new EditorGUI.DisabledScope(!overrideFogColorProp.boolValue))
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fogColorGradient"), GUIContent.none);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                overrideCloudColorProp.boolValue = EditorGUILayout.ToggleLeft("Clouds", overrideCloudColorProp.boolValue, GUILayout.Width(70));
                using (new EditorGUI.DisabledScope(!overrideCloudColorProp.boolValue))
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("scatteringColorGradient"), GUIContent.none);
                }
                Rect gradientRect = GUILayoutUtility.GetLastRect();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(gradientRect.x + 77);
                GUILayout.Label("☼", GUILayout.Width(20));
                GUILayout.FlexibleSpace();
                GUILayout.Label("☽", GUILayout.Width(20));
                GUILayout.Space(5);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical(boxStyle);
            windFoldout = EditorGUILayout.Foldout(windFoldout, "Wind", foldoutStyle);
            if (windFoldout)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("baseWindPower"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("baseWindSpeed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("burstsPower"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("burstsSpeed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("burstsScale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("microPower"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("microSpeed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("microFrequency"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical(boxStyle);
            grassFoldout = EditorGUILayout.Foldout(grassFoldout, "Grass", foldoutStyle);
            if (grassFoldout)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("renderDistance"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical(boxStyle);
            cloudsFoldout = EditorGUILayout.Foldout(cloudsFoldout, "Clouds", foldoutStyle);
            if (cloudsFoldout)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Altitude"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("volumeSamples"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("volumeSize"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloudsMaterial"));

                EditorGUILayout.Space(10);

                if (EMscript.cloudsMaterial != null)
                {
                    if (materialEditor == null || materialEditor.target != EMscript.cloudsMaterial)
                    {
                        materialEditor = Editor.CreateEditor(EMscript.cloudsMaterial);
                    }
                    materialEditor.DrawHeader();
                    materialEditor.OnInspectorGUI();
                }
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}