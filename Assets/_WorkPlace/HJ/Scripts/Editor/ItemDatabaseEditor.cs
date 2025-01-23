using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(ItemList))]
public class ItemDatabaseEditor : Editor
{
    private SerializedProperty itemsProperty;   //아이템 데이터베이스의 items
    private bool[] foldouts;                    //토글

    private void OnEnable()
    {
        itemsProperty = serializedObject.FindProperty("itemList");
        foldouts = new bool[itemsProperty.arraySize];
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //레이블 표시
        EditorGUILayout.LabelField("Item Database", EditorStyles.boldLabel);

        //리스트 추가 버튼
        if (GUILayout.Button("Add New Item"))
        {
            AddNewItem();
        }


        //아이템 리스트를 박스 안에 표시
        EditorGUILayout.BeginVertical("box"); //박스 시작
        GUILayout.Space(10); //박스 위쪽 여백

        for (int i = 0; i < itemsProperty.arraySize; i++)
        {
            SerializedProperty itemProperty = itemsProperty.GetArrayElementAtIndex(i);
            string itemId = itemProperty.FindPropertyRelative("id").stringValue;

            //왼쪽, 오른쪽 여백 추가
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15); //왼쪽 여백

            EditorGUILayout.BeginVertical(); //아이템 컨텐츠
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], itemId); //아이템 id 표시
            if (foldouts[i])
            {
                DrawItemEditor(itemProperty, i);
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(15); //오른쪽 여백
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10); //박스 아래쪽 여백
        EditorGUILayout.EndVertical(); //박스를 종료


        serializedObject.ApplyModifiedProperties();
    }

    private void AddNewItem()
    {
        itemsProperty.arraySize++;
        foldouts = new bool[itemsProperty.arraySize];
    }

    private void DrawItemEditor(SerializedProperty itemProperty, int index)
    {
        //아이템 공통항목
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("id"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("name"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("description"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("type"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("grade"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("costValue"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("quantity"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("maxStack"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("isDiscardable"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("isStackable"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("sprite"));
        EditorGUILayout.PropertyField(itemProperty.FindPropertyRelative("dropChance"));

        //아이템 타입
        ItemType itemType = (ItemType)itemProperty.FindPropertyRelative("type").enumValueIndex;
        ConsumableType consumableType = (ConsumableType)itemProperty.FindPropertyRelative("consumableType").enumValueIndex;

        //아이템 타입에 따른 동적 속성
        switch (itemType)
        {
            case ItemType.무기:
            case ItemType.장신구:
                DrawPropertyIfExists(itemProperty, "effect");
                DrawPropertyIfExists(itemProperty, "itemStat");
                DrawPropertyIfExists(itemProperty, "durability");
                break;
            case ItemType.방어구:
                DrawPropertyIfExists(itemProperty, "effect");
                DrawPropertyIfExists(itemProperty, "itemStat");
                DrawPropertyIfExists(itemProperty, "durability");
                DrawPropertyIfExists(itemProperty, "equipmentSlot");
                break;
            case ItemType.소모품:
                DrawPropertyIfExists(itemProperty, "effect");
                DrawPropertyIfExists(itemProperty, "consumableType");
                DrawPropertyIfExists(itemProperty, "effectAmount");

                if (consumableType == ConsumableType.버프)
                {
                    DrawPropertyIfExists(itemProperty, "itemStat");
                }
                break;
            case ItemType.퀘스트:
                DrawPropertyIfExists(itemProperty, "questId");
                break;
        }

        GUILayout.Space(10);

        // 아이템 복사 버튼
        if (GUILayout.Button("Duplicate Item"))
        {
            DuplicateItem(index);
        }

        //아이템 삭제 버튼
        if (GUILayout.Button("Remove Item"))
        {
            itemsProperty.DeleteArrayElementAtIndex(index);
        }
    }

    private void DrawPropertyIfExists(SerializedProperty parentProperty, string propertyName)
    {
        SerializedProperty property = parentProperty.FindPropertyRelative(propertyName);
        if (property != null)
        {
            EditorGUILayout.PropertyField(property);
        }
    }

    private void DuplicateItem(int index)
    {
        itemsProperty.InsertArrayElementAtIndex(index); // 배열에 새로운 요소 추가
        SerializedProperty newItem = itemsProperty.GetArrayElementAtIndex(index + 1); // 새로 추가된 아이템
        SerializedProperty sourceItem = itemsProperty.GetArrayElementAtIndex(index); // 복사 원본 아이템

        CopySerializedProperty(sourceItem, newItem); // 속성 복사
        foldouts = new bool[itemsProperty.arraySize]; // 폴드아웃 갱신
    }

    private void CopySerializedProperty(SerializedProperty source, SerializedProperty destination)
    {
        object sourceValue = source.GetValue();
        destination.SetValue(sourceValue);
        destination.serializedObject.ApplyModifiedProperties();
    }
}

public static class SerializedPropertyExtensions
{
    public static object GetValue(this SerializedProperty property)
    {
        object obj = property.serializedObject.targetObject;
        string[] path = property.propertyPath.Split('.');
        foreach (string part in path)
        {
            FieldInfo field = obj.GetType().GetField(part, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null) return null;
            obj = field.GetValue(obj);
        }
        return obj;
    }

    public static void SetValue(this SerializedProperty property, object value)
    {
        object obj = property.serializedObject.targetObject;
        string[] path = property.propertyPath.Split('.');
        for (int i = 0; i < path.Length - 1; i++)
        {
            FieldInfo field = obj.GetType().GetField(path[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null) return;
            obj = field.GetValue(obj);
        }
        FieldInfo targetField = obj.GetType().GetField(path[^1], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (targetField != null)
        {
            targetField.SetValue(obj, value);
        }
    }
}
