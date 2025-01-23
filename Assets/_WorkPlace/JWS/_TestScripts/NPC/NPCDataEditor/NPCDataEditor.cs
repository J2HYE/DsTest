using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NonePlayerCharacter))]
public class NPCDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // SerializedObject 업데이트
        serializedObject.Update();

        // 기본 필드 표시
        SerializedProperty npctypeProperty = serializedObject.FindProperty("npcType");
        SerializedProperty isMainQuestProperty = serializedObject.FindProperty("isMainQuest");
        SerializedProperty mainIndexProperty = serializedObject.FindProperty("mainIndex");

        EditorGUILayout.LabelField("▣ NPC 설정");
        EditorGUILayout.PropertyField(npctypeProperty, new GUIContent("      npcType"));
        EditorGUILayout.PropertyField(isMainQuestProperty, new GUIContent("      isMainQuest"));

        // "isMainQuest"가 true일 때만 드롭다운 표시
        if (isMainQuestProperty.boolValue)
        {
            // 드롭다운 항목 생성
            string[] indexOptions = new string[11]; // "None" + 1~10
            indexOptions[0] = "None";
            for (int i = 1; i <= 10; i++)
            {
                indexOptions[i] = i.ToString();
            }

            // 드롭다운 UI 표시
            int selectedIndex = mainIndexProperty.intValue;
            selectedIndex = EditorGUILayout.Popup("      mainIndex", selectedIndex, indexOptions);

            // 선택 결과 반영
            if (selectedIndex != mainIndexProperty.intValue)
            {
                mainIndexProperty.intValue = selectedIndex;
            }
        }

        serializedObject.ApplyModifiedProperties();


        // 읽기 전용 처리
        GUI.enabled = false;
        // NonePlayerCharacter와 NPCData 참조 가져오기
        NonePlayerCharacter holder = (NonePlayerCharacter)target;
        SerializedProperty npcDataProperty = serializedObject.FindProperty("currentNPCData");

        if (npcDataProperty == null ||
            string.IsNullOrEmpty(npcDataProperty.FindPropertyRelative("id").stringValue) ||
            string.IsNullOrEmpty(npcDataProperty.FindPropertyRelative("name").stringValue))
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            style.fontSize = 20;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleCenter;
            GUILayout.Space(10);
            EditorGUILayout.LabelField("▣ 할당된 NPC정보가 없습니다!", style);
            GUILayout.Space(10);
            return;
        }

        // NPCData 내부 속성 접근
        SerializedProperty idProperty = npcDataProperty.FindPropertyRelative("id");
        SerializedProperty nameProperty = npcDataProperty.FindPropertyRelative("name");
        SerializedProperty descriptionProperty = npcDataProperty.FindPropertyRelative("description");
        SerializedProperty npcTypeProperty = npcDataProperty.FindPropertyRelative("npcType");
        SerializedProperty spriteProperty = npcDataProperty.FindPropertyRelative("sprite");
        SerializedProperty gameobjectProperty = npcDataProperty.FindPropertyRelative("currentNPC");
        SerializedProperty itemsProperty = npcDataProperty.FindPropertyRelative("items");
        SerializedProperty questsProperty = npcDataProperty.FindPropertyRelative("quests");
        SerializedProperty patrolPointsProperty = npcDataProperty.FindPropertyRelative("patrolPoints");
        SerializedProperty voiceLinesProperty = npcDataProperty.FindPropertyRelative("voiceLines");
        SerializedProperty interactionEffectProperty = npcDataProperty.FindPropertyRelative("interactionEffect");

        // 기본 필드 표시
        EditorGUILayout.LabelField("▣ Base Settings");
        EditorGUILayout.PropertyField(idProperty, new GUIContent("      ID"));
        EditorGUILayout.PropertyField(nameProperty, new GUIContent("      Name"));
        EditorGUILayout.PropertyField(descriptionProperty, new GUIContent("      Description"));
        EditorGUILayout.PropertyField(npcTypeProperty, new GUIContent("      NPC Type"));
        EditorGUILayout.PropertyField(spriteProperty, new GUIContent("      Sprite"));
        EditorGUILayout.PropertyField(gameobjectProperty, new GUIContent("      CurrentNPC"));

        // 조건부 필드 표시
        NPCType npcType = (NPCType)npcTypeProperty.enumValueIndex;

        if (npcType == NPCType.상점)
        {
            EditorGUILayout.LabelField("▣ Shop Settings");
            SerializedProperty isShopProperty = npcDataProperty.FindPropertyRelative("isShop");
            EditorGUILayout.PropertyField(isShopProperty, new GUIContent("      Is Shop"));

            EditorGUILayout.PropertyField(itemsProperty, new GUIContent("      Shop Items"), true); // 배열 필드
        }

        if (npcType == NPCType.퀘스트)
        {
            EditorGUILayout.LabelField("▣ Quest Settings");
            SerializedProperty isQuestGiverProperty = npcDataProperty.FindPropertyRelative("isQuestGiver");
            EditorGUILayout.PropertyField(isQuestGiverProperty, new GUIContent("      Is Quest Giver"));

            EditorGUILayout.PropertyField(questsProperty, new GUIContent("      Quests"), true); // 배열 필드
        }

        SerializedProperty canMoveProperty = npcDataProperty.FindPropertyRelative("canMove");
        if (canMoveProperty.boolValue)
        {
            EditorGUILayout.LabelField("▣ Movement Settings");
            SerializedProperty moveSpeedProperty = npcDataProperty.FindPropertyRelative("moveSpeed");
            EditorGUILayout.PropertyField(moveSpeedProperty, new GUIContent("      Move Speed"));

            EditorGUILayout.PropertyField(patrolPointsProperty, new GUIContent("      Patrol Points"), true); // 배열 필드
        }

        SerializedProperty hasScheduleProperty = npcDataProperty.FindPropertyRelative("hasSchedule");
        if (hasScheduleProperty.boolValue)
        {
            EditorGUILayout.LabelField("▣ Schedule Settings");
            SerializedProperty activeTimeProperty = npcDataProperty.FindPropertyRelative("activeTime");
            EditorGUILayout.PropertyField(activeTimeProperty, new GUIContent("      Active Time"));
        }

        // 효과 관련 필드
        if (voiceLinesProperty != null && voiceLinesProperty.isArray)
        {
            EditorGUILayout.PropertyField(voiceLinesProperty, new GUIContent("      Voice Lines"), true);
        }

        if (interactionEffectProperty != null)
        {
            EditorGUILayout.PropertyField(interactionEffectProperty, new GUIContent("      Interaction Effect"));
        }
    }
}