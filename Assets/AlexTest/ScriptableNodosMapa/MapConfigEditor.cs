#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapConfig))]
public class MapConfigEditor : Editor
{
    private SerializedProperty levels;
    private SerializedProperty minMaxNodesPerLevel;
    private SerializedProperty levelConfigs;
    private SerializedProperty possibleNodes;

    private bool[] levelFoldouts;

    private void OnEnable()
    {
        levels = serializedObject.FindProperty("levels");
        minMaxNodesPerLevel = serializedObject.FindProperty("minMaxNodesPerLevel");
        levelConfigs = serializedObject.FindProperty("levelConfigs");
        possibleNodes = serializedObject.FindProperty("possibleNodes");


        UpdateFoldoutArray();
    }

    private void UpdateFoldoutArray()
    {
        int levelCount = levels.intValue;
        if (levelFoldouts == null || levelFoldouts.Length != levelCount)
        {
            levelFoldouts = new bool[levelCount];
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(levels);
        UpdateFoldoutArray();

        EditorGUILayout.PropertyField(minMaxNodesPerLevel, true);
        EditorGUILayout.PropertyField(possibleNodes, true);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Level Configurations", EditorStyles.boldLabel);

        if (levelConfigs.arraySize != levels.intValue)
        {
            serializedObject.ApplyModifiedProperties();
            (target as MapConfig).OnValidate();
            serializedObject.Update();
        }

        for (int i = 0; i < levelConfigs.arraySize; i++)
        {
            SerializedProperty levelConfig = levelConfigs.GetArrayElementAtIndex(i);
            SerializedProperty levelName = levelConfig.FindPropertyRelative("levelName");
            SerializedProperty nodeChances = levelConfig.FindPropertyRelative("nodeChances");

            levelFoldouts[i] = EditorGUILayout.Foldout(levelFoldouts[i], levelName.stringValue, true);

            if (levelFoldouts[i])
            {
                EditorGUI.indentLevel++;

                for (int j = 0; j < nodeChances.arraySize; j++)
                {
                    SerializedProperty nodeChance = nodeChances.GetArrayElementAtIndex(j);
                    SerializedProperty type = nodeChance.FindPropertyRelative("type");
                    SerializedProperty enabled = nodeChance.FindPropertyRelative("enabled");
                    SerializedProperty spawnChance = nodeChance.FindPropertyRelative("spawnChance");

                    EditorGUILayout.BeginHorizontal();

                    // Checkbox para habilitar o deshabilitar el nodo
                    enabled.boolValue = EditorGUILayout.Toggle(enabled.boolValue, GUILayout.Width(20));

                    // Nombre del tipo de nodo
                    EditorGUILayout.LabelField(type.enumDisplayNames[type.enumValueIndex], GUILayout.Width(150));

                    // Si está deshabilitado, mostrar el slider de probabilidad
                    if (!enabled.boolValue)
                    {
                        spawnChance.floatValue = EditorGUILayout.Slider(spawnChance.floatValue, 0, 100);
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Siempre aparece", GUILayout.Width(300));
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
